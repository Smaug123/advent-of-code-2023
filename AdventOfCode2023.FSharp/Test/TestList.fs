namespace Test

open NUnit.Framework
open FsUnitTyped
open FsCheck
open AdventOfCode2023

[<TestFixture>]
module TestList =

    [<Test>]
    let ``n-tuples have the right length`` () =
        let property (n : int) (xs : char list) =
            let n = min (abs n) 6
            let xs = xs |> List.take (min 10 xs.Length)
            let tuples = List.nTuples n xs
            tuples |> List.forall (fun i -> i.Length = n)

        property 1 [ 'v' ] |> shouldEqual true
        Check.QuickThrowOnFailure property
