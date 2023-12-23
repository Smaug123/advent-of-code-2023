namespace AdventOfCode2023

open System

[<RequireQualifiedAccess>]
module List =
    let rec nTuples (n : int) (xs : 'a list) : 'a list list =
#if DEBUG
        if n < 0 then
            raise (ArgumentException "n cannot be negative")
#endif
        match n, xs with
        | 0, _ -> [ [] ]
        | _, [] -> []
        | _, x :: xs ->
            let withX = nTuples (n - 1) xs |> List.map (fun xs -> x :: xs)
            let withoutX = nTuples n xs
            withX @ withoutX

    let inline inclusionExclusion (sizeOf : 'a -> 'ret) (sizeOfTuple : 'a list -> 'ret) (xs : 'a list) : 'ret =
        let mutable result = List.sumBy sizeOf xs
        let mutable i = 2

        while i <= xs.Length do
            let nTuples = nTuples i xs
            let sum = List.sumBy sizeOfTuple nTuples

            if sum = LanguagePrimitives.GenericZero then
                i <- Int32.MaxValue
            else
                if i % 2 = 0 then
                    result <- result - sum
                else
                    result <- result + sum

                i <- i + 1

        result
