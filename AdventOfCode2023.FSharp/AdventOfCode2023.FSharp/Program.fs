namespace AdventOfCode2023

#if DEBUG
#else
#nowarn "9"
#endif

open System
open System.Diagnostics
open System.IO

module Program =

    let inline toUs (ticks : int64) =
        1_000_000.0 * float ticks / float Stopwatch.Frequency

    [<EntryPoint>]
    let main argv =
        let endToEnd = Stopwatch.StartNew ()
        endToEnd.Restart ()

        let dir = DirectoryInfo argv.[0]

        let sw = Stopwatch.StartNew ()

        Console.WriteLine "=====Day 1====="

        do
            sw.Restart ()

            let input =
                try
                    Path.Combine (dir.FullName, "day1part1.txt") |> File.ReadAllText
                with :? FileNotFoundException ->
                    Path.Combine (dir.FullName, "day1.txt") |> File.ReadAllText

            let part1 = Day1.part1 input
            sw.Stop ()
            Console.WriteLine (part1.ToString ())
            Console.Error.WriteLine ((1_000.0 * float sw.ElapsedTicks / float Stopwatch.Frequency).ToString () + "ms")
            sw.Restart ()
            let input = Path.Combine (dir.FullName, "day1.txt") |> File.ReadAllText
            let part2 = Day1.part2 input
            sw.Stop ()
            Console.WriteLine (part2.ToString ())
            Console.Error.WriteLine ((1_000.0 * float sw.ElapsedTicks / float Stopwatch.Frequency).ToString () + "ms")

        Console.WriteLine "=====Day 2====="

        do
            let input = Path.Combine (dir.FullName, "day2.txt") |> File.ReadAllText
            sw.Restart ()
            let part1 = Day2.part1 input
            sw.Stop ()
            Console.WriteLine (part1.ToString ())
            Console.Error.WriteLine ((1_000.0 * float sw.ElapsedTicks / float Stopwatch.Frequency).ToString () + "ms")
            sw.Restart ()
            let part2 = Day2.part2 input
            sw.Stop ()
            Console.WriteLine (part2.ToString ())
            Console.Error.WriteLine ((1_000.0 * float sw.ElapsedTicks / float Stopwatch.Frequency).ToString () + "ms")

        Console.WriteLine "=====Day 3====="

        do
            let input = Path.Combine (dir.FullName, "day3.txt") |> File.ReadAllBytes

            sw.Restart ()
            let resultArr, len, lineCount = Day3.parse input
            sw.Stop ()

            Console.Error.WriteLine (
                (1_000.0 * float sw.ElapsedTicks / float Stopwatch.Frequency).ToString ()
                + "ms parse"
            )
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
            let part1 = Day3.part1 contents
            sw.Stop ()
            Console.WriteLine (part1.ToString ())
            Console.Error.WriteLine ((1_000.0 * float sw.ElapsedTicks / float Stopwatch.Frequency).ToString () + "ms")
            sw.Restart ()
            let part2 = Day3.part2 contents
            Console.WriteLine (part2.ToString ())
            Console.Error.WriteLine ((1_000.0 * float sw.ElapsedTicks / float Stopwatch.Frequency).ToString () + "ms")

        Console.WriteLine "=====Day 4====="

        do
            let input = Path.Combine (dir.FullName, "day4.txt") |> File.ReadAllText
            sw.Restart ()
            let part1 = Day4.part1 input
            sw.Stop ()
            Console.WriteLine (part1.ToString ())
            Console.Error.WriteLine ((1_000.0 * float sw.ElapsedTicks / float Stopwatch.Frequency).ToString () + "ms")
            sw.Restart ()
            let part2 = Day4.part2 input
            sw.Stop ()
            Console.WriteLine (part2.ToString ())
            Console.Error.WriteLine ((1_000.0 * float sw.ElapsedTicks / float Stopwatch.Frequency).ToString () + "ms")

        Console.WriteLine "=====Day 5====="

        do
            let input = Path.Combine (dir.FullName, "day5.txt") |> File.ReadAllText
            sw.Restart ()
            let part1 = Day5.part1 input
            sw.Stop ()
            Console.WriteLine (part1.ToString ())
            Console.Error.WriteLine ((1_000.0 * float sw.ElapsedTicks / float Stopwatch.Frequency).ToString () + "ms")
            sw.Restart ()
            let part2 = Day5.part2 input
            sw.Stop ()
            Console.WriteLine (part2.ToString ())
            Console.Error.WriteLine ((1_000.0 * float sw.ElapsedTicks / float Stopwatch.Frequency).ToString () + "ms")

        Console.WriteLine "=====Day 6====="

        do
            let input = Path.Combine (dir.FullName, "day6.txt") |> File.ReadAllText
            sw.Restart ()
            let part1 = Day6.part1 input
            sw.Stop ()
            Console.WriteLine (part1.ToString ())
            Console.Error.WriteLine ((1_000.0 * float sw.ElapsedTicks / float Stopwatch.Frequency).ToString () + "ms")
            sw.Restart ()
            let part2 = Day6.part2 input
            sw.Stop ()
            Console.WriteLine (part2.ToString ())
            Console.Error.WriteLine ((1_000.0 * float sw.ElapsedTicks / float Stopwatch.Frequency).ToString () + "ms")

        Console.WriteLine "=====Day 7====="

        do
            let input = Path.Combine (dir.FullName, "day7.txt") |> File.ReadAllText
            sw.Restart ()
            let part1 = Day7.part1 input
            sw.Stop ()
            Console.WriteLine (part1.ToString ())
            Console.Error.WriteLine ((1_000.0 * float sw.ElapsedTicks / float Stopwatch.Frequency).ToString () + "ms")
            sw.Restart ()
            let part2 = Day7.part2 input
            sw.Stop ()
            Console.WriteLine (part2.ToString ())
            Console.Error.WriteLine ((1_000.0 * float sw.ElapsedTicks / float Stopwatch.Frequency).ToString () + "ms")

        Console.WriteLine "=====Day 8====="

        do
            let input =
                try
                    Path.Combine (dir.FullName, "day8part1.txt") |> File.ReadAllText
                with :? FileNotFoundException ->
                    Path.Combine (dir.FullName, "day8.txt") |> File.ReadAllText

            sw.Restart ()
            let part1 = Day8.part1 input
            sw.Stop ()
            Console.WriteLine (part1.ToString ())
            Console.Error.WriteLine ((1_000.0 * float sw.ElapsedTicks / float Stopwatch.Frequency).ToString () + "ms")
            sw.Restart ()
            let input = Path.Combine (dir.FullName, "day8.txt") |> File.ReadAllText
            let part2 = Day8.part2 input
            sw.Stop ()
            Console.WriteLine (part2.ToString ())
            Console.Error.WriteLine ((1_000.0 * float sw.ElapsedTicks / float Stopwatch.Frequency).ToString () + "ms")

        Console.WriteLine "=====Day 9====="

        do
            let input = Path.Combine (dir.FullName, "day9.txt") |> File.ReadAllText

            sw.Restart ()
            let part1 = Day9.part1 input
            sw.Stop ()
            Console.WriteLine (part1.ToString ())
            Console.Error.WriteLine ((1_000.0 * float sw.ElapsedTicks / float Stopwatch.Frequency).ToString () + "ms")
            sw.Restart ()
            let part2 = Day9.part2 input
            sw.Stop ()
            Console.WriteLine (part2.ToString ())
            Console.Error.WriteLine ((1_000.0 * float sw.ElapsedTicks / float Stopwatch.Frequency).ToString () + "ms")

        Console.WriteLine "=====Day 10====="

        do
            let input = Path.Combine (dir.FullName, "day10.txt") |> File.ReadAllText

            sw.Restart ()
            let part1 = Day10.part1 input
            sw.Stop ()
            Console.WriteLine (part1.ToString ())
            Console.Error.WriteLine ((1_000.0 * float sw.ElapsedTicks / float Stopwatch.Frequency).ToString () + "ms")
            sw.Restart ()
            let part2 = Day10.part2 input
            sw.Stop ()
            Console.WriteLine (part2.ToString ())
            Console.Error.WriteLine ((1_000.0 * float sw.ElapsedTicks / float Stopwatch.Frequency).ToString () + "ms")

        endToEnd.Stop ()

        Console.Error.WriteLine (
            (1_000.0 * float endToEnd.ElapsedTicks / float Stopwatch.Frequency).ToString ()
            + "ms total"
        )

        0
