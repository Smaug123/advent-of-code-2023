namespace AdventOfCode2023.Test

open AdventOfCode2023
open NUnit.Framework
open FsUnitTyped
open System.IO

[<TestFixture>]
module TestDay18 =

    [<Test>]
    let sample = Assembly.getEmbeddedResource typeof<Dummy>.Assembly "day18.txt"

    [<Test>]
    let part1Sample () = sample |> Day18.part1 |> shouldEqual 62

    [<Test>]
    let part2Sample () = sample |> Day18.part2 |> shouldEqual 0

    [<Test>]
    let part1Actual () =
        let s =
            try
                File.ReadAllText (Path.Combine (__SOURCE_DIRECTORY__, "../../inputs/day18.txt"))
            with
            | :? DirectoryNotFoundException
            | :? FileNotFoundException ->
                Assert.Inconclusive ()
                failwith "unreachable"

        Day18.part1 s |> shouldEqual 106459

    [<Test>]
    let part2Actual () =
        let s =
            try
                File.ReadAllText (Path.Combine (__SOURCE_DIRECTORY__, "../../inputs/day18.txt"))
            with
            | :? DirectoryNotFoundException
            | :? FileNotFoundException ->
                Assert.Inconclusive ()
                failwith "unreachable"

        Day18.part2 s |> shouldEqual 0
