namespace AdventOfCode2023.Test

open AdventOfCode2023
open NUnit.Framework
open FsUnitTyped
open System.IO

[<TestFixture>]
module TestDay16 =

    [<Test>]
    let sample = Assembly.getEmbeddedResource typeof<Dummy>.Assembly "day16.txt"

    [<Test>]
    let part1Sample () = sample |> Day16.part1 |> shouldEqual 46

    [<Test>]
    let part2Sample () = sample |> Day16.part2 |> shouldEqual 51

    [<Test>]
    let part1Actual () =
        let s =
            try
                File.ReadAllText (Path.Combine (__SOURCE_DIRECTORY__, "../../inputs/day16.txt"))
            with
            | :? DirectoryNotFoundException
            | :? FileNotFoundException ->
                Assert.Inconclusive ()
                failwith "unreachable"

        Day16.part1 s |> shouldEqual 8112

    [<Test>]
    let part2Actual () =
        let s =
            try
                File.ReadAllText (Path.Combine (__SOURCE_DIRECTORY__, "../../inputs/day16.txt"))
            with
            | :? DirectoryNotFoundException
            | :? FileNotFoundException ->
                Assert.Inconclusive ()
                failwith "unreachable"

        Day16.part2 s |> shouldEqual 8314
