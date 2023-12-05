namespace AdventOfCode2023

open System
open System.Globalization
open System.Runtime.CompilerServices

type EfficientString = System.ReadOnlySpan<char>

[<RequireQualifiedAccess>]
module EfficientString =

    let inline isEmpty (s : EfficientString) : bool = s.IsEmpty


    let inline ofString (s : string) : EfficientString = s.AsSpan ()

    let inline toString (s : EfficientString) : string = s.ToString ()

    let inline trimStart (s : EfficientString) : EfficientString = s.TrimStart ()

    let inline slice (start : int) (length : int) (s : EfficientString) : EfficientString = s.Slice (start, length)

    let inline equals (a : string) (other : EfficientString) : bool =
        MemoryExtensions.Equals (other, a.AsSpan (), StringComparison.Ordinal)

    /// Mutates the input to drop up to the first instance of the input char,
    /// and returns what was dropped.
    /// If the char is not present, deletes the input.
    let takeUntil<'a> (c : char) (s : EfficientString byref) : EfficientString =
        let first = s.IndexOf c

        if first < 0 then
            let toRet = s
            s <- EfficientString.Empty
            toRet
        else
            let toRet = slice 0 first s
            s <- slice (first + 1) (s.Length - first - 1) s
            toRet

[<Struct>]
[<IsByRefLike>]
type StringSplitEnumerator =
    internal
        {
            Original : EfficientString
            mutable Remaining : EfficientString
            mutable InternalCurrent : EfficientString
            SplitOn : char
        }

    interface IDisposable with
        member this.Dispose () = ()

    member this.Current : EfficientString = this.InternalCurrent

    member this.MoveNext () =
        if this.Remaining.Length = 0 then
            false
        else
            this.InternalCurrent <- EfficientString.takeUntil this.SplitOn &this.Remaining
            true

    member this.GetEnumerator () = this

[<RequireQualifiedAccess>]
module StringSplitEnumerator =

    let make (splitChar : char) (s : string) : StringSplitEnumerator =
        {
            Original = EfficientString.ofString s
            Remaining = EfficientString.ofString s
            InternalCurrent = EfficientString.Empty
            SplitOn = splitChar
        }

    let make' (splitChar : char) (s : ReadOnlySpan<char>) : StringSplitEnumerator =
        {
            Original = s
            Remaining = s
            InternalCurrent = EfficientString.Empty
            SplitOn = splitChar
        }

    let chomp (s : string) (e : byref<StringSplitEnumerator>) : unit =
#if DEBUG
        if not (e.MoveNext ()) || not (EfficientString.equals s e.Current) then
            failwithf "expected '%s', got '%s'" s (e.Current.ToString ())
#else
        e.MoveNext () |> ignore
#endif

    let consumeInt (e : byref<StringSplitEnumerator>) : int =
        if not (e.MoveNext ()) then
            failwith "expected an int, got nothing"

        Int32.Parse e.Current

    let consumeU32 (e : byref<StringSplitEnumerator>) : uint32 =
        if not (e.MoveNext ()) then
            failwith "expected an int, got nothing"

        UInt32.Parse e.Current

    let consumeU64 (e : byref<StringSplitEnumerator>) : uint64 =
        if not (e.MoveNext ()) then
            failwith "expected an int, got nothing"

        UInt64.Parse e.Current
