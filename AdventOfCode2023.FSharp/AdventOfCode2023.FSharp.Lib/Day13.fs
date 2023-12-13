namespace AdventOfCode2023

open System

[<RequireQualifiedAccess>]
module Day13 =

    let inline isPowerOf2 (i : uint32) =
        // https://stackoverflow.com/a/600306/126995
        (i &&& (i - 1ul)) = 0ul

    let rowToInt (row : ReadOnlySpan<char>) : uint32 =
        let mutable mult = 1ul
        let mutable answer = 0ul

        for c = row.Length - 1 downto 0 do
            if row.[c] = '#' then
                answer <- answer + mult

            mult <- mult * 2ul

        answer

    let colToInt (grid : ReadOnlySpan<char>) (rowLength : int) (colNum : int) =
        let mutable mult = 1ul
        let mutable answer = 0ul

        for i = grid.Count '\n' - 1 downto 0 do
            if grid.[i * (rowLength + 1) + colNum] = '#' then
                answer <- answer + mult

            mult <- mult * 2ul

        answer

    let verifyReflection (group : ResizeArray<'a>) (smaller : int) (bigger : int) : bool =
        let midPoint = (smaller + bigger) / 2

        let rec isOkWithin (curr : int) =
            if smaller + curr > midPoint then
                true
            else if group.[smaller + curr] = group.[bigger - curr] then
                isOkWithin (curr + 1)
            else
                false

        if not (isOkWithin 0) then
            false
        else

        smaller = 0 || bigger = group.Count - 1

    /// Find reflection among rows.
    /// Returns 0 to indicate "no answer".
    [<TailCall>]
    let rec findRow (banAnswer : uint32) (rows : ResizeArray<uint32>) (currentLine : int) : uint32 =
        if currentLine = rows.Count - 1 then
            0ul
        else
            let mutable answer = UInt32.MaxValue
            let mutable i = currentLine

            while i < rows.Count - 1 do
                i <- i + 1

                if currentLine % 2 <> i % 2 then
                    if rows.[i] = rows.[currentLine] then
                        if verifyReflection rows currentLine i then
                            let desiredAnswer = uint32 (((currentLine + i) / 2) + 1)

                            if desiredAnswer <> banAnswer then
                                answer <- uint32 desiredAnswer
                                i <- Int32.MaxValue

            if answer < UInt32.MaxValue then
                answer
            else
                findRow banAnswer rows (currentLine + 1)

    let render (rowBuf : ResizeArray<_>) (colBuf : ResizeArray<_>) (group : ReadOnlySpan<char>) =
        rowBuf.Clear ()
        colBuf.Clear ()
        let lineLength = group.IndexOf '\n'

        for col = 0 to lineLength - 1 do
            colBuf.Add (colToInt group lineLength col)

        for row in StringSplitEnumerator.make' '\n' group do
            if not row.IsEmpty then
                rowBuf.Add (rowToInt row)

    /// Returns 0 to indicate "no solution".
    let solve (banAnswer : uint32) (rowBuf : ResizeArray<_>) (colBuf : ResizeArray<_>) : uint32 =
        match
            findRow
                (if banAnswer >= 100ul then
                     banAnswer / 100ul
                 else
                     UInt32.MaxValue)
                rowBuf
                0
        with
        | rowIndex when rowIndex > 0ul -> 100ul * rowIndex
        | _ -> findRow banAnswer colBuf 0

    /// Returns also the group with this gro
    let peelGroup (s : ReadOnlySpan<char>) : ReadOnlySpan<char> =
        let index = s.IndexOf "\n\n"

        if index < 0 then
            // last group
            s
        else
            s.Slice (0, index + 1)

    let part1 (s : string) =
        let mutable s = s.AsSpan ()
        let rows = ResizeArray ()
        let cols = ResizeArray ()
        let mutable answer = 0ul

        while not s.IsEmpty do
            let group = peelGroup s

            render rows cols group
            // There's an obvious perf optimisation where we don't compute cols
            // until we know there's no row answer. Life's too short.
            answer <- answer + solve UInt32.MaxValue rows cols

            if group.Length >= s.Length then
                s <- ReadOnlySpan<char>.Empty
            else
                s <- s.Slice (group.Length + 1)

        answer

    let flipAt (rows : ResizeArray<_>) (cols : ResizeArray<_>) (rowNum : int) (colNum : int) : unit =
        rows.[rowNum] <-
            let index = 1ul <<< (cols.Count - colNum - 1)

            if rows.[rowNum] &&& index > 0ul then
                rows.[rowNum] - index
            else
                rows.[rowNum] + index

        cols.[colNum] <-
            let index = 1ul <<< (rows.Count - rowNum - 1)

            if cols.[colNum] &&& index > 0ul then
                cols.[colNum] - index
            else
                cols.[colNum] + index

    let part2 (s : string) =
        let mutable s = s.AsSpan ()
        let rows = ResizeArray ()
        let cols = ResizeArray ()
        let mutable answer = 0ul

        while not s.IsEmpty do
            let group = peelGroup s

            render rows cols group

            let bannedAnswer = solve UInt32.MaxValue rows cols

            let mutable isDone = false
            let mutable rowToChange = 0

            while not isDone && rowToChange < rows.Count do
                let mutable colToChange = 0

                while not isDone && colToChange < cols.Count do
                    flipAt rows cols rowToChange colToChange

                    match solve bannedAnswer rows cols with
                    | solved when solved > 0ul ->
                        isDone <- true
                        answer <- answer + solved
                    | _ ->
                        flipAt rows cols rowToChange colToChange
                        colToChange <- colToChange + 1

                rowToChange <- rowToChange + 1

            if group.Length >= s.Length then
                s <- ReadOnlySpan<char>.Empty
            else
                s <- s.Slice (group.Length + 1)

        answer
