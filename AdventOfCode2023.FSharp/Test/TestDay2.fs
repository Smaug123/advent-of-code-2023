namespace AdventOfCode2023.Test

open AdventOfCode2023
open NUnit.Framework
open FsUnitTyped
open System.IO

[<TestFixture>]
module TestDay2 =

    let sample =
        """Game 1: 3 blue, 4 red; 1 red, 2 green, 6 blue; 2 green
Game 2: 1 blue, 2 green; 3 green, 4 blue, 1 red; 1 green, 1 blue
Game 3: 8 green, 6 blue, 20 red; 5 blue, 4 red, 13 green; 5 green, 1 red
Game 4: 1 green, 3 red, 6 blue; 3 green, 6 red; 3 green, 15 blue, 14 red
Game 5: 6 red, 1 blue, 3 green; 2 blue, 1 red, 2 green
"""

    [<Test>]
    let part1Sample () = sample |> Day2.part1 |> shouldEqual 8

    [<Test>]
    let part2Sample () =
        sample |> Day2.part2 |> shouldEqual 2286

    [<Test>]
    let part1Actual () =
        let s =
            try
                File.ReadAllText (Path.Combine (__SOURCE_DIRECTORY__, "../../inputs/day2.txt"))
            with
            | :? DirectoryNotFoundException
            | :? FileNotFoundException ->
                Assert.Inconclusive ()
                failwith "unreachable"

        Day2.part1 s |> shouldEqual 2727

    [<Test>]
    let part2Actual () =
        let s =
            try
                File.ReadAllText (Path.Combine (__SOURCE_DIRECTORY__, "../../inputs/day2.txt"))
            with
            | :? DirectoryNotFoundException
            | :? FileNotFoundException ->
                Assert.Inconclusive ()
                failwith "unreachable"

        Day2.part2 s |> shouldEqual 56580
