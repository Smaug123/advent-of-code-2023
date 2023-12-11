namespace AdventOfCode2023.Test

open AdventOfCode2023
open NUnit.Framework
open FsUnitTyped
open System.IO

[<TestFixture>]
module TestDay11 =

    let sample = Assembly.getEmbeddedResource typeof<Dummy>.Assembly "day11.txt"

    [<Test>]
    let part1Sample () =
        sample |> Day11.part1 |> shouldEqual 374uL

    [<Test>]
    let part2Sample () =
        let data = sample |> Day11.parse
        Day11.solve data 10uL |> shouldEqual 1030uL
        Day11.solve data 100uL |> shouldEqual 8410uL

    [<Test>]
    let part1Actual () =
        let s =
            try
                File.ReadAllText (Path.Combine (__SOURCE_DIRECTORY__, "../../inputs/day11.txt"))
            with
            | :? DirectoryNotFoundException
            | :? FileNotFoundException ->
                Assert.Inconclusive ()
                failwith "unreachable"

        Day11.part1 s |> shouldEqual 9947476uL

    [<Test>]
    let part2Actual () =
        let s =
            try
                File.ReadAllText (Path.Combine (__SOURCE_DIRECTORY__, "../../inputs/day11.txt"))
            with
            | :? DirectoryNotFoundException
            | :? FileNotFoundException ->
                Assert.Inconclusive ()
                failwith "unreachable"

        Day11.part2 s |> shouldEqual 519939907614uL
