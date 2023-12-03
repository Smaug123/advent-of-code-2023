namespace AdventOfCode2023.Test

open AdventOfCode2023
open NUnit.Framework
open FsUnitTyped
open System.IO

[<TestFixture>]
module TestDay1 =

    let sample1 =
        """1abc2
pqr3stu8vwx
a1b2c3d4e5f
treb7uchet
"""

    [<Test>]
    let part1Sample () =
        sample1 |> Day1.part1 |> shouldEqual 142

    let sample2 =
        """two1nine
eightwothree
abcone2threexyz
xtwone3four
4nineeightseven2
zoneight234
7pqrstsixteen
"""

    [<Test>]
    let part2Sample () =
        sample2 |> Day1.part2 |> shouldEqual 281

    [<Test>]
    let part1Actual () =
        let s =
            try
                File.ReadAllText (Path.Combine (__SOURCE_DIRECTORY__, "../../inputs/day1.txt"))
            with
            | :? DirectoryNotFoundException
            | :? FileNotFoundException ->
                Assert.Inconclusive ()
                failwith "unreachable"

        Day1.part1 s |> shouldEqual 54304

    [<Test>]
    let part2Actual () =
        let s =
            try
                File.ReadAllText (Path.Combine (__SOURCE_DIRECTORY__, "../../inputs/day1.txt"))
            with
            | :? DirectoryNotFoundException
            | :? FileNotFoundException ->
                Assert.Inconclusive ()
                failwith "unreachable"

        Day1.part2 s |> shouldEqual 54418
