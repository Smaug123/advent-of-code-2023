namespace AdventOfCode2023.Test

open AdventOfCode2023
open NUnit.Framework
open FsUnitTyped
open System.IO

[<TestFixture>]
module TestDay5 =

    let sample = Assembly.getEmbeddedResource typeof<Dummy>.Assembly "day5.txt"

    [<Test>]
    let part1Sample () =
        sample |> Day5.part1 |> shouldEqual 35ul

    [<Test>]
    let part2Sample () =
        sample |> Day5.part2 |> shouldEqual 46ul

    [<Test>]
    let part1Actual () =
        let s =
            try
                File.ReadAllText (Path.Combine (__SOURCE_DIRECTORY__, "../../inputs/day5.txt"))
            with
            | :? DirectoryNotFoundException
            | :? FileNotFoundException ->
                Assert.Inconclusive ()
                failwith "unreachable"

        Day5.part1 s |> shouldEqual 806029445ul

    [<Test>]
    let part2Actual () =
        let s =
            try
                File.ReadAllText (Path.Combine (__SOURCE_DIRECTORY__, "../../inputs/day5.txt"))
            with
            | :? DirectoryNotFoundException
            | :? FileNotFoundException ->
                Assert.Inconclusive ()
                failwith "unreachable"

        Day5.part2 s |> shouldEqual 59370572ul
