namespace AdventOfCode2023.Test

open AdventOfCode2023
open NUnit.Framework
open FsUnitTyped
open System.IO

[<TestFixture>]
module TestDay2 =

    let sample = Assembly.getEmbeddedResource typeof<Dummy>.Assembly "day2.txt"

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
