namespace AdventOfCode2023.Test

open AdventOfCode2023
open NUnit.Framework
open FsUnitTyped
open System.IO

[<TestFixture>]
module TestDay8 =

    [<Test>]
    let part1Sample () =
        Assembly.getEmbeddedResource typeof<Dummy>.Assembly "day8part1.txt"
        |> Day8.part1
        |> shouldEqual 2

    [<Test>]
    let part1Sample2 () =
        """LLR

AAA = (BBB, BBB)
BBB = (AAA, ZZZ)
ZZZ = (ZZZ, ZZZ)"""
        |> Day8.part1
        |> shouldEqual 6

    [<Test>]
    let part2Sample () =
        Assembly.getEmbeddedResource typeof<Dummy>.Assembly "day8.txt"
        |> Day8.part2
        |> shouldEqual 6uL

    [<Test>]
    let part1Actual () =
        let s =
            try
                File.ReadAllText (Path.Combine (__SOURCE_DIRECTORY__, "../../inputs/day8.txt"))
            with
            | :? DirectoryNotFoundException
            | :? FileNotFoundException ->
                Assert.Inconclusive ()
                failwith "unreachable"

        Day8.part1 s |> shouldEqual 19199

    [<Test>]
    let part2Actual () =
        let s =
            try
                File.ReadAllText (Path.Combine (__SOURCE_DIRECTORY__, "../../inputs/day8.txt"))
            with
            | :? DirectoryNotFoundException
            | :? FileNotFoundException ->
                Assert.Inconclusive ()
                failwith "unreachable"

        Day8.part2 s |> shouldEqual 13663968099527uL
