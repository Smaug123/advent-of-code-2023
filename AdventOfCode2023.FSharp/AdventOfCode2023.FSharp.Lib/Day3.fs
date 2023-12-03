namespace AdventOfCode2023

open System.Collections.Generic

[<RequireQualifiedAccess>]
module Day3 =
    let inline private isSymbol (i : byte) = i > 200uy

    let inline private isGear (i : byte) = i = 255uy

    /// Returns the parsed board as a buffer, the length of the buffer (there may be garbage at the end), and
    /// the number of lines the resulting 2D array has.
    let parse (fileContents : byte[]) =
        let mutable lineCount = 0
        let mutable len = 0
        let resultArr = Array.zeroCreate fileContents.Length

        for b in fileContents do
            if b = byte '.' then
                resultArr.[len] <- 100uy
                len <- len + 1
            elif b = byte '*' then
                resultArr.[len] <- 255uy
                len <- len + 1
            elif byte '0' <= b && b <= byte '9' then
                resultArr.[len] <- b - byte '0'
                len <- len + 1
            elif b = 10uy then
                lineCount <- lineCount + 1
            else
                resultArr.[len] <- 254uy
                len <- len + 1

        resultArr, len, lineCount

    let part1 (contents : Arr2D<byte>) =
        let lineLength = contents.Width

        let isNearSymbol (row : int) (numStart : int) (curCol : int) : bool =
            let mutable isNearSymbol = false

            if row > 0 then
                for col = max (numStart - 1) 0 to min curCol (lineLength - 1) do
                    if isSymbol (Arr2D.get contents col (row - 1)) then
                        isNearSymbol <- true

            if row < contents.Height - 1 then
                for col = max (numStart - 1) 0 to min curCol (lineLength - 1) do
                    if isSymbol (Arr2D.get contents col (row + 1)) then
                        isNearSymbol <- true

            if
                (numStart > 0 && isSymbol (Arr2D.get contents (numStart - 1) row))
                || (curCol < lineLength && isSymbol (Arr2D.get contents curCol row))
            then
                isNearSymbol <- true

            isNearSymbol

        let mutable total = 0

        for row = 0 to contents.Height - 1 do
            let mutable currNum = 0
            let mutable numStart = -1

            for col = 0 to lineLength - 1 do
                if Arr2D.get contents col row < 10uy then
                    if numStart = -1 then
                        numStart <- col

                    currNum <- currNum * 10 + int (Arr2D.get contents col row)
                elif numStart > -1 then
                    if isNearSymbol row numStart col then
                        total <- total + currNum

                    currNum <- 0
                    numStart <- -1

            if numStart >= 0 then
                if isNearSymbol row numStart lineLength then
                    total <- total + currNum

                currNum <- 0
                numStart <- -1

        total

    let part2 (contents : Arr2D<byte>) =
        let lineLength = contents.Width

        let isNearGear (row : int) (numStart : int) (curCol : int) : (int * int) IReadOnlyList =
            let gearsNear = ResizeArray ()

            if row > 0 then
                for col = max (numStart - 1) 0 to min curCol (lineLength - 1) do
                    if isGear (Arr2D.get contents col (row - 1)) then
                        gearsNear.Add (row - 1, col)

            if row < lineLength - 1 then
                for col = max (numStart - 1) 0 to min curCol (lineLength - 1) do
                    if isGear (Arr2D.get contents col (row + 1)) then
                        gearsNear.Add (row + 1, col)

            if (numStart > 0 && isGear (Arr2D.get contents (numStart - 1) row)) then
                gearsNear.Add (row, numStart - 1)

            if (curCol < lineLength && isGear (Arr2D.get contents curCol row)) then
                gearsNear.Add (row, curCol)

            gearsNear

        let gears = Dictionary<int * int, ResizeArray<int>> ()

        let addGear (gearPos : int * int) (num : int) =
            match gears.TryGetValue gearPos with
            | false, _ ->
                let arr = ResizeArray ()
                arr.Add num
                gears.Add (gearPos, arr)
            | true, arr when arr.Count < 3 -> arr.Add num
            | _ -> ()

        for row = 0 to contents.Height - 1 do
            let mutable currNum = 0
            let mutable numStart = -1

            for col = 0 to lineLength - 1 do
                if Arr2D.get contents col row < 10uy then
                    if numStart = -1 then
                        numStart <- col

                    currNum <- currNum * 10 + int (Arr2D.get contents col row)
                elif numStart > -1 then
                    for gearPos in isNearGear row numStart col do
                        addGear gearPos currNum

                    currNum <- 0
                    numStart <- -1

            if numStart >= 0 then
                for gearPos in isNearGear row numStart lineLength do
                    addGear gearPos currNum

                currNum <- 0
                numStart <- -1

        let mutable answer = 0

        for KeyValue (_gearPos, gears) in gears do
            if gears.Count = 2 then
                answer <- answer + gears.[0] * gears.[1]

        answer
