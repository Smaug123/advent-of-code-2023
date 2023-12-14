namespace AdventOfCode2023

#if DEBUG
#else
#nowarn "9"
#endif

open System

[<RequireQualifiedAccess>]
module Day14 =
    let slideNorth (arr : Arr2D<byte>) : unit =
        for col = 0 to arr.Width - 1 do
            let mutable targetPos = -1
            let mutable pos = 0

            while targetPos = -1 do
                if Arr2D.get arr col pos = 0uy then
                    targetPos <- pos

                pos <- pos + 1

            while pos < arr.Height do
                let current = Arr2D.get arr col pos

                if current = 2uy then
                    targetPos <- pos + 1
                    let mutable hasMoved = false

                    while pos < arr.Height && not hasMoved do
                        if Arr2D.get arr col pos = 0uy then
                            targetPos <- pos
                            hasMoved <- true

                        pos <- pos + 1
                elif current = 1uy then
                    Arr2D.set arr col targetPos 1uy
                    Arr2D.set arr col pos 0uy
                    targetPos <- targetPos + 1
                    pos <- pos + 1
                else // current = 0uy
                    pos <- pos + 1

    let slideSouth (arr : Arr2D<byte>) : unit =
        for col = 0 to arr.Width - 1 do
            let mutable targetPos = arr.Height
            let mutable pos = arr.Height - 1

            while targetPos = arr.Height do
                if Arr2D.get arr col pos = 0uy then
                    targetPos <- pos

                pos <- pos - 1

            while pos >= 0 do
                let current = Arr2D.get arr col pos

                if current = 2uy then
                    targetPos <- pos - 1
                    let mutable hasMoved = false

                    while pos >= 0 && not hasMoved do
                        if Arr2D.get arr col pos = 0uy then
                            targetPos <- pos
                            hasMoved <- true

                        pos <- pos - 1
                elif current = 1uy then
                    Arr2D.set arr col targetPos 1uy
                    Arr2D.set arr col pos 0uy
                    targetPos <- targetPos - 1
                    pos <- pos - 1
                else // current = 0uy
                    pos <- pos - 1

    let slideEast (arr : Arr2D<byte>) : unit =
        for row = 0 to arr.Height - 1 do
            let mutable targetPos = arr.Width
            let mutable pos = arr.Width - 1

            while targetPos = arr.Width do
                if Arr2D.get arr pos row = 0uy then
                    targetPos <- pos

                pos <- pos - 1

            while pos >= 0 do
                let current = Arr2D.get arr pos row

                if current = 2uy then
                    targetPos <- pos - 1
                    let mutable hasMoved = false

                    while pos >= 0 && not hasMoved do
                        if Arr2D.get arr pos row = 0uy then
                            targetPos <- pos
                            hasMoved <- true

                        pos <- pos - 1
                elif current = 1uy then
                    Arr2D.set arr targetPos row 1uy
                    Arr2D.set arr pos row 0uy
                    targetPos <- targetPos - 1
                    pos <- pos - 1
                else // current = 0uy
                    pos <- pos - 1

    let slideWest (arr : Arr2D<byte>) : unit =
        for row = 0 to arr.Height - 1 do
            let mutable targetPos = -1
            let mutable pos = 0

            while targetPos = -1 do
                if Arr2D.get arr pos row = 0uy then
                    targetPos <- pos

                pos <- pos + 1

            while pos < arr.Height do
                let current = Arr2D.get arr pos row

                if current = 2uy then
                    targetPos <- pos + 1
                    let mutable hasMoved = false

                    while pos < arr.Width && not hasMoved do
                        if Arr2D.get arr pos row = 0uy then
                            targetPos <- pos
                            hasMoved <- true

                        pos <- pos + 1
                elif current = 1uy then
                    Arr2D.set arr targetPos row 1uy
                    Arr2D.set arr pos row 0uy
                    targetPos <- targetPos + 1
                    pos <- pos + 1
                else // current = 0uy
                    pos <- pos + 1

    let print (board : Arr2D<byte>) =
        for row = 0 to board.Height - 1 do
            for col = 0 to board.Width - 1 do
                match Arr2D.get board col row with
                | 0uy -> printf "."
                | 1uy -> printf "O"
                | 2uy -> printf "#"
                | _ -> failwith "bad value"

            printfn ""

        printfn ""

    let score (board : Arr2D<byte>) =
        let mutable answer = 0ul

        for row = 0 to board.Height - 1 do
            for col = 0 to board.Width - 1 do
                if Arr2D.get board col row = 1uy then
                    answer <- answer + (board.Height - row |> uint32)

        answer

    let hash (board : Arr2D<byte>) =
        let mutable hash = 0uL
        let mutable pos = 0uL

        for x = 0 to board.Width - 1 do
            for y = 0 to board.Height - 1 do
                hash <- hash + pos * uint64 (Arr2D.get board x y)
                pos <- pos + 1uL

        hash

    let part1 (s : string) =
        let s = s.AsSpan ()
        let lineLength = s.IndexOf '\n'

        let buffer = Array.zeroCreate (lineLength * s.Length / (lineLength + 1))
        let mutable i = 0

        for c in s do
            match c with
            | '#' -> buffer.[i] <- 2uy
            | '.' -> buffer.[i] <- 0uy
            | 'O' -> buffer.[i] <- 1uy
            | '\n' -> i <- i - 1
            | _ -> failwith "bad char"

            i <- i + 1

