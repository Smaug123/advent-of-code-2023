namespace AdventOfCode2023

#if DEBUG
#else
#nowarn "9"
#endif

open System

[<RequireQualifiedAccess>]
module Day10 =

    /// Returns first the line number, then the position within that line.
    /// lineLength includes the newline, as does pos.
    let inline private toRowAndCol (lineLength : int) (pos : int) : struct (int * int) =
        let lineNum = pos / lineLength
        let withinLine = pos % lineLength
        struct (lineNum, withinLine)

    let inline private ofRowAndCol (lineLength : int) (lineNum : int) (col : int) : int = lineNum * lineLength + col

    let inline nextPoint (s : ReadOnlySpan<char>) (lineLength : int) (currPos : int) (prevPos : int) =
        let struct (currLineNum, currCol) = toRowAndCol lineLength currPos
        let struct (prevLineNum, prevCol) = toRowAndCol lineLength prevPos

        match s.[currPos] with
        | '|' ->
            if prevLineNum < currLineNum then
                ofRowAndCol lineLength (currLineNum + 1) currCol
            else
                ofRowAndCol lineLength (currLineNum - 1) currCol
        | '-' ->
            if prevCol < currCol then
                ofRowAndCol lineLength currLineNum (currCol + 1)
            else
                ofRowAndCol lineLength currLineNum (currCol - 1)
        | 'L' ->
            if prevLineNum = currLineNum then
                ofRowAndCol lineLength (currLineNum - 1) currCol
            else
                ofRowAndCol lineLength currLineNum (currCol + 1)
        | '7' ->
            if prevLineNum = currLineNum then
                ofRowAndCol lineLength (currLineNum + 1) currCol
            else
                ofRowAndCol lineLength currLineNum (currCol - 1)
        | 'F' ->
            if prevLineNum = currLineNum then
                ofRowAndCol lineLength (currLineNum + 1) currCol
            else
                ofRowAndCol lineLength currLineNum (currCol + 1)
        | 'J' ->
            if prevLineNum = currLineNum then
                ofRowAndCol lineLength (currLineNum - 1) currCol
            else
                ofRowAndCol lineLength currLineNum (currCol - 1)
        | c -> failwithf "unrecognised: %c" c

    let part1 (s : string) =
        let s = s.AsSpan ()
        let lineLength = (s.IndexOf '\n' + 1)
        let startPos = s.IndexOf 'S'

        let struct (startLine, startCol) = toRowAndCol lineLength startPos
        let mutable distance = 1
        let mutable prevPointA = startPos
        let mutable prevPointB = startPos

        let mutable pointA =
            let pos = ofRowAndCol lineLength startLine (startCol - 1)

            match s.[pos] with
            | '-'
            | 'L'
            | 'F' -> pos
            | _ ->

            let pos = ofRowAndCol lineLength startLine (startCol + 1)

            match s.[pos] with
            | '-'
            | 'J'
            | '7' -> pos
            | _ ->

            ofRowAndCol lineLength (startLine + 1) startCol

        let mutable pointB =
            let pos = ofRowAndCol lineLength (startLine - 1) startCol

            match if pos >= 0 then s.[pos] else 'n' with
            | '|'
            | '7'
            | 'F' -> pos
            | _ ->

            let pos = ofRowAndCol lineLength (startLine + 1) startCol

            match if pos < s.Length then s.[pos] else 'n' with
            | '|'
            | 'L'
            | 'J' -> pos
            | _ ->

            let pos = ofRowAndCol lineLength startLine (startCol + 1)

            match if pos < s.Length then s.[pos] else 'n' with
            | '-'
            | 'J'
            | '7' -> pos
            | _ -> ofRowAndCol lineLength startLine (startCol - 1)

        while pointA <> pointB do
            let currentA = pointA
            pointA <- nextPoint s lineLength pointA prevPointA
            prevPointA <- currentA

            let currentB = pointB
            pointB <- nextPoint s lineLength pointB prevPointB
            prevPointB <- currentB

            distance <- distance + 1

        distance

    let floodFill (stackBuf : ResizeArray<_>) (s : Arr2D<byte>) (currX : int) (currY : int) =
        stackBuf.Clear ()
        stackBuf.Add currX
        stackBuf.Add currY

        while stackBuf.Count > 0 do
            let currY = stackBuf.[stackBuf.Count - 1]
            stackBuf.RemoveAt (stackBuf.Count - 1)
            let currX = stackBuf.[stackBuf.Count - 1]
            stackBuf.RemoveAt (stackBuf.Count - 1)

            if currX > 0 then
                if Arr2D.get s (currX - 1) currY = 0uy then
                    Arr2D.set s (currX - 1) currY 2uy
                    stackBuf.Add (currX - 1)
                    stackBuf.Add currY

            if currX < s.Width - 1 then
                if Arr2D.get s (currX + 1) currY = 0uy then
                    Arr2D.set s (currX + 1) currY 2uy
                    stackBuf.Add (currX + 1)
                    stackBuf.Add currY

            if currY > 0 then
                if Arr2D.get s currX (currY - 1) = 0uy then
                    Arr2D.set s currX (currY - 1) 2uy
                    stackBuf.Add currX
                    stackBuf.Add (currY - 1)

            if currY < s.Height - 1 then
                if Arr2D.get s currX (currY + 1) = 0uy then
                    Arr2D.set s currX (currY + 1) 2uy
                    stackBuf.Add currX
                    stackBuf.Add (currY + 1)

    let print (s : Arr2D<byte>) =
        for y = 0 to s.Height - 1 do
            for x = 0 to s.Width - 1 do
                match Arr2D.get s x y with
                | 0uy -> printf " "
                | 1uy -> printf "#"
                | 2uy -> printf "."
                | s -> failwithf "unrecognised: %i" s

            printfn ""

        printfn ""
        printfn ""

    let inline setAt (arr : Arr2D<byte>) (x : int) (y : int) (matching : char) (target : byte) =
        Arr2D.set arr x y target

        match matching with
        | '-' ->
            Arr2D.set arr (x - 1) y target
            Arr2D.set arr (x + 1) y target
        | '|' ->
            Arr2D.set arr x (y - 1) target
            Arr2D.set arr x (y + 1) target
        | 'L' ->
            Arr2D.set arr x (y - 1) target
            Arr2D.set arr (x + 1) y target
        | 'J' ->
            Arr2D.set arr x (y - 1) target
            Arr2D.set arr (x - 1) y target
        | '7' ->
            Arr2D.set arr x (y + 1) target
            Arr2D.set arr (x - 1) y target
        | 'F' ->
            Arr2D.set arr x (y + 1) target
            Arr2D.set arr (x + 1) y target
        | c -> failwithf "bad char: %c" c

    let part2 (s : string) =
        let s = s.AsSpan ()
        let lineCount = s.Count '\n'
        let lineLength = (s.IndexOf '\n' + 1)
        let startPos = s.IndexOf 'S'

        let buffer = Array.zeroCreate (lineCount * lineLength * 9)
