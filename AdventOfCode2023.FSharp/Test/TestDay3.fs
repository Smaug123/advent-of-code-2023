namespace AdventOfCode2023.Test

#if DEBUG
#else
#nowarn "9"
#endif

open AdventOfCode2023
open NUnit.Framework
open FsUnitTyped
open System.IO

[<TestFixture>]
module TestDay3 =

    let sample =
        """467..114..
...*......
..35..633.
......#...
617*......
.....+.58.
..592.....
......755.
...$.*....
.664.598..
"""

    [<Test>]
    let part1Sample () =
        let arr, len, rows = sample.ToCharArray () |> Array.map byte |> Day3.parse

#if DEBUG
        let arr =
            {
                Elements = arr
                Width = len / rows
            }
#else
        use arr = fixed arr

        let arr =
            {
                Elements = arr
                Length = len
                Width = len / rows
            }
#endif

        arr |> Day3.part1 |> shouldEqual 4361

    [<Test>]
    let part2Sample () =
        let arr, len, rows = sample.ToCharArray () |> Array.map byte |> Day3.parse

#if DEBUG
        let arr =
            {
                Elements = arr
                Width = len / rows
            }
#else
        use arr = fixed arr

        let arr =
            {
                Elements = arr
                Length = len
                Width = len / rows
            }
#endif

        arr |> Day3.part2 |> shouldEqual 467835

    [<Test>]
    let part1Actual () =
        let bytes =
            try
                File.ReadAllBytes (Path.Combine (__SOURCE_DIRECTORY__, "../../inputs/day3.txt"))
            with
            | :? DirectoryNotFoundException
            | :? FileNotFoundException ->
                Assert.Inconclusive ()
                failwith "unreachable"

        let arr, len, rows = Day3.parse bytes

#if DEBUG
        let arr =
            {
                Elements = arr
                Width = len / rows
            }
#else
        use arr = fixed arr

        let arr =
            {
                Elements = arr
                Length = len
                Width = len / rows
            }
#endif

        Day3.part1 arr |> shouldEqual 540131

    [<Test>]
    let part2Actual () =
        let bytes =
            try
                File.ReadAllBytes (Path.Combine (__SOURCE_DIRECTORY__, "../../inputs/day3.txt"))
            with
            | :? DirectoryNotFoundException
            | :? FileNotFoundException ->
                Assert.Inconclusive ()
                failwith "unreachable"

        let arr, len, rows = Day3.parse bytes

#if DEBUG
        let arr =
            {
                Elements = arr
                Width = len / rows
            }
#else
        use arr = fixed arr

        let arr =
            {
                Elements = arr
                Length = len
                Width = len / rows
            }
#endif

        Day3.part2 arr |> shouldEqual 86879020
