namespace AdventOfCode2023

open System

[<RequireQualifiedAccess>]
module Day1 =

    let inline firstDigit (s : ReadOnlySpan<char>) =
        let pos = s.IndexOfAnyInRange ('0', '9')
        byte s.[pos] - byte '0'

    // No surrogate pairs please!
    let inline lastDigit (s : ReadOnlySpan<char>) =
        let pos = s.LastIndexOfAnyInRange ('0', '9')
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

    let isDigitSpelled (s : ReadOnlySpan<char>) (pos : int) (answer : byref<byte>) =
        // Can't be bothered to write a jump-table compiler
        if s.[pos] >= '0' && s.[pos] <= '9' then
            answer <- byte s.[pos] - byte '0'
        else if s.[pos] = 'o' then
            if pos + 2 < s.Length && s.[pos + 1] = 'n' && s.[pos + 2] = 'e' then
                answer <- 1uy
        elif s.[pos] = 't' then
            if pos + 2 < s.Length && s.[pos + 1] = 'w' && s.[pos + 2] = 'o' then
                answer <- 2uy
            elif
                pos + 4 < s.Length
                && s.[pos + 1] = 'h'
                && s.[pos + 2] = 'r'
                && s.[pos + 3] = 'e'
                && s.[pos + 4] = 'e'
            then
                answer <- 3uy
        elif s.[pos] = 'f' then
            if pos + 3 < s.Length then
                if s.[pos + 1] = 'o' && s.[pos + 2] = 'u' && s.[pos + 3] = 'r' then
                    answer <- 4uy
                elif s.[pos + 1] = 'i' && s.[pos + 2] = 'v' && s.[pos + 3] = 'e' then
                    answer <- 5uy
        elif s.[pos] = 's' then
            if pos + 2 < s.Length && s.[pos + 1] = 'i' && s.[pos + 2] = 'x' then
                answer <- 6uy
            elif
                pos + 4 < s.Length
                && s.[pos + 1] = 'e'
                && s.[pos + 2] = 'v'
                && s.[pos + 3] = 'e'
                && s.[pos + 4] = 'n'
            then
                answer <- 7uy
        elif s.[pos] = 'e' then
            if
                pos + 4 < s.Length
                && s.[pos + 1] = 'i'
                && s.[pos + 2] = 'g'
                && s.[pos + 3] = 'h'
                && s.[pos + 4] = 't'
            then
                answer <- 8uy
        elif s.[pos] = 'n' then
            if
                pos + 3 < s.Length
                && s.[pos + 1] = 'i'
                && s.[pos + 2] = 'n'
                && s.[pos + 3] = 'e'
            then
                answer <- 9uy

    let firstDigitIncSpelled (s : ReadOnlySpan<char>) =
        let mutable pos = 0
        let mutable answer = 255uy

        while answer = 255uy do
            isDigitSpelled s pos &answer
            pos <- pos + 1

        answer

    let lastDigitIncSpelled (s : ReadOnlySpan<char>) =
        let mutable pos = s.Length - 1
        let mutable answer = 255uy

        while answer = 255uy do
            isDigitSpelled s pos &answer
            pos <- pos - 1

        answer

    let part2 (s : string) =
        use enum = StringSplitEnumerator.make '\n' s
        let mutable total = 0

        for line in enum do
            if not line.IsEmpty then
                total <- total + int (10uy * firstDigitIncSpelled line + lastDigitIncSpelled line)

        total
