namespace AdventOfCode2023.Test

open AdventOfCode2023
open NUnit.Framework
open FsUnitTyped
open System.IO

[<TestFixture>]
module TestDay15 =

    [<Test>]
    let sample = Assembly.getEmbeddedResource typeof<Dummy>.Assembly "day15.txt"

    [<Test>]
    let part1Sample () =
        sample |> Day15.part1 |> shouldEqual 1320

    [<Test>]
    let part2Sample () =
        sample |> Day15.part2 |> shouldEqual 145ul

    [<Test>]
    let part1Actual () =
        let s =
            try
                File.ReadAllText (Path.Combine (__SOURCE_DIRECTORY__, "../../inputs/day15.txt"))
            with
            | :? DirectoryNotFoundException
            | :? FileNotFoundException ->
                Assert.Inconclusive ()
                failwith "unreachable"

        Day15.part1 s |> shouldEqual 521434

    [<Test>]
    let part2Actual () =
        let s =
            try
                File.ReadAllText (Path.Combine (__SOURCE_DIRECTORY__, "../../inputs/day15.txt"))
            with
            | :? DirectoryNotFoundException
            | :? FileNotFoundException ->
                Assert.Inconclusive ()
                failwith "unreachable"

        Day15.part2 s |> shouldEqual 248279ul
