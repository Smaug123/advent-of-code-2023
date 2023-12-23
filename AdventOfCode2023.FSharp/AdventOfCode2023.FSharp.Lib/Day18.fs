namespace AdventOfCode2023

#if DEBUG
#else
#nowarn "9"
#endif

open System
open System.Collections.Generic
open System.Globalization

[<RequireQualifiedAccess>]
module Day18 =

    let part1 (s : string) =
        // Interleaved row and col
        let mutable weHave = ResizeArray<int> ()
        weHave.Add 0
        weHave.Add 0

        let mutable minCol = 0
        let mutable maxCol = 0
        let mutable minRow = 0
        let mutable maxRow = 0

        let mutable col = 0
        let mutable row = 0
        let mutable edgeCount = 0
        use rows = StringSplitEnumerator.make '\n' s

        for currRow in rows do
            use mutable entries = StringSplitEnumerator.make' ' ' currRow
            entries.MoveNext () |> ignore
            let dir = Direction.ofChar entries.Current.[0]
            entries.MoveNext () |> ignore

            let distance =
                Int32.Parse (entries.Current, NumberStyles.None, CultureInfo.InvariantCulture)

            edgeCount <- edgeCount + distance

            match dir with
            | Direction.Down ->
                for _ = 1 to distance do
                    row <- row + 1
                    weHave.Add row
                    weHave.Add col

                maxRow <- max row maxRow
            | Direction.Up ->
                for _ = 1 to distance do
                    row <- row - 1
                    weHave.Add row
                    weHave.Add col

                minRow <- min row minRow
            | Direction.Left ->
                for _ = 1 to distance do
                    col <- col - 1
                    weHave.Add row
                    weHave.Add col

                minCol <- min col minCol
            | Direction.Right ->
                for _ = 1 to distance do
                    col <- col + 1
                    weHave.Add row
                    weHave.Add col

                maxCol <- max col maxCol
            | _ -> failwith "bad dir"

        let buffer = Array.zeroCreate ((maxRow - minRow + 3) * (maxCol - minCol + 3))
#if DEBUG
        let system : Arr2D<byte> =
            {
                Elements = buffer
                Width = maxCol - minCol + 3
            }
#else
        use ptr = fixed buffer

        let system : Arr2D<byte> =
            {
                Elements = ptr
                Length = buffer.Length
                Width = maxCol - minCol + 3
            }
#endif

        let mutable pointIndex = 0

        while pointIndex < weHave.Count do
            let row = weHave.[pointIndex]
            let col = weHave.[pointIndex + 1]
            Arr2D.set system (col - minCol + 1) (row - minRow + 1) 1uy
            pointIndex <- pointIndex + 2

        Arr2D.floodFill (ResizeArray ()) system 0uy 2uy 0 0

        Arr2D.count system 0uy + edgeCount

    let part2 (s : string) = -1
