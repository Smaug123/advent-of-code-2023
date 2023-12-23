namespace AdventOfCode2023.Test

open System.Threading
open AdventOfCode2023
open NUnit.Framework
open FsUnitTyped
open FsCheck

[<TestFixture>]
module TestIntervalSet =

    /// Normalises e.g. (5, 3) to (3, 5) too.
    let toIntervalSet (model : (int * int) list) =
        (IntervalSet.empty, model)
        ||> List.fold (fun intervals (x1, x2) -> IntervalSet.add (min x1 x2) (max x1 x2) intervals)

    let modelContains (x : int) (model : (int * int) list) =
        model
        |> List.exists (fun (x1, x2) ->
            let x1, x2 = min x1 x2, max x1 x2
            x1 <= x && x <= x2
        )

    [<Test>]
    let ``IntervalSet add works`` () =
        let property (pos : int ref) (neg : int ref) (x : int) (xs : (int * int) list) =
            let intervals = toIntervalSet xs

            let actual = IntervalSet.contains x intervals
            let expected = modelContains x xs

            if actual then
                Interlocked.Increment pos |> ignore
            else
                Interlocked.Increment neg |> ignore

            expected = actual

        let pos = ref 0
        let neg = ref 0

        Check.One (
            { Config.Default with
                MaxTest = 1000
            },
            property pos neg
        )

        printfn "Fraction of positive cases: %f" ((float pos.Value) / (float pos.Value + float neg.Value))

    [<Test>]
    let ``Intersection works`` () =
        let property
            (pos : int ref)
            (neg : int ref)
            (trials : int list)
            (xsModel : (int * int) list)
            (ysModel : (int * int) list)
            =
            let xs = toIntervalSet xsModel
            let ys = toIntervalSet ysModel

            let intervals = IntervalSet.intersection xs ys

            for x in trials do
                let actual = IntervalSet.contains x intervals
                let expected = modelContains x xsModel && modelContains x ysModel

                expected |> shouldEqual actual

                if actual then
                    Interlocked.Increment pos |> ignore
                else
                    Interlocked.Increment neg |> ignore

        let pos = ref 0
        let neg = ref 0

        Check.One (
            { Config.Default with
                MaxTest = 1000
            },
            property pos neg
        )

        printfn "Fraction of positive cases: %f" ((float pos.Value) / (float pos.Value + float neg.Value))