#if DEBUG
        let system : Arr2D<byte> =
            {
                Elements = buffer
                Width = 3 * lineLength
            }
#else
        use ptr = fixed buffer

        let system : Arr2D<byte> =
            {
                Elements = ptr
                Length = buffer.Length
                Width = 3 * lineLength
            }
#endif

        let struct (startLine, startCol) = toRowAndCol lineLength startPos
        let mutable prevPointA = startPos
        let mutable prevPointB = startPos

        Arr2D.set system (3 * startCol + 1) (3 * startLine + 1) 1uy

        let mutable pointA =
            let pos = ofRowAndCol lineLength startLine (startCol - 1)

            match if pos >= 0 then s.[pos] else 'n' with
            | '-'
            | 'L'
            | 'F' ->
                Arr2D.set system (3 * startCol) (3 * startLine + 1) 1uy
                pos
            | _ ->

            let pos = ofRowAndCol lineLength startLine (startCol + 1)

            match if pos < s.Length then s.[pos] else 'n' with
            | '-'
            | 'J'
            | '7' ->
                Arr2D.set system (3 * startCol + 2) (3 * startLine + 1) 1uy
                pos
            | _ ->

            Arr2D.set system (3 * startCol) (3 * startLine) 1uy
            Arr2D.set system (3 * startCol) (3 * startLine + 2) 1uy
            ofRowAndCol lineLength (startLine + 1) startCol

        let mutable pointB =
            let pos = ofRowAndCol lineLength (startLine - 1) startCol

            match if pos >= 0 then s.[pos] else 'n' with
            | '|'
            | '7'
            | 'F' ->
                Arr2D.set system (3 * startCol + 1) (3 * startLine) 1uy
                pos
            | _ ->

            let pos = ofRowAndCol lineLength (startLine + 1) startCol

            match if pos < s.Length then s.[pos] else 'n' with
            | '|'
            | 'L'
            | 'J' ->
                Arr2D.set system (3 * startCol + 1) (3 * startLine + 2) 1uy
                pos
            | _ ->

            let pos = ofRowAndCol lineLength startLine (startCol + 1)

            match if pos < s.Length then s.[pos] else 'n' with
            | '-'
            | 'J'
            | '7' ->
                Arr2D.set system (3 * startCol + 2) (3 * startLine + 1) 1uy
                pos
            | _ -> ofRowAndCol lineLength startLine (startCol - 1)

        do
            let struct (row, col) = toRowAndCol lineLength pointA
            setAt system (3 * col + 1) (3 * row + 1) s.[pointA] 1uy

        do
            let struct (row, col) = toRowAndCol lineLength pointB
            setAt system (3 * col + 1) (3 * row + 1) s.[pointB] 1uy

        while pointA <> pointB do
            let currentA = pointA
            pointA <- nextPoint s lineLength pointA prevPointA
            prevPointA <- currentA

            do
                let struct (row, col) = toRowAndCol lineLength pointA
                setAt system (3 * col + 1) (3 * row + 1) s.[pointA] 1uy

            let currentB = pointB
            pointB <- nextPoint s lineLength pointB prevPointB
            prevPointB <- currentB

            do
                let struct (row, col) = toRowAndCol lineLength pointB
                setAt system (3 * col + 1) (3 * row + 1) s.[pointB] 1uy

        let stackBuf = ResizeArray ()

        for line = 0 to system.Height - 1 do
            floodFill stackBuf system 0 line
            floodFill stackBuf system (system.Width - 1) line

        for col = 0 to system.Width - 1 do
            floodFill stackBuf system col 0
            floodFill stackBuf system col (system.Height - 1)

        let mutable answer = 0

        for row = 0 to lineCount - 1 do
            for col = 0 to lineLength - 1 do
                if Arr2D.get system (3 * col + 1) (3 * row + 1) = 0uy then
                    answer <- answer + 1

        answer
