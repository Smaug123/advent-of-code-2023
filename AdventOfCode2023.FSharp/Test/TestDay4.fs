namespace AdventOfCode2023.Test

open AdventOfCode2023
open NUnit.Framework
open FsUnitTyped
open System.IO

[<TestFixture>]
module TestDay4 =

    let sample = Assembly.getEmbeddedResource typeof<Dummy>.Assembly "day4.txt"

    [<Test>]
    let part1Sample () = sample |> Day4.part1 |> shouldEqual 13

    [<Test>]
    let part2Sample () = sample |> Day4.part2 |> shouldEqual 30

    [<Test>]
    let part1Actual () =
        let s =
            try
                File.ReadAllText (Path.Combine (__SOURCE_DIRECTORY__, "../../inputs/day4.txt"))
            with
            | :? DirectoryNotFoundException
            | :? FileNotFoundException ->
                Assert.Inconclusive ()
                failwith "unreachable"

        Day4.part1 s |> shouldEqual 27454

    [<Test>]
    let part2Actual () =
        let s =
            try
                File.ReadAllText (Path.Combine (__SOURCE_DIRECTORY__, "../../inputs/day4.txt"))
            with
            | :? DirectoryNotFoundException
            | :? FileNotFoundException ->
                Assert.Inconclusive ()
                failwith "unreachable"

        Day4.part2 s |> shouldEqual 6857330
