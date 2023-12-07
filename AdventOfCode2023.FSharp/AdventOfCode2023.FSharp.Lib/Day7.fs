namespace AdventOfCode2023

open System
open System.Collections.Generic

type Hand =
    | Five = 10
    | Four = 9
    | FullHouse = 8
    | Three = 7
    | TwoPairs = 6
    | Pair = 5
    | High = 4

type HandContents =
    {
        First : byte
        Second : byte
        Third : byte
        Fourth : byte
        Fifth : byte
    }

[<RequireQualifiedAccess>]
module Day7 =

    let inline toByte (adjustJoker : bool) (c : char) : byte =
        if c <= '9' then byte c - byte '0'
        elif c = 'T' then 10uy
        elif c = 'J' then (if adjustJoker then 1uy else 11uy)
        elif c = 'Q' then 12uy
        elif c = 'K' then 13uy
        elif c = 'A' then 14uy
        else failwithf "could not parse: %c" c

    let inline private updateState (tallies : ResizeArray<_>) newNum =
        let mutable isAdded = false

        for i = 0 to tallies.Count - 1 do
            if fst tallies.[i] = newNum then
                tallies.[i] <- (fst tallies.[i], snd tallies.[i] + 1)
                isAdded <- true

        if not isAdded then
            tallies.Add (newNum, 1)

    let inline parseHand
        (tallyBuffer : ResizeArray<_>)
        (adjustJoker : bool)
        (s : ReadOnlySpan<char>)
        : Hand * HandContents
        =
        let contents =
            {
                First = toByte adjustJoker s.[0]
                Second = toByte adjustJoker s.[1]
                Third = toByte adjustJoker s.[2]
                Fourth = toByte adjustJoker s.[3]
                Fifth = toByte adjustJoker s.[4]
            }

        tallyBuffer.Clear ()
        tallyBuffer.Add (contents.First, 1)
        updateState tallyBuffer contents.Second
        updateState tallyBuffer contents.Third
        updateState tallyBuffer contents.Fourth
        updateState tallyBuffer contents.Fifth

        let jokerCount, jokerPos =
            if not adjustJoker then
                0, -1
            else
                let mutable count = 0
                let mutable jokerPos = -1

                for i = 0 to tallyBuffer.Count - 1 do
                    let card, tally = tallyBuffer.[i]

                    if card = 1uy then
                        count <- tally
                        jokerPos <- i

                count, jokerPos

        let hand =
            if jokerCount > 0 then
                match tallyBuffer.Count with
                | 1 ->
                    // Five jokers
                    Hand.Five
                | 2 ->
                    // Jokers plus one other card type
                    Hand.Five
                | 3 ->
                    // Jokers plus two other card types. Either full house, or four of a kind
                    if jokerCount >= 2 then
                        // JJABB or JJJAB
                        Hand.Four
                    else if
                        // JAABB or JAAAB
                        jokerPos <> 0
                    then
                        if snd tallyBuffer.[0] = 2 then
                            Hand.FullHouse
                        else
                            Hand.Four
                    else if snd tallyBuffer.[1] = 2 then
                        Hand.FullHouse
                    else
                        Hand.Four
                | 4 ->
                    // Jokers plus three other card types, exactly one of which therefore is a two-of.
                    Hand.Three
                | 5 ->
                    // Five different cards, one of which is a joker.
                    Hand.Pair
                | _ -> failwith "bad tallyBuffer"
            elif tallyBuffer.Count = 1 then
                Hand.Five
            elif tallyBuffer.Count = 2 then
                // AAAAB or AAABB
                if snd tallyBuffer.[0] = 3 || snd tallyBuffer.[0] = 2 then
                    Hand.FullHouse
                else
                    Hand.Four
            elif tallyBuffer.Count = 3 then
                // AAABC or AABBC
                if snd tallyBuffer.[0] = 3 then Hand.Three
                elif snd tallyBuffer.[0] = 2 then Hand.TwoPairs
                elif snd tallyBuffer.[1] = 3 then Hand.Three
                elif snd tallyBuffer.[1] = 2 then Hand.TwoPairs
                else Hand.Three
            elif tallyBuffer.Count = 4 then
                Hand.Pair
            else
                Hand.High

        hand, contents

    let parse (adjustJoker : bool) (s : string) : ResizeArray<Hand * HandContents * int> =
        use mutable lines = StringSplitEnumerator.make '\n' s
        let result = ResizeArray ()
        let tallies = ResizeArray 5

        while lines.MoveNext () do
            if not lines.Current.IsEmpty then
                use mutable line = StringSplitEnumerator.make' ' ' lines.Current
                line.MoveNext () |> ignore
                let hand, contents = parseHand tallies adjustJoker line.Current
                line.MoveNext () |> ignore
                let bid = Int32.Parse line.Current

                result.Add (hand, contents, bid)

        result

    let compArrBasic (a : HandContents) (b : HandContents) =
        if a.First > b.First then 1
        elif a.First < b.First then -1
        elif a.Second > b.Second then 1
        elif a.Second < b.Second then -1
        elif a.Third > b.Third then 1
        elif a.Third < b.Third then -1
        elif a.Fourth > b.Fourth then 1
        elif a.Fourth < b.Fourth then -1
        elif a.Fifth > b.Fifth then 1
        elif a.Fifth < b.Fifth then -1
        else 0

    let compBasic : IComparer<Hand * HandContents * int> =
        { new IComparer<_> with
            member _.Compare ((aHand, aContents, _), (bHand, bContents, _)) =
                match compare aHand bHand with
                | 0 -> compArrBasic aContents bContents
                | x -> x
        }

    let part1 (s : string) : int =
        let parsed = parse false s

        parsed.Sort compBasic

        let mutable answer = 0
        let mutable pos = 1

        for _, _, bid in parsed do
            answer <- answer + bid * pos
            pos <- pos + 1

        answer

    let part2 (s : string) =
        let parsed = parse true s

        parsed.Sort compBasic

        let mutable answer = 0
        let mutable pos = 1

        for _, _, bid in parsed do
            answer <- answer + bid * pos
            pos <- pos + 1

        answer
