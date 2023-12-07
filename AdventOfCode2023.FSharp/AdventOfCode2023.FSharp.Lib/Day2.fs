namespace AdventOfCode2023

open System
open System.Globalization

[<RequireQualifiedAccess>]
module Day2 =
    let inline parseInt (s : ReadOnlySpan<char>) : int =
        Int32.Parse (s, NumberStyles.None, CultureInfo.InvariantCulture)

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
                        if parseInt prevWord > 14 then
                            isOk <- false
                    | 'r' ->
                        if parseInt prevWord > 12 then
                            isOk <- false
                    | 'g' ->
                        if parseInt prevWord > 13 then
                            isOk <- false
                    | _ -> ()

                    prevWord <- words.Current

                if isOk then
                    answer <- answer + parseInt (line.Slice (5, line.IndexOf ':' - 5))

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
                    | 'b' -> blues <- max blues (parseInt prevWord)
                    | 'r' -> reds <- max reds (parseInt prevWord)
                    | 'g' -> greens <- max greens (parseInt prevWord)
                    | _ -> ()

                    prevWord <- words.Current

                answer <- answer + (reds * greens * blues)

        answer
