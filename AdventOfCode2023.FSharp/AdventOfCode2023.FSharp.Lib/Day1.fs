namespace AdventOfCode2023

open System

[<RequireQualifiedAccess>]
module Day1 =

    let firstDigit (s : ReadOnlySpan<char>) =
        let mutable pos = 0

        while '0' > s.[pos] || s.[pos] > '9' do
            pos <- pos + 1

        byte s.[pos] - byte '0'

    // No surrogate pairs please!
    let lastDigit (s : ReadOnlySpan<char>) =
        let mutable pos = s.Length - 1

        while '0' > s.[pos] || s.[pos] > '9' do
            pos <- pos - 1

        byte s.[pos] - byte '0'

    let part1 (s : string) =
        use enum = StringSplitEnumerator.make '\n' s
        let mutable total = 0

        for line in enum do
            if not line.IsEmpty then
                let firstDigit = firstDigit line
                let lastDigit = lastDigit line

                total <- total + int (lastDigit + 10uy * firstDigit)

        total

    let table =
        [|
            "one", 1uy
            "two", 2uy
            "three", 3uy
            "four", 4uy
            "five", 5uy
            "six", 6uy
            "seven", 7uy
            "eight", 8uy
            "nine", 9uy
        |]

    let firstDigitIncSpelled (s : ReadOnlySpan<char>) =
        let mutable pos = 0
        let mutable answer = 255uy

        while answer = 255uy do
            if s.[pos] >= '0' && s.[pos] <= '9' then
                answer <- byte s.[pos] - byte '0'
            else
                for i, value in table do
                    if
                        pos + i.Length < s.Length
                        && MemoryExtensions.SequenceEqual (s.Slice (pos, i.Length), i)
                    then
                        answer <- value

                pos <- pos + 1

        answer

    let lastDigitIncSpelled (s : ReadOnlySpan<char>) =
        let mutable pos = s.Length - 1
        let mutable answer = 255uy

        while answer = 255uy do
            if s.[pos] >= '0' && s.[pos] <= '9' then
                answer <- byte s.[pos] - byte '0'
            else
                for i, value in table do
                    if
                        pos - i.Length + 1 >= 0
                        && MemoryExtensions.SequenceEqual (s.Slice (pos - i.Length + 1, i.Length), i)
                    then
                        answer <- value

                pos <- pos - 1

        answer

    let part2 (s : string) =
        use enum = StringSplitEnumerator.make '\n' s
        let mutable total = 0

        for line in enum do
            if not line.IsEmpty then
                total <- total + int (10uy * firstDigitIncSpelled line + lastDigitIncSpelled line)

        total
