namespace AdventOfCode2023.Test

open AdventOfCode2023
open NUnit.Framework
open FsUnitTyped
open System.IO

[<TestFixture>]
module TestDay6 =

    let sample = Assembly.getEmbeddedResource typeof<Dummy>.Assembly "day6.txt"

    [<Test>]
    let part1Sample () =
        sample |> Day6.part1 |> shouldEqual 288uL

    [<Test>]
    let part2Sample () =
        sample |> Day6.part2 |> shouldEqual 71503uL

    [<Test>]
    let part1Actual () =
        let s =
            try
                File.ReadAllText (Path.Combine (__SOURCE_DIRECTORY__, "../../inputs/day6.txt"))
            with
            | :? DirectoryNotFoundException
            | :? FileNotFoundException ->
                Assert.Inconclusive ()
                failwith "unreachable"

        Day6.part1 s |> shouldEqual 32076uL

    [<Test>]
    let part2Actual () =
        let s =
            try
                File.ReadAllText (Path.Combine (__SOURCE_DIRECTORY__, "../../inputs/day6.txt"))
            with
            | :? DirectoryNotFoundException
            | :? FileNotFoundException ->
                Assert.Inconclusive ()
                failwith "unreachable"

        Day6.part2 s |> shouldEqual 34278221uL
