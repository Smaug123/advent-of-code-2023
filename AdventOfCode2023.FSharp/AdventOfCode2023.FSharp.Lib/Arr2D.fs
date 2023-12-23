namespace AdventOfCode2023

#if DEBUG
#else
#nowarn "9"
#endif

open System
open Microsoft.FSharp.NativeInterop

[<Struct>]
#if DEBUG
type Arr2D<'a> =
    {
        Elements : 'a array
        Width : int
    }

    member this.Height = this.Elements.Length / this.Width
#else
type Arr2D<'a when 'a : unmanaged> =
    {
        Elements : nativeptr<'a>
        Length : int
        Width : int
    }

    member this.Height = this.Length / this.Width
#endif

[<RequireQualifiedAccess>]
module Arr2D =

    /// It's faster to iterate forward over the first argument, `x`.
    let inline get (arr : Arr2D<'a>) (x : int) (y : int) : 'a =
#if DEBUG
        arr.Elements.[y * arr.Width + x]
#else
        NativePtr.get arr.Elements (y * arr.Width + x)
#endif

    let inline set (arr : Arr2D<'a>) (x : int) (y : int) (newVal : 'a) : unit =
#if DEBUG
        arr.Elements.[y * arr.Width + x] <- newVal
#else
        NativePtr.write (NativePtr.add arr.Elements (y * arr.Width + x)) newVal
#endif

#if DEBUG
    let create (width : int) (height : int) (value : 'a) : Arr2D<'a> =
        let arr = Array.create (width * height) value

        {
            Width = width
            Elements = arr
        }
#else
    /// The input array must be at least of size width * height
    let create (arr : nativeptr<'a>) (width : int) (height : int) (value : 'a) : Arr2D<'a> =
        {
            Width = width
            Elements = arr
            Length = width * height
        }
#endif

    [<RequiresExplicitTypeArguments>]
#if DEBUG
    let zeroCreate<'a when 'a : unmanaged> (width : int) (height : int) : Arr2D<'a> =
        {
            Elements = Array.zeroCreate (width * height)
            Width = width
        }
#else
    let zeroCreate<'a when 'a : unmanaged> (elts : nativeptr<'a>) (width : int) (height : int) : Arr2D<'a> =
        {
            Elements = elts
            Width = width
            Length = width * height
        }
#endif

    /// The closure is given x and then y.
#if DEBUG
    let inline init (width : int) (height : int) (f : int -> int -> 'a) : Arr2D<'a> =
        let result = zeroCreate<'a> width height
#else
    let inline init (arr : nativeptr<'a>) (width : int) (height : int) (f : int -> int -> 'a) : Arr2D<'a> =
        let result = zeroCreate<'a> arr width height
#endif

        for y = 0 to height - 1 do
            for x = 0 to width - 1 do
                set result x y (f x y)

        result

    let inline clear (a : Arr2D<'a>) : unit =
#if DEBUG
        System.Array.Clear a.Elements
#else
        NativePtr.initBlock a.Elements 0uy (uint32 sizeof<'a> * uint32 a.Length)
#endif

    /// Pass in a buffer of memory which we will use entirely for our own purposes (and may resize)
    /// to maintain state.
    /// `empty` is the value in empty cells; we will fill them with `fillWith`.
    let floodFill
        (stackBuf : ResizeArray<int>)
        (s : Arr2D<'a>)
        (empty : 'a)
        (fillWith : 'a)
        (currX : int)
        (currY : int)
        : unit
        =
        stackBuf.Clear ()
        stackBuf.Add currX
        stackBuf.Add currY

        while stackBuf.Count > 0 do
            let currY = stackBuf.[stackBuf.Count - 1]
            stackBuf.RemoveAt (stackBuf.Count - 1)
            let currX = stackBuf.[stackBuf.Count - 1]
            stackBuf.RemoveAt (stackBuf.Count - 1)

            if currX > 0 then
                if get s (currX - 1) currY = empty then
                    set s (currX - 1) currY fillWith
                    stackBuf.Add (currX - 1)
                    stackBuf.Add currY

            if currX < s.Width - 1 then
                if get s (currX + 1) currY = empty then
                    set s (currX + 1) currY fillWith
                    stackBuf.Add (currX + 1)
                    stackBuf.Add currY

            if currY > 0 then
                if get s currX (currY - 1) = empty then
                    set s currX (currY - 1) fillWith
                    stackBuf.Add currX
                    stackBuf.Add (currY - 1)

            if currY < s.Height - 1 then
                if get s currX (currY + 1) = empty then
                    set s currX (currY + 1) fillWith
                    stackBuf.Add currX
                    stackBuf.Add (currY + 1)

    /// SIMD go brr
    let inline count< ^a when 'a : equality and 'a : unmanaged and 'a :> IEquatable<'a>>
        (arr : Arr2D<'a>)
        (x : 'a)
        : int
        =
        let span =
#if DEBUG
            arr.Elements.AsSpan ()
#else
            ReadOnlySpan<'a> (NativePtr.toVoidPtr arr.Elements, arr.Length)
#endif
        MemoryExtensions.Count (span, x)

    let print (arr : Arr2D<byte>) =
        for row = 0 to arr.Height - 1 do
            for col = 0 to arr.Width - 1 do
                match get arr col row with
                | 1uy -> printf "#"
                | 0uy -> printf "."
                | 2uy -> printf "O"
                | _ -> failwith "bad"

            printfn ""

        printfn ""
