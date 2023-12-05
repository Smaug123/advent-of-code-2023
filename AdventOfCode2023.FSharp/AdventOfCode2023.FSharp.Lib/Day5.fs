namespace AdventOfCode2023

open System

[<Struct>]
type Range =
    {
        SourceStart : uint32
        DestStart : uint32
        Len : uint32
    }

[<RequireQualifiedAccess>]
module Day5 =

    let parse (s : string) =
        use mutable lines = StringSplitEnumerator.make '\n' s
        lines.MoveNext () |> ignore

        let seeds =
            use mutable line1 = StringSplitEnumerator.make' ' ' lines.Current
            StringSplitEnumerator.chomp "seeds:" &line1
            let result = ResizeArray ()

            while line1.MoveNext () do
                result.Add (UInt32.Parse line1.Current)

            result.ToArray ()

        lines.MoveNext () |> ignore

        let mappings = ResizeArray ()

        let mutable currentMapping = null

        for line in lines do
            if line.IsEmpty then
                if not (isNull currentMapping) then
                    mappings.Add currentMapping
                    currentMapping <- null
            else if isNull currentMapping then
                currentMapping <- ResizeArray ()
            else
                use mutable line = StringSplitEnumerator.make' ' ' line
                let destStart = StringSplitEnumerator.consumeU32 &line
                let sourceStart = StringSplitEnumerator.consumeU32 &line
                let rangeLen = StringSplitEnumerator.consumeU32 &line

                {
                    SourceStart = sourceStart
                    DestStart = destStart
                    Len = rangeLen
                }
                |> currentMapping.Add

        seeds, mappings

    let part1 (s : string) =
        let seeds, mappings = parse s

        let mutable best = UInt32.MaxValue

        for seed in seeds do
            let mutable remapped = seed

            for map in mappings do
                let mutable hasRemappedThisLayer = false

                for interval in map do
                    if not hasRemappedThisLayer then
                        if
                            interval.SourceStart <= remapped
                            && remapped - interval.SourceStart < interval.Len
                        then
                            hasRemappedThisLayer <- true
                            remapped <- remapped + (interval.DestStart - interval.SourceStart)

            if remapped < best then
                best <- remapped

        best

    // The input ranges are inclusive at both ends.
    // Returns any range we didn't map.
    let private split
        (resultStarts : ResizeArray<uint32>)
        (resultEnds : ResizeArray<uint32>)
        start
        finish
        (rangeFromLayer : Range)
        : (uint32 * uint32 * (uint32 * uint32) voption) voption
        =
        let low = rangeFromLayer.SourceStart
        let high = rangeFromLayer.SourceStart + rangeFromLayer.Len - 1ul

        if low <= start then
            if finish <= high then
                // low ... start .. finish .. high
                // so the entire input range gets mapped down
                resultStarts.Add (start + rangeFromLayer.DestStart - rangeFromLayer.SourceStart)
                resultEnds.Add (finish + rangeFromLayer.DestStart - rangeFromLayer.SourceStart)

                ValueNone
            elif start <= high then
                // low .. start .. high .. finish
                // so start .. high gets mapped down
                // and high + 1 .. finish stays where it is.
                // high < finish is already guaranteed by previous if block.
                resultStarts.Add (start + rangeFromLayer.DestStart - rangeFromLayer.SourceStart)
                resultEnds.Add (high + rangeFromLayer.DestStart - rangeFromLayer.SourceStart)

                ValueSome (high + 1ul, finish, ValueNone)
            else
                ValueSome (start, finish, ValueNone)
        else if high <= finish then
            // start .. low .. high .. finish
            // so start .. low - 1 stays where it is
            // low .. high gets mapped down
            // and high + 1 .. finish stays where it is
            resultStarts.Add (low + rangeFromLayer.DestStart - rangeFromLayer.SourceStart)
            resultEnds.Add (high + rangeFromLayer.DestStart - rangeFromLayer.SourceStart)

            ValueSome (start, low - 1ul, ValueSome (high + 1ul, finish))
        elif low < finish then
            // start .. low .. finish .. high
            // so start .. low - 1 stays where it is
            // and low .. finish gets mapped down
            resultStarts.Add (low + rangeFromLayer.DestStart - rangeFromLayer.SourceStart)
            resultEnds.Add (finish + rangeFromLayer.DestStart - rangeFromLayer.SourceStart)

            ValueSome (start, low - 1ul, ValueNone)
        else
            ValueSome (start, finish, ValueNone)

    let part2 (s : string) : uint32 =
        let seeds, mappings = parse s

        let mutable intervalStarts = ResizeArray ()
        let mutable intervalEnds = ResizeArray ()

        for i = 0 to (seeds.Length - 1) / 2 do
            intervalStarts.Add seeds.[2 * i]
            intervalEnds.Add (seeds.[2 * i + 1] + seeds.[2 * i] - 1ul)

        let mutable nextIntervalStarts = ResizeArray ()
        let mutable nextIntervalEnds = ResizeArray ()

        for mapLayer in mappings do
            let mutable i = 0

            while i < intervalStarts.Count do
                // split interval according to every map
                let mutable allMoved = false
                let mutable currentRange = 0

                while not allMoved && currentRange < mapLayer.Count do
                    let range = mapLayer.[currentRange]
                    // range is e.g. 50 98 2, i.e. "98-99 goes to 50-51"
                    match split nextIntervalStarts nextIntervalEnds intervalStarts.[i] intervalEnds.[i] range with
                    | ValueNone -> allMoved <- true
                    | ValueSome (start, finish, v) ->
                        intervalStarts.[i] <- start
                        intervalEnds.[i] <- finish

                        match v with
                        | ValueNone -> ()
                        | ValueSome (start, finish) ->
                            intervalStarts.Add start
                            intervalEnds.Add finish

                    currentRange <- currentRange + 1

                if not allMoved then
                    nextIntervalStarts.Add intervalStarts.[i]
                    nextIntervalEnds.Add intervalEnds.[i]

                i <- i + 1

            let oldIntervals = intervalStarts
            oldIntervals.Clear ()
            intervalStarts <- nextIntervalStarts
            nextIntervalStarts <- oldIntervals

            let oldIntervals = intervalEnds
            oldIntervals.Clear ()
            intervalEnds <- nextIntervalEnds
            nextIntervalEnds <- oldIntervals


        // SIMD go brrr
        System.Linq.Enumerable.Min intervalStarts
