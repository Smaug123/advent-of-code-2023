namespace AdventOfCode2023

open System
open System.Collections.Generic
open System.Globalization

[<RequireQualifiedAccess>]
module Day12 =

    let rec solve
        (dict : Dictionary<int * int, uint64>)
        (line : ReadOnlySpan<char>)
        (groups : IReadOnlyList<int>)
        (remainingToFill : int)
        (currentGroupIndex : int)
        : uint64
        =
        if line.Length = 0 then
            if currentGroupIndex = groups.Count then
                LanguagePrimitives.GenericOne
            else
                LanguagePrimitives.GenericZero
        elif currentGroupIndex = groups.Count then
            if line.Contains '#' then
                LanguagePrimitives.GenericZero
            else
                LanguagePrimitives.GenericOne
        else

        match dict.TryGetValue ((line.Length, currentGroupIndex)) with
        | true, v -> v
        | false, _ ->

        if remainingToFill > line.Length then
            dict.Add ((line.Length, currentGroupIndex), LanguagePrimitives.GenericZero)
            LanguagePrimitives.GenericZero
        else

        match line.[0] with
        | '#' ->
            if currentGroupIndex >= groups.Count then
                LanguagePrimitives.GenericZero
            else
                let mutable isOk = true

                for i = 1 to groups.[currentGroupIndex] - 1 do
                    if isOk && (i >= line.Length || (line.[i] <> '#' && line.[i] <> '?')) then
                        isOk <- false

                if not isOk then
                    LanguagePrimitives.GenericZero
                else if groups.[currentGroupIndex] < line.Length then
                    if line.[groups.[currentGroupIndex]] = '#' then
                        LanguagePrimitives.GenericZero
                    else
                        solve
                            dict
                            (line.Slice (groups.[currentGroupIndex] + 1))
                            groups
                            (remainingToFill - groups.[currentGroupIndex] - 1)
                            (currentGroupIndex + 1)
                else
                    solve
                        dict
                        ReadOnlySpan<_>.Empty
                        groups
                        (remainingToFill - groups.[currentGroupIndex] - 1)
                        (currentGroupIndex + 1)
        | '.' -> solve dict (line.Slice 1) groups remainingToFill currentGroupIndex
        | '?' ->
            let afterMark = line.IndexOfAnyExcept ('?', '#')

            if afterMark >= 0 && groups.[currentGroupIndex] > afterMark then
                // this group would extend into a dot if this were filled in!
                let firstHash = line.IndexOf '#'

                if firstHash >= 0 && firstHash < afterMark then
                    // this group *is* filled in, contradiction
                    LanguagePrimitives.GenericZero
                else
                    solve dict (line.Slice afterMark) groups remainingToFill currentGroupIndex
            else

            let ifDot = solve dict (line.Slice 1) groups remainingToFill currentGroupIndex
            dict.TryAdd ((line.Length - 1, currentGroupIndex), ifDot) |> ignore

            let ifHash =
                if currentGroupIndex >= groups.Count then
                    LanguagePrimitives.GenericZero
                else

                let mutable isOk = true

                for i = 1 to groups.[currentGroupIndex] - 1 do
                    if isOk && (i >= line.Length || (line.[i] <> '#' && line.[i] <> '?')) then
                        isOk <- false

                if not isOk then
                    LanguagePrimitives.GenericZero
                else if groups.[currentGroupIndex] < line.Length then
                    if
                        groups.[currentGroupIndex] < line.Length
                        && line.[groups.[currentGroupIndex]] = '#'
                    then
                        LanguagePrimitives.GenericZero
                    else
                        solve
                            dict
                            (line.Slice (groups.[currentGroupIndex] + 1))
                            groups
                            (remainingToFill - groups.[currentGroupIndex] - 1)
                            (currentGroupIndex + 1)
                else
                    solve
                        dict
                        ReadOnlySpan<_>.Empty
                        groups
                        (remainingToFill - groups.[currentGroupIndex] - 1)
                        (currentGroupIndex + 1)

            let ans = ifDot + ifHash
            dict.TryAdd ((line.Length, currentGroupIndex), ans) |> ignore
            ans
        | _ ->
            if currentGroupIndex = groups.Count then
                LanguagePrimitives.GenericOne
            else
                LanguagePrimitives.GenericZero

    let part1 (s : string) =
        use mutable lines = StringSplitEnumerator.make '\n' s

        let mutable answer = 0uL
        let arr = ResizeArray ()

        let dict = Dictionary ()

        for line in lines do
            if not line.IsEmpty then
                arr.Clear ()
                use ints = StringSplitEnumerator.make' ',' (line.Slice (line.IndexOf ' ' + 1))

                for int in ints do
                    arr.Add (Int32.Parse (int, NumberStyles.None, CultureInfo.InvariantCulture))

                let remainingToFill =
                    let mutable ans = -1

                    for i = 0 to arr.Count - 1 do
                        ans <- ans + arr.[i] + 1

                    ans

                dict.Clear ()
                let solved = solve dict line arr remainingToFill 0
                answer <- answer + solved

        answer

    let part2 (s : string) =
        use mutable lines = StringSplitEnumerator.make '\n' s

        let mutable answer = 0uL
        let arr = ResizeArray ()

        let dict = Dictionary ()

        for line in lines do
            if not line.IsEmpty then
                arr.Clear ()
                let spaceIndex = line.IndexOf ' '

                for _ = 0 to 4 do
                    use ints = StringSplitEnumerator.make' ',' (line.Slice (spaceIndex + 1))

                    for int in ints do
                        arr.Add (Int32.Parse (int, NumberStyles.None, CultureInfo.InvariantCulture))

                let sliced = line.Slice(0, spaceIndex).ToString ()

                let line =
                    String.Concat (sliced, '?', sliced, '?', sliced, '?', sliced, '?', sliced)

                let remainingToFill =
                    let mutable ans = -1

                    for i = 0 to arr.Count - 1 do
                        ans <- ans + arr.[i] + 1

                    ans

                dict.Clear ()
                let solved = solve dict (line.AsSpan ()) arr remainingToFill 0
                answer <- answer + solved

        answer
