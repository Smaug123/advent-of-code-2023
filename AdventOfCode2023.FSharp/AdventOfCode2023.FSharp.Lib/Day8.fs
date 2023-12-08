namespace AdventOfCode2023

open System
open System.Collections.Generic

[<RequireQualifiedAccess>]
module Day8 =

    type Instructions =
        {
            /// "true" is 'R'
            Steps : bool array

            Nodes : Dictionary<string, string * string>
        }

    let parse (s : string) =
        use mutable lines = StringSplitEnumerator.make '\n' s

        lines.MoveNext () |> ignore

        let stepsLine = lines.Current.TrimEnd ()
        let steps = Array.zeroCreate stepsLine.Length

        for i = 0 to stepsLine.Length - 1 do
            steps.[i] <- (stepsLine.[i] = 'R')

        let dict = Dictionary ()

        while lines.MoveNext () do
            if not lines.Current.IsEmpty then
                use mutable line = StringSplitEnumerator.make' ' ' lines.Current
                line.MoveNext () |> ignore
                let key = line.Current.ToString ()
                line.MoveNext () |> ignore
                line.MoveNext () |> ignore
                let v1 = line.Current.Slice(1, line.Current.Length - 2).ToString ()
                line.MoveNext () |> ignore
                let v2 = line.Current.Slice(0, line.Current.Length - 1).ToString ()
                dict.[key] <- (v1, v2)

        {
            Steps = steps
            Nodes = dict
        }

    let part1 (s : string) =
        let data = parse s
        let mutable i = 0
        let mutable currentNode = "AAA"
        let mutable answer = 0

        while currentNode <> "ZZZ" do
            let instruction = data.Nodes.[currentNode]

            if data.Steps.[i] then
                // "true" is R
                currentNode <- snd instruction
            else
                currentNode <- fst instruction

            i <- (i + 1) % data.Steps.Length
            answer <- answer + 1

        answer

    let inline lcm (periods : ^T[]) =
        let mutable lcm = periods.[0]
        let mutable i = 1

        while i < periods.Length do
            let euclid = Arithmetic.euclideanAlgorithm lcm periods.[i]
            lcm <- (lcm * periods.[i]) / euclid.Hcf
            i <- i + 1

        lcm

    let part2 (s : string) =
        let data = parse s

        let startingNodes = ResizeArray ()

        for key in data.Nodes.Keys do
            if key.[key.Length - 1] = 'A' then
                startingNodes.Add key

        let periods =
            Array.init
                startingNodes.Count
                (fun startNode ->
                    let mutable i = 0
                    let mutable currentNode = startingNodes.[startNode]
                    let mutable answer = 0ul

                    while currentNode.[currentNode.Length - 1] <> 'Z' do
                        let instruction = data.Nodes.[currentNode]

                        if data.Steps.[i] then
                            // "true" is R
                            currentNode <- snd instruction
                        else
                            currentNode <- fst instruction

                        i <- (i + 1) % data.Steps.Length
                        answer <- answer + 1ul

                    uint64 answer
                )

        lcm periods
