namespace AdventOfCode2023

#if DEBUG
#else
#nowarn "9"
#endif

open System

[<RequireQualifiedAccess>]
module Run =
    let mutable shouldWrite = true

    let day1 (partTwo : bool) (input : string) =
        if not partTwo then
            let output = Day1.part1 input

            if shouldWrite then
                Console.WriteLine output
        else
            let output = Day1.part2 input

            if shouldWrite then
                Console.WriteLine output

    let day2 (partTwo : bool) (input : string) =
        if not partTwo then
            let output = Day2.part1 input

            if shouldWrite then
                Console.WriteLine output
        else
            let output = Day2.part2 input

            if shouldWrite then
                Console.WriteLine output

    let day3 (partTwo : bool) (input : string) =
        let resultArr, len, lineCount = Day3.parse (input.ToCharArray () |> Array.map byte)
#if DEBUG
        let contents =
            {
                Elements = Array.take len resultArr
                Width = len / lineCount
            }
#else
        use ptr = fixed resultArr

        let contents =
            {
                Elements = ptr
                Length = len
                Width = len / lineCount
            }
#endif
        if not partTwo then
            let output = Day3.part1 contents

            if shouldWrite then
                Console.WriteLine output
        else
            let output = Day3.part2 contents

            if shouldWrite then
                Console.WriteLine output

    let day4 (partTwo : bool) (input : string) =
        if not partTwo then
            let output = Day4.part1 input

            if shouldWrite then
                Console.WriteLine output
        else
            let output = Day4.part2 input

            if shouldWrite then
                Console.WriteLine output

    let day5 (partTwo : bool) (input : string) =
        if not partTwo then
            let output = Day5.part1 input

            if shouldWrite then
                Console.WriteLine output
        else
            let output = Day5.part2 input

            if shouldWrite then
                Console.WriteLine output

    let day6 (partTwo : bool) (input : string) =
        if not partTwo then
            let output = Day6.part1 input

            if shouldWrite then
                Console.WriteLine output
        else
            let output = Day6.part2 input

            if shouldWrite then
                Console.WriteLine output

    let day7 (partTwo : bool) (input : string) =
        if not partTwo then
            let output = Day7.part1 input

            if shouldWrite then
                Console.WriteLine output
        else
            let output = Day7.part2 input

            if shouldWrite then
                Console.WriteLine output

    let day8 (partTwo : bool) (input : string) =
        if not partTwo then
            let output = Day8.part1 input

            if shouldWrite then
                Console.WriteLine output
        else
            let output = Day8.part2 input

            if shouldWrite then
                Console.WriteLine output

    let day9 (partTwo : bool) (input : string) =
        if not partTwo then
            let output = Day9.part1 input

            if shouldWrite then
                Console.WriteLine output
        else
            let output = Day9.part2 input

            if shouldWrite then
                Console.WriteLine output

    let day10 (partTwo : bool) (input : string) =
        if not partTwo then
            let output = Day10.part1 input

            if shouldWrite then
                Console.WriteLine output
        else
            let output = Day10.part2 input

            if shouldWrite then
                Console.WriteLine output

    let day11 (partTwo : bool) (input : string) =
        if not partTwo then
            let output = Day11.part1 input

            if shouldWrite then
                Console.WriteLine output
        else
            let output = Day11.part2 input

            if shouldWrite then
                Console.WriteLine output


    let day12 (partTwo : bool) (input : string) =
        if not partTwo then
            let output = Day12.part1 input

            if shouldWrite then
                Console.WriteLine output
        else
            let output = Day12.part2 input

            if shouldWrite then
                Console.WriteLine output

    let day13 (partTwo : bool) (input : string) =
        if not partTwo then
            let output = Day13.part1 input

            if shouldWrite then
                Console.WriteLine output
        else
            let output = Day13.part2 input

            if shouldWrite then
                Console.WriteLine output

    let day14 (partTwo : bool) (input : string) =
        if not partTwo then
            let output = Day14.part1 input

            if shouldWrite then
                Console.WriteLine output
        else
            let output = Day14.part2 input

            if shouldWrite then
                Console.WriteLine output

    let day15 (partTwo : bool) (input : string) =
        if not partTwo then
            let output = Day15.part1 input

            if shouldWrite then
                Console.WriteLine output
        else
            let output = Day15.part2 input

            if shouldWrite then
                Console.WriteLine output

    let allRuns =
        [|
            day1
            day2
            day3
            day4
            day5
            day6
            day7
            day8
            day9
            day10
            day11
            day12
            day13
            day14
            day15
        |]
