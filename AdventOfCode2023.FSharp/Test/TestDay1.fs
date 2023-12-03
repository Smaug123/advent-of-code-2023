namespace AdventOfCode2023.Test

open AdventOfCode2023
open NUnit.Framework
open FsUnitTyped
open System.IO

[<TestFixture>]
module TestDay1 =

    let sample1 = Assembly.getEmbeddedResource typeof<Dummy>.Assembly "day1part1.txt"

    [<Test>]
    let part1Sample () =
        sample1 |> Day1.part1 |> shouldEqual 142

    let sample2 = Assembly.getEmbeddedResource typeof<Dummy>.Assembly "day1.txt"

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
