namespace AdventOfCode2023

open System
open System.Globalization

[<RequireQualifiedAccess>]
module Day15 =

    let hash (s : ReadOnlySpan<char>) : int =
        let mutable v = 0

        for c in s do
            v <- v + int (byte c)
            v <- (17 * v) % 256

        v

    let part1 (s : string) =
        let s = s.AsSpan().TrimEnd ()
        use chunks = StringSplitEnumerator.make' ',' s
        let mutable answer = 0

        for chunk in chunks do
            answer <- answer + hash chunk

        answer

    let inline removeFirst<'a> ([<InlineIfLambda>] toRemove : 'a -> bool) (arr : ResizeArray<'a>) : unit =
        let mutable i = 0

        while i < arr.Count do
            if toRemove arr.[i] then
                for j = i to arr.Count - 2 do
                    arr.[j] <- arr.[j + 1]

                arr.RemoveAt (arr.Count - 1)
                i <- arr.Count

            i <- i + 1

    let inline replace
        ([<InlineIfLambda>] withKey : 'a -> 'key)
        (key : 'key)
        (value : 'a)
        (arr : ResizeArray<'a>)
        : unit
        =
        let mutable i = 0

        while i < arr.Count do
            if withKey arr.[i] = key then
                arr.[i] <- value
                i <- arr.Count

            i <- i + 1

        if i < arr.Count + 1 then
            // no replacement was made
            arr.Add value

    let inline getLength (labelAndLength : uint64) : uint32 =
        (labelAndLength % uint64 UInt32.MaxValue) |> uint32

    let inline getLabel (labelAndLength : uint64) : uint32 =
        (labelAndLength / uint64 UInt32.MaxValue) |> uint32

    let inline focusingPower (boxNumber : uint32) (arr : ResizeArray<_>) =
        let mutable answer = 0ul

        for i = 0 to arr.Count - 1 do
            answer <- answer + (boxNumber + 1ul) * (uint32 i + 1ul) * getLength arr.[i]

        answer

    let inline toUint32 (s : ReadOnlySpan<char>) : uint32 =
        let mutable answer = 0ul

        for c in s do
            answer <- answer * 26ul + uint32 (byte c - byte 'a')

        answer

    let inline pack (label : uint32) (focalLength : uint32) : uint64 =
        uint64 label * uint64 UInt32.MaxValue + uint64 focalLength

    let part2 (s : string) =
        let s = s.AsSpan().TrimEnd ()
        use chunks = StringSplitEnumerator.make' ',' s
        // The max length of a label turns out to be 6, which means we need 26^6 < 2^32 entries.
        // So we'll use a uint32 instead of our string, to save hopping around memory.
        // We'll also pack the focal length into the elements, to save tupling.
        let lenses = Array.init 256 (fun _ -> ResizeArray<uint64> ())

        for chunk in chunks do
            if chunk.[chunk.Length - 1] = '-' then
                let label = chunk.Slice (0, chunk.Length - 1)
                let labelShrunk = toUint32 label
                removeFirst (fun labelAndLength -> getLabel labelAndLength = labelShrunk) lenses.[hash label]
            else
                let equalsPos = chunk.IndexOf '='

                let focalLength =
                    UInt32.Parse (chunk.Slice (equalsPos + 1), NumberStyles.None, CultureInfo.InvariantCulture)

                let label = chunk.Slice (0, equalsPos)
                let labelShrunk = toUint32 label
                replace getLabel labelShrunk (pack labelShrunk focalLength) lenses.[hash label]

        let mutable answer = 0ul

        for i = 0 to 255 do
            answer <- answer + focusingPower (uint32 i) lenses.[i]

        answer
