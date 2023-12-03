namespace AdventOfCode2023

open System

[<RequireQualifiedAccess>]
module Day2 =

    let part1 (s : string) =
        use lines = StringSplitEnumerator.make '\n' s
        let mutable answer = 0

        for line in lines do
            if not line.IsEmpty then
                use mutable words = StringSplitEnumerator.make' ' ' line
                let mutable prevWord = ReadOnlySpan<char>.Empty
                let mutable isOk = true

                while isOk && words.MoveNext () do
                    match words.Current.[0] with
                    | 'b' ->
                        if Int32.Parse prevWord > 14 then
                            isOk <- false
                    | 'r' ->
                        if Int32.Parse prevWord > 12 then
                            isOk <- false
                    | 'g' ->
                        if Int32.Parse prevWord > 13 then
                            isOk <- false
                    | _ -> ()

                    prevWord <- words.Current

                if isOk then
                    answer <- answer + Int32.Parse (line.Slice (5, line.IndexOf ':' - 5))

        answer

    let part2 (s : string) =
        use lines = StringSplitEnumerator.make '\n' s
        let mutable answer = 0

        for line in lines do
            if not line.IsEmpty then
                let mutable reds = 0
                let mutable blues = 0
                let mutable greens = 0
                use mutable words = StringSplitEnumerator.make' ' ' line
                let mutable prevWord = ReadOnlySpan<char>.Empty

                while words.MoveNext () do
                    match words.Current.[0] with
                    | 'b' -> blues <- max blues (Int32.Parse prevWord)
                    | 'r' -> reds <- max reds (Int32.Parse prevWord)
                    | 'g' -> greens <- max greens (Int32.Parse prevWord)
                    | _ -> ()

                    prevWord <- words.Current

                answer <- answer + (reds * greens * blues)

        answer
