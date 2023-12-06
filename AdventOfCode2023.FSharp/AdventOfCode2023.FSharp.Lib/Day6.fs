namespace AdventOfCode2023

open System

[<RequireQualifiedAccess>]
module Day6 =

    let parse (s : string) =
        use mutable lines = StringSplitEnumerator.make '\n' s

        let times =
            lines.MoveNext () |> ignore
            use mutable line = StringSplitEnumerator.make' ' ' lines.Current
            StringSplitEnumerator.chomp "Time:" &line
            line.MoveNext () |> ignore
            let times = ResizeArray ()

            while line.MoveNext () do
                if not line.Current.IsEmpty then
                    times.Add (UInt64.Parse line.Current)

            times

        let distance =
            lines.MoveNext () |> ignore
            use mutable line = StringSplitEnumerator.make' ' ' lines.Current
            StringSplitEnumerator.chomp "Distance:" &line
            line.MoveNext () |> ignore
            let distance = ResizeArray ()

            while line.MoveNext () do
                if not line.Current.IsEmpty then
                    distance.Add (UInt64.Parse line.Current)

            distance

        times, distance

    let furthest (distance : uint64) (toBeat : uint64) =
        // Count i in [1 .. distance - 1] such that (distance - i) * i > toBeat
        // i.e. such that distance * i - i * i > toBeat
        // -i^2 + distance * i - toBeat = 0 when:
        // i = (distance +- sqrt(distance^2 - 4 * toBeat)) / 2
        let distFloat = float distance
        let inside = sqrt (distFloat * distFloat - 4.0 * float toBeat)
        let limit1 = (distFloat + inside) / 2.0
        let limit2 = (distFloat - sqrt (distFloat * distFloat - 4.0 * float toBeat)) / 2.0
        // round limit2 up and limit1 down
        let limit1 = uint64 (floor limit1)
        let limit2 = uint64 (ceil limit2)
        // cope with edge case of an exact square
        if (uint64 inside) * (uint64 inside) = uint64 distance * uint64 distance - 4uL * uint64 toBeat then
            limit1 - limit2 - 1uL
        else
            limit1 - limit2 + 1uL

    let part1 (s : string) =
        let times, distance = parse s
        let mutable answer = 1uL

        for i = 0 to times.Count - 1 do
            let time = times.[i]
            let distance = distance.[i]
            let winners = furthest time distance
            answer <- answer * winners

        answer

    let concat (digits : ResizeArray<uint64>) : uint64 =
        let mutable answer = 0uL

        for digit in digits do
            let mutable power = 10uL

            while digit >= power do
                power <- power * 10uL

            answer <- answer * power + digit

        answer

    let part2 (s : string) =
        let times, distance = parse s
        let concatTime = concat times
        let concatDist = concat distance
        furthest concatTime concatDist
