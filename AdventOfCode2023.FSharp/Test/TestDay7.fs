namespace AdventOfCode2023.Test

open AdventOfCode2023
open NUnit.Framework
open FsUnitTyped
open System.IO

[<TestFixture>]
module TestDay7 =

    let sample = Assembly.getEmbeddedResource typeof<Dummy>.Assembly "day7.txt"

    [<Test>]
    let part1Sample () =
        sample |> Day7.part1 |> shouldEqual 6440ul

    [<Test>]
    let part2Sample () =
        sample |> Day7.part2 |> shouldEqual 5905ul

    [<Test>]
    let part1Actual () =
        let s =
            try
                File.ReadAllText (Path.Combine (__SOURCE_DIRECTORY__, "../../inputs/day7.txt"))
            with
            | :? DirectoryNotFoundException
            | :? FileNotFoundException ->
                Assert.Inconclusive ()
                failwith "unreachable"

        Day7.part1 s |> shouldEqual 250058342ul

    [<Test>]
    let part2Actual () =
        let s =
            try
                File.ReadAllText (Path.Combine (__SOURCE_DIRECTORY__, "../../inputs/day7.txt"))
            with
            | :? DirectoryNotFoundException
            | :? FileNotFoundException ->
                Assert.Inconclusive ()
                failwith "unreachable"

        Day7.part2 s |> shouldEqual 250506580ul
