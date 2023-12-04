namespace AdventOfCode2023

open System
open System.Collections.Generic

[<RequireQualifiedAccess>]
module Day4 =

    let part1 (s : string) =
        use lines = StringSplitEnumerator.make '\n' s
        let mutable total = 0
        let winningNumbers = HashSet ()

        for line in lines do
            if not (line.IsWhiteSpace ()) then
                let mutable accumulatingWinning = true
                winningNumbers.Clear ()
                use mutable split = StringSplitEnumerator.make' ' ' line
                StringSplitEnumerator.chomp "Card" &split

                while split.Current.IsEmpty || split.Current.[split.Current.Length - 1] <> ':' do
                    split.MoveNext () |> ignore

                split.MoveNext () |> ignore

                while accumulatingWinning do
                    while split.Current.IsEmpty do
                        split.MoveNext () |> ignore

                    if split.Current.[0] = '|' then
                        accumulatingWinning <- false
                    else
                        winningNumbers.Add (Int32.Parse split.Current) |> ignore
                        split.MoveNext () |> ignore

                let mutable answer = 0

                while split.MoveNext () do
                    if not split.Current.IsEmpty then
                        let n = Int32.Parse split.Current

                        if winningNumbers.Contains n then
                            answer <- answer + 1

                if answer > 0 then
                    total <- total + (1 <<< (answer - 1))

        total



    let part2 (s : string) =
        use lines = StringSplitEnumerator.make '\n' s
        let mutable total = 0
        let winningNumbers = HashSet ()
        let winners = ResizeArray ()

        for line in lines do
            if not (line.IsWhiteSpace ()) then
                let mutable accumulatingWinning = true
                winningNumbers.Clear ()
                use mutable split = StringSplitEnumerator.make' ' ' line
                StringSplitEnumerator.chomp "Card" &split

                while split.Current.IsEmpty || split.Current.[split.Current.Length - 1] <> ':' do
                    split.MoveNext () |> ignore

                split.MoveNext () |> ignore

                while accumulatingWinning do
                    while split.Current.IsEmpty do
                        split.MoveNext () |> ignore

                    if split.Current.[0] = '|' then
                        accumulatingWinning <- false
                    else
                        winningNumbers.Add (Int32.Parse split.Current) |> ignore
                        split.MoveNext () |> ignore

                let mutable answer = 0

                while split.MoveNext () do
                    if not split.Current.IsEmpty then
                        let n = Int32.Parse split.Current

                        if winningNumbers.Contains n then
                            answer <- answer + 1

                winners.Add answer

        let ans = Array.create winners.Count 1

        for i = 0 to winners.Count - 1 do
            for j = i + 1 to winners.[i] + i do
                ans.[j] <- ans.[j] + ans.[i]

        ans |> Array.sum
