namespace AdventOfCode2023.Test

open AdventOfCode2023
open NUnit.Framework
open FsUnitTyped
open System.IO

[<TestFixture>]
module TestDay10 =

    let part1Sample1 () =
        """.....
.S-7.
.|.|.
.L-J.
.....
"""
        |> Day10.part1
        |> shouldEqual 4


    [<Test>]
    let part1Sample () =
        Assembly.getEmbeddedResource typeof<Dummy>.Assembly "day10part1.txt"
        |> Day10.part1
        |> shouldEqual 8

    [<Test>]
    let part2Sample1 () =
        """...........
.S-------7.
.|F-----7|.
.||.....||.
.||.....||.
.|L-7.F-J|.
.|..|.|..|.
.L--J.L--J.
...........
"""
        |> Day10.part2
        |> shouldEqual 4

    [<Test>]
    let part2Sample2 () =
        """..........
.S------7.
.|F----7|.
.||....||.
.||....||.
.|L-7F-J|.
.|..||..|.
.L--JL--J.
..........
"""
        |> Day10.part2
        |> shouldEqual 4

    [<Test>]
    let part2Sample3 () =
        """.F----7F7F7F7F-7....
.|F--7||||||||FJ....
.||.FJ||||||||L7....
FJL7L7LJLJ||LJ.L-7..
L--J.L7...LJS7F-7L7.
....F-J..F7FJ|L7L7L7
....L7.F7||L7|.L7L7|
.....|FJLJ|FJ|F7|.LJ
....FJL-7.||.||||...
....L---J.LJ.LJLJ...
"""
        |> Day10.part2
        |> shouldEqual 8

    [<Test>]
    let part2Sample () =
        Assembly.getEmbeddedResource typeof<Dummy>.Assembly "day10.txt"
        |> Day10.part2
        |> shouldEqual 10

    [<Test>]
    let part1Actual () =
        let s =
            try
                File.ReadAllText (Path.Combine (__SOURCE_DIRECTORY__, "../../inputs/day10.txt"))
            with
            | :? DirectoryNotFoundException
            | :? FileNotFoundException ->
                Assert.Inconclusive ()
                failwith "unreachable"

        Day10.part1 s |> shouldEqual 6842

    [<Test>]
    let part2Actual () =
        let s =
            try
                File.ReadAllText (Path.Combine (__SOURCE_DIRECTORY__, "../../inputs/day10.txt"))
            with
            | :? DirectoryNotFoundException
            | :? FileNotFoundException ->
                Assert.Inconclusive ()
                failwith "unreachable"

        Day10.part2 s |> shouldEqual 393
