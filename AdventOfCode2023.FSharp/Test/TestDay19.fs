namespace AdventOfCode2023.Test

open AdventOfCode2023
open NUnit.Framework
open FsUnitTyped
open System.IO

[<TestFixture>]
module TestDay19 =
    [<Test>]
    let sample = Assembly.getEmbeddedResource typeof<Dummy>.Assembly "day19.txt"

    [<Test>]
    let part1Sample () =
        use mutable s = StringSplitEnumerator.make '\n' sample
        let workflows = Day19.readWorkflows &s
        Day19.part1 workflows &s |> shouldEqual 19114

    [<Test>]
    let part2Sample () =
        use mutable s = StringSplitEnumerator.make '\n' sample
        let workflows = Day19.readWorkflows &s
        Day19.part2 workflows &s |> shouldEqual 167409079868000uL

    [<Test>]
    let part1Actual () =
        let s =
            try
                File.ReadAllText (Path.Combine (__SOURCE_DIRECTORY__, "../../inputs/day19.txt"))
            with
            | :? DirectoryNotFoundException
            | :? FileNotFoundException ->
                Assert.Inconclusive ()
                failwith "unreachable"

        use mutable s = StringSplitEnumerator.make '\n' s
        let workflows = Day19.readWorkflows &s

        Day19.part1 workflows &s |> shouldEqual 368964

    [<Test>]
    let part2Actual () =
        let s =
            try
                File.ReadAllText (Path.Combine (__SOURCE_DIRECTORY__, "../../inputs/day19.txt"))
            with
            | :? DirectoryNotFoundException
            | :? FileNotFoundException ->
                Assert.Inconclusive ()
                failwith "unreachable"

        use mutable s = StringSplitEnumerator.make '\n' s
        let workflows = Day19.readWorkflows &s

        Day19.part2 workflows &s |> shouldEqual 127675188176682uL
