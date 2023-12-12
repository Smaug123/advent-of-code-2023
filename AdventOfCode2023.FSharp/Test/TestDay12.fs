namespace AdventOfCode2023.Test

open AdventOfCode2023
open NUnit.Framework
open FsUnitTyped
open System.IO

[<TestFixture>]
module TestDay12 =

    let sample = Assembly.getEmbeddedResource typeof<Dummy>.Assembly "day12.txt"

    [<Test>]
    let part1Sample () =
        sample |> Day12.part1 |> shouldEqual 21uL

    [<Test>]
    let part2Sample () =
        sample |> Day12.part2 |> shouldEqual 525152uL

    [<Test>]
    let part1Actual () =
        let s =
            try
                File.ReadAllText (Path.Combine (__SOURCE_DIRECTORY__, "../../inputs/day12.txt"))
            with
            | :? DirectoryNotFoundException
            | :? FileNotFoundException ->
                Assert.Inconclusive ()
                failwith "unreachable"

        Day12.part1 s |> shouldEqual 7402uL

    [<Test>]
    let part2Actual () =
        let s =
            try
                File.ReadAllText (Path.Combine (__SOURCE_DIRECTORY__, "../../inputs/day12.txt"))
            with
            | :? DirectoryNotFoundException
            | :? FileNotFoundException ->
                Assert.Inconclusive ()
                failwith "unreachable"

        Day12.part2 s |> shouldEqual 3384337640277uL
