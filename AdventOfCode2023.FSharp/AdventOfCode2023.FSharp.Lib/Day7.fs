namespace AdventOfCode2023

open System
open System.Globalization
open AdventOfCode2023.ResizeArray

type Hand =
    | Five = 6
    | Four = 5
    | FullHouse = 4
    | Three = 3
    | TwoPairs = 2
    | Pair = 1
    | High = 0

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

    [<Literal>]
    let joker = 0uy

    let inline toByte (adjustJoker : bool) (c : char) : byte =
        if c <= '9' then byte c - byte '1'
        elif c = 'T' then 9uy
        elif c = 'J' then (if adjustJoker then joker else 10uy)
        elif c = 'Q' then 11uy
        elif c = 'K' then 12uy
        elif c = 'A' then 13uy
        else failwithf "could not parse: %c" c

    let inline private updateState (tallies : ResizeArray<_>) newNum =
        let mutable isAdded = false

        for i = 0 to tallies.Count - 1 do
            if fst tallies.[i] = newNum then
                tallies.[i] <- (fst tallies.[i], snd tallies.[i] + 1)
                isAdded <- true

        if not isAdded then
            tallies.Add (newNum, 1)

    type RankedHand = uint32

    [<Literal>]
    let fourteen = 14ul

    [<Literal>]
    let fourteenFive = fourteen * fourteen * fourteen * fourteen * fourteen

    [<Literal>]
    let fourteenFour = fourteen * fourteen * fourteen * fourteen

    [<Literal>]
    let fourteenThree = fourteen * fourteen * fourteen

    [<Literal>]
    let fourteenTwo = fourteen * fourteen

    let toInt (hand : Hand) (contents : HandContents) : RankedHand =
        uint32 hand * fourteenFive
        + uint32 contents.First * fourteenFour
        + uint32 contents.Second * fourteenThree
        + uint32 contents.Third * fourteenTwo
        + uint32 contents.Fourth * fourteen
        + uint32 contents.Fifth

    let parseHand (tallyBuffer : ResizeArray<_>) (adjustJoker : bool) (s : ReadOnlySpan<char>) : RankedHand =
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
                let mutable jokerCount = 0
                let mutable jokerPos = 0

                while jokerPos < tallyBuffer.Count && jokerCount = 0 do
                    let card, tally = tallyBuffer.[jokerPos]

                    if card = joker then
                        jokerCount <- tally
                    else
                        jokerPos <- jokerPos + 1

                jokerCount, jokerPos

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

        toInt hand contents

    type RankedHandAndBid = uint32

    [<Literal>]
    let bidSeparator = 1001ul

    let inline toRankedHandAndBid (r : RankedHand) (bid : uint32) : RankedHandAndBid = bidSeparator * r + bid

    let inline getBid (r : RankedHandAndBid) : uint32 = uint32 (r % bidSeparator)

    let parse (adjustJoker : bool) (s : string) : ResizeArray<RankedHandAndBid> =
        use mutable lines = StringSplitEnumerator.make '\n' s
        let result = ResizeArray.create 4
        let tallies = ResizeArray.create 5

        while lines.MoveNext () do
            if not lines.Current.IsEmpty then
                use mutable line = StringSplitEnumerator.make' ' ' lines.Current
                line.MoveNext () |> ignore
                let rankedHand = parseHand tallies adjustJoker line.Current
                line.MoveNext () |> ignore

                let bid =
                    UInt32.Parse (line.Current, NumberStyles.Integer, CultureInfo.InvariantCulture)

                result.Add (toRankedHandAndBid rankedHand bid)

        result

    let part1 (s : string) =
        let arr = parse false s

        arr.Sort ()

        let mutable answer = 0ul

        for i = 0 to arr.Count - 1 do
            answer <- answer + getBid arr.[i] * (uint32 i + 1ul)

        answer

    let part2 (s : string) =
        let arr = parse true s

        arr.Sort ()

        let mutable answer = 0ul

        for i = 0 to arr.Count - 1 do
            answer <- answer + getBid arr.[i] * (uint32 i + 1ul)

        answer