#if DEBUG
        let system : Arr2D<byte> =
            {
                Elements = buffer
                Width = lineLength
            }
#else
        use ptr = fixed buffer

        let system : Arr2D<byte> =
            {
                Elements = ptr
                Length = buffer.Length
                Width = lineLength
            }
#endif

        slideNorth system

        score system

    let cycleOnce (arr : Arr2D<_>) =
        slideNorth arr
        slideWest arr
        slideSouth arr
        slideEast arr

    let part2 (s : string) =
        let s = s.AsSpan ()
        let lineLength = s.IndexOf '\n'

        let buffer = Array.zeroCreate (lineLength * s.Length / (lineLength + 1))
        let mutable i = 0

        for c in s do
            match c with
            | '#' -> buffer.[i] <- 2uy
            | '.' -> buffer.[i] <- 0uy
            | 'O' -> buffer.[i] <- 1uy
            | '\n' -> i <- i - 1
            | _ -> failwith "bad char"

            i <- i + 1

#if DEBUG
        let system : Arr2D<byte> =
            {
                Elements = buffer
                Width = lineLength
            }
#else
        use ptr = fixed buffer

        let system : Arr2D<byte> =
            {
                Elements = ptr
                Length = buffer.Length
                Width = lineLength
            }
#endif

        let mutable tortoise = 1
        let mutable hare = 2
        let scores = ResizeArray<_> ()
        scores.Add (score system, hash system)
        cycleOnce system
        scores.Add (score system, hash system)
        cycleOnce system
        scores.Add (score system, hash system)

        while scores.[hare] <> scores.[tortoise] do
            cycleOnce system
            scores.Add (score system, hash system)
            cycleOnce system
            scores.Add (score system, hash system)

            hare <- hare + 2
            tortoise <- tortoise + 1

        tortoise <- 0
        // mu-table heh heh
        let mutable firstRepetition = 0

        while scores.[hare] <> scores.[tortoise] do
            cycleOnce system
            scores.Add (score system, hash system)
            hare <- hare + 1
            tortoise <- tortoise + 1
            firstRepetition <- firstRepetition + 1

        let mutable cycleLength = 1
        hare <- tortoise + 1

        while scores.[tortoise] <> scores.[hare] do
            hare <- hare + 1
            cycleOnce system
            scores.Add (score system, hash system)
            cycleLength <- cycleLength + 1

        let cycles = (1_000_000_000uL - uint64 firstRepetition) % (uint64 cycleLength)

        fst scores.[firstRepetition + int cycles]
