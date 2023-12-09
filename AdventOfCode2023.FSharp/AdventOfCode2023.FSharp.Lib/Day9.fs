namespace AdventOfCode2023

open System

[<RequireQualifiedAccess>]
module Day9 =

    let extrapolate (isStart : bool) (arr : ResizeArray<int64>) =
        let mutable answer = 0L
        let pos = if isStart then -1L else int64 arr.Count

        for i = 0 to arr.Count - 1 do
            let mutable product = Rational.ofInt arr.[i]

            for j = 0 to arr.Count - 1 do
                if j <> i then
                    product <- product * Rational.make (pos - int64 j) (int64 i - int64 j)

            answer <- answer + Rational.assertIntegral product

        answer

    let part1 (s : string) =
        use s = StringSplitEnumerator.make '\n' s
        let mutable answer = 0L
        let arr = ResizeArray ()

        for line in s do
            arr.Clear ()
            use line = StringSplitEnumerator.make' ' ' line

            for number in line do
                let number = Int64.Parse number
                arr.Add number

            answer <- answer + extrapolate false arr

        answer

    let part2 (s : string) =
        use s = StringSplitEnumerator.make '\n' s
        let mutable answer = 0L
        let arr = ResizeArray ()

        for line in s do
            arr.Clear ()
            use line = StringSplitEnumerator.make' ' ' line

            for number in line do
                let number = Int64.Parse number
                arr.Add number

            answer <- answer + extrapolate true arr

        answer
