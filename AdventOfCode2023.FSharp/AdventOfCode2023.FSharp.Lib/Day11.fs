namespace AdventOfCode2023

[<RequireQualifiedAccess>]
module Day11 =

    type Data =
        {
            RowsWithoutGalaxies : ResizeArray<int>
            ColsWithoutGalaxies : ResizeArray<int>
            /// row * col
            Galaxies : ResizeArray<int * int>
        }

    let parse (s : string) : Data =
        let galaxies = ResizeArray ()
        let rowsWithoutGalaxies = ResizeArray ()
        let mutable hasAnyGalaxy = false
        let mutable currRowIndex = 0
        let mutable currColIndex = 0

        for c in s do
            if c = '\n' then
                if not hasAnyGalaxy then
                    rowsWithoutGalaxies.Add currRowIndex

                currRowIndex <- currRowIndex + 1
                currColIndex <- 0
                hasAnyGalaxy <- false
            elif c = '#' then
                hasAnyGalaxy <- true
                galaxies.Add (currRowIndex, currColIndex)
                currColIndex <- currColIndex + 1
            else
                currColIndex <- currColIndex + 1

        galaxies.Sort (fun (_, c1) (_, c2) -> compare c1 c2)

        let colsWithoutGalaxies =
            let result = ResizeArray ()
            let mutable prevCol = 0

            for _, c in galaxies do
                if c > prevCol then
                    for j = prevCol + 1 to c - 1 do
                        result.Add j

                    prevCol <- c

            result

        {
            RowsWithoutGalaxies = rowsWithoutGalaxies
            ColsWithoutGalaxies = colsWithoutGalaxies
            Galaxies = galaxies
        }

    let solve (data : Data) (expansion : uint64) =
        let mutable answer = 0uL

        for galaxy1 = 0 to data.Galaxies.Count - 1 do
            let row1, col1 = data.Galaxies.[galaxy1]

            for galaxy2 = galaxy1 + 1 to data.Galaxies.Count - 1 do
                let row2, col2 = data.Galaxies.[galaxy2]
                let baseDistance = uint64 (abs (row1 - row2) + abs (col1 - col2))

                let extraDistance =
                    let mutable extraDistance = 0uL

                    for i = 1 + min row1 row2 to max row1 row2 - 1 do
                        if data.RowsWithoutGalaxies.Contains i then
                            extraDistance <- extraDistance + expansion - 1uL

                    for i = 1 + min col1 col2 to max col1 col2 - 1 do
                        if data.ColsWithoutGalaxies.Contains i then
                            extraDistance <- extraDistance + expansion - 1uL

                    extraDistance

                answer <- answer + extraDistance + baseDistance

        answer


    let part1 (s : string) =
        let data = parse s

        solve data 2uL

    let part2 (s : string) =
        let data = parse s

        solve data 1_000_000uL
