namespace AdventOfCode2023

#if DEBUG
#else
#nowarn "9"
#endif

open System

[<RequireQualifiedAccess>]
module Day16 =

    let inline storeDirectionAndPos (numCols : int) (col : int) (row : int) (direction : Direction) : uint16 =
        4us * uint16 (col + numCols * row) + Direction.toUInt direction

    let inline getDirection (input : uint16) =
        match input % 4us with
        | 0us -> Direction.Left
        | 1us -> Direction.Right
        | 2us -> Direction.Up
        | 3us -> Direction.Down
        | _ -> failwith "bad"

    let inline getCol (numCols : int) (input : uint16) = (input / 4us) % uint16 numCols |> int

    let inline getRow (numCols : int) (input : uint16) = (input / 4us) / uint16 numCols |> int

    let inline maxEncoded (numCols : int) (numRows : int) : uint16 =
        4us * uint16 ((numCols - 1) + numCols * (numRows - 1)) + 3us

    let inline getAt (numCols : int) (s : string) (row : int) (col : int) = s.[row * (numCols + 1) + col]

    let advance (arr : Arr2D<_>) (going : ResizeArray<_>) (s : string) (nextUp : uint16) =
        let numCols = arr.Width
        let numLines = arr.Height
        let col = getCol numCols nextUp
        let row = getRow numCols nextUp
        let dir = getDirection nextUp
        Arr2D.set arr col row true

        match dir with
        | Direction.Right ->
            match getAt numCols s row col with
            | '-'
            | '.' ->
                if col < arr.Width - 1 then
                    going.[going.Count - 1] <- storeDirectionAndPos numCols (col + 1) row dir
                else
                    going.RemoveAt (going.Count - 1)
            | '/' ->
                if row > 0 then
                    going.[going.Count - 1] <- storeDirectionAndPos numCols col (row - 1) Direction.Up
                else
                    going.RemoveAt (going.Count - 1)
            | '\\' ->
                if row < numLines - 1 then
                    going.[going.Count - 1] <- storeDirectionAndPos numCols col (row + 1) Direction.Down
                else
                    going.RemoveAt (going.Count - 1)
            | '|' ->
                going.RemoveAt (going.Count - 1)

                if row < numLines - 1 then
                    going.Add (storeDirectionAndPos numCols col (row + 1) Direction.Down)

                if row > 0 then
                    going.Add (storeDirectionAndPos numCols col (row - 1) Direction.Up)
            | c -> failwith $"Unrecognised char: %c{c}"
        | Direction.Left ->
            match getAt numCols s row col with
            | '-'
            | '.' ->
                if col > 0 then
                    going.[going.Count - 1] <- storeDirectionAndPos numCols (col - 1) row dir
                else
                    going.RemoveAt (going.Count - 1)
            | '\\' ->
                if row > 0 then
                    going.[going.Count - 1] <- storeDirectionAndPos numCols col (row - 1) Direction.Up
                else
                    going.RemoveAt (going.Count - 1)
            | '/' ->
                if row < numLines - 1 then
                    going.[going.Count - 1] <- storeDirectionAndPos numCols col (row + 1) Direction.Down
                else
                    going.RemoveAt (going.Count - 1)
            | '|' ->
                going.RemoveAt (going.Count - 1)

                if row < numLines - 1 then
                    going.Add (storeDirectionAndPos numCols col (row + 1) Direction.Down)

                if row > 0 then
                    going.Add (storeDirectionAndPos numCols col (row - 1) Direction.Up)
            | c -> failwith $"Unrecognised char: %c{c}"
        | Direction.Up ->
            match getAt numCols s row col with
            | '|'
            | '.' ->
                if row > 0 then
                    going.[going.Count - 1] <- storeDirectionAndPos numCols col (row - 1) dir
                else
                    going.RemoveAt (going.Count - 1)
            | '/' ->
                if col < numCols - 1 then
                    going.[going.Count - 1] <- storeDirectionAndPos numCols (col + 1) row Direction.Right
                else
                    going.RemoveAt (going.Count - 1)
            | '\\' ->
                if col > 0 then
                    going.[going.Count - 1] <- storeDirectionAndPos numCols (col - 1) row Direction.Left
                else
                    going.RemoveAt (going.Count - 1)
            | '-' ->
                going.RemoveAt (going.Count - 1)

                if col < numCols - 1 then
                    going.Add (storeDirectionAndPos numCols (col + 1) row Direction.Right)

                if col > 0 then
                    going.Add (storeDirectionAndPos numCols (col - 1) row Direction.Left)
            | c -> failwith $"Unrecognised char: %c{c}"
        | Direction.Down ->
            match getAt numCols s row col with
            | '|'
            | '.' ->
                if row < arr.Height - 1 then
                    going.[going.Count - 1] <- storeDirectionAndPos numCols col (row + 1) dir
                else
                    going.RemoveAt (going.Count - 1)
            | '\\' ->
                if col < numCols - 1 then
                    going.[going.Count - 1] <- storeDirectionAndPos numCols (col + 1) row Direction.Right
                else
                    going.RemoveAt (going.Count - 1)
            | '/' ->
                if col > 0 then
                    going.[going.Count - 1] <- storeDirectionAndPos numCols (col - 1) row Direction.Left
                else
                    going.RemoveAt (going.Count - 1)
            | '-' ->
                going.RemoveAt (going.Count - 1)

                if col < numCols - 1 then
                    going.Add (storeDirectionAndPos numCols (col + 1) row Direction.Right)

                if col > 0 then
                    going.Add (storeDirectionAndPos numCols (col - 1) row Direction.Left)
            | c -> failwith $"Unrecognised char: %c{c}"
        | _ -> failwith "bad"

    let part1 (s : string) =
        let numLines = s.AsSpan().Count '\n'
        let numCols = s.IndexOf '\n'
        let buf = Array.zeroCreate (numLines * numCols)
