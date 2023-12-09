namespace AdventOfCode2023.Test

open AdventOfCode2023
open NUnit.Framework
open FsUnitTyped
open System.IO

[<TestFixture>]
module TestDay9 =

    let sample = Assembly.getEmbeddedResource typeof<Dummy>.Assembly "day9.txt"

    [<Test>]
    let part1Sample () =
        sample |> Day9.part1 |> shouldEqual 114L

    [<Test>]
    let part2Sample () =
        Assembly.getEmbeddedResource typeof<Dummy>.Assembly "day9.txt"
        |> Day9.part2
        |> shouldEqual 2L

    [<Test>]
    let part1Actual () =
        let s =
            try
                File.ReadAllText (Path.Combine (__SOURCE_DIRECTORY__, "../../inputs/day9.txt"))
            with
            | :? DirectoryNotFoundException
            | :? FileNotFoundException ->
                Assert.Inconclusive ()
                failwith "unreachable"

        Day9.part1 s |> shouldEqual 1898776583L

    [<Test>]
    let part2Actual () =
        let s =
            try
                File.ReadAllText (Path.Combine (__SOURCE_DIRECTORY__, "../../inputs/day9.txt"))
            with
            | :? DirectoryNotFoundException
            | :? FileNotFoundException ->
                Assert.Inconclusive ()
                failwith "unreachable"

        Day9.part2 s |> shouldEqual 1100L
