namespace AdventOfCode2023.Test

open System

open AdventOfCode2023
open NUnit.Framework
open FsUnitTyped
open System.IO

[<TestFixture>]
module TestDay14 =

    [<Test>]
    let sample = Assembly.getEmbeddedResource typeof<Dummy>.Assembly "day14.txt"

    [<Test>]
    let part1Sample () =
        sample |> Day14.part1 |> shouldEqual 136ul

    [<Test>]
    let part2Sample () =
        sample |> Day14.part2 |> shouldEqual 64ul

    [<Test>]
    let part1Actual () =
        let s =
            try
                File.ReadAllText (Path.Combine (__SOURCE_DIRECTORY__, "../../inputs/day14.txt"))
            with
            | :? DirectoryNotFoundException
            | :? FileNotFoundException ->
                Assert.Inconclusive ()
                failwith "unreachable"

        Day14.part1 s |> shouldEqual 111339ul

    [<Test>]
    let part2Actual () =
        let s =
            try
                File.ReadAllText (Path.Combine (__SOURCE_DIRECTORY__, "../../inputs/day14.txt"))
            with
            | :? DirectoryNotFoundException
            | :? FileNotFoundException ->
                Assert.Inconclusive ()
                failwith "unreachable"

        Day14.part2 s |> shouldEqual 93736ul