#if DEBUG
        let arr : Arr2D<bool> =
            {
                Elements = buf
                Width = numCols
            }
#else
        use ptr = fixed buf

        let arr : Arr2D<bool> =
            {
                Elements = ptr
                Width = numCols
                Length = buf.Length
            }
#endif
        let going = ResizeArray ()

        going.Add (storeDirectionAndPos numCols 0 0 Direction.Right)

        let seen = Array.zeroCreate (int (maxEncoded numCols numLines) + 1)

        while going.Count > 0 do
            let nextUp = going.[going.Count - 1]

            match seen.[int nextUp] with
            | true -> going.RemoveAt (going.Count - 1)
            | false ->
                seen.[int nextUp] <- true
                advance arr going s nextUp

        buf.AsSpan().Count true

    let part2 (s : string) =
        let numLines = s.AsSpan().Count '\n'
        let numCols = s.IndexOf '\n'
        let buf = Array.zeroCreate (numLines * numCols)
#if DEBUG
        let arr : Arr2D<bool> =
            {
                Elements = buf
                Width = numCols
            }
#else
        use ptr = fixed buf

        let arr : Arr2D<bool> =
            {
                Elements = ptr
                Width = numCols
                Length = buf.Length
            }
#endif
        let going = ResizeArray ()
        let seen = Array.zeroCreate (int (maxEncoded numCols numLines) + 1)
        let mutable best = 0

        for start = 0 to numCols - 1 do
            going.Clear ()
            Array.Clear seen
            Array.Clear buf
            going.Add (storeDirectionAndPos numCols start 0 Direction.Down)

            while going.Count > 0 do
                let nextUp = going.[going.Count - 1]

                match seen.[int nextUp] with
                | true -> going.RemoveAt (going.Count - 1)
                | false ->
                    seen.[int nextUp] <- true
                    advance arr going s nextUp

            let lit = buf.AsSpan().Count true
            best <- max best lit

            going.Clear ()
            Array.Clear seen
            Array.Clear buf
            going.Add (storeDirectionAndPos numCols start (numLines - 1) Direction.Up)

            while going.Count > 0 do
                let nextUp = going.[going.Count - 1]

                match seen.[int nextUp] with
                | true -> going.RemoveAt (going.Count - 1)
                | false ->
                    seen.[int nextUp] <- true
                    advance arr going s nextUp

            let lit = buf.AsSpan().Count true
            best <- max best lit

        for start = 0 to numLines - 1 do
            going.Clear ()
            Array.Clear seen
            Array.Clear buf
            going.Add (storeDirectionAndPos numCols 0 start Direction.Right)

            while going.Count > 0 do
                let nextUp = going.[going.Count - 1]

                match seen.[int nextUp] with
                | true -> going.RemoveAt (going.Count - 1)
                | false ->
                    seen.[int nextUp] <- true
                    advance arr going s nextUp

            let lit = buf.AsSpan().Count true
            best <- max best lit

            going.Clear ()
            Array.Clear seen
            Array.Clear buf
            going.Add (storeDirectionAndPos numCols (numCols - 1) start Direction.Left)

            while going.Count > 0 do
                let nextUp = going.[going.Count - 1]

                match seen.[int nextUp] with
                | true -> going.RemoveAt (going.Count - 1)
                | false ->
                    seen.[int nextUp] <- true
                    advance arr going s nextUp

            let lit = buf.AsSpan().Count true
            best <- max best lit

        best
