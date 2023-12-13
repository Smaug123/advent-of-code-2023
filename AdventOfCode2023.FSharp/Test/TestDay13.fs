namespace AdventOfCode2023.Test

open System

open AdventOfCode2023
open NUnit.Framework
open FsUnitTyped
open System.IO

[<TestFixture>]
module TestDay13 =

    [<Test>]
    let ``rowToInt test`` () =
        Day13.rowToInt ("#.##..##.".AsSpan ()) |> shouldEqual 358ul

    [<Test>]
    let ``colToInt test`` () =
        let s =
            """#.##..##.
..#.##.#.
##......#
##......#
..#.##.#.
..##..##.
#.#.##.#.
"""

        Day13.colToInt (s.AsSpan ()) 9 0
        |> shouldEqual (List.sum [ 1 ; 8 ; 16 ; 64 ] |> uint32)

    [<Test>]

    let sample = Assembly.getEmbeddedResource typeof<Dummy>.Assembly "day13.txt"

    [<Test>]
    let part1Sample () =
        sample |> Day13.part1 |> shouldEqual 405ul

    [<Test>]
    let part2Sample () =
        sample |> Day13.part2 |> shouldEqual 400ul

    [<Test>]
    let part1Actual () =
        let s =
            try
                File.ReadAllText (Path.Combine (__SOURCE_DIRECTORY__, "../../inputs/day13.txt"))
            with
            | :? DirectoryNotFoundException
            | :? FileNotFoundException ->
                Assert.Inconclusive ()
                failwith "unreachable"

        Day13.part1 s |> shouldEqual 30158ul

    [<Test>]
    let part2Actual () =
        let s =
            try
                File.ReadAllText (Path.Combine (__SOURCE_DIRECTORY__, "../../inputs/day13.txt"))
            with
            | :? DirectoryNotFoundException
            | :? FileNotFoundException ->
                Assert.Inconclusive ()
                failwith "unreachable"

        Day13.part2 s |> shouldEqual 36474ul
