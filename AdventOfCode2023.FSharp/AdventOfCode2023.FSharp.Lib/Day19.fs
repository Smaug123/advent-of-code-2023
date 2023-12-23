namespace AdventOfCode2023

open System
open System.Collections.Generic
open System.Globalization

[<RequireQualifiedAccess>]
module Day19 =

    type Component =
        | X = 0
        | M = 1
        | A = 2
        | S = 3

    [<RequireQualifiedAccess>]
    module Component =
        let ofChar (c : char) : Component =
            match c with
            | 'x' -> Component.X
            | 'm' -> Component.M
            | 'a' -> Component.A
            | 's' -> Component.S
            | c -> failwith $"bad component: %c{c}"

    type Dest =
        | Register of string
        | Accept
        | Reject

        static member OfString (s : EfficientString) : Dest =
            if s.Length = 1 && s.[0] = 'A' then Dest.Accept
            elif s.Length = 1 && s.[0] = 'R' then Dest.Reject
            else Dest.Register (s.ToString ())

    type Rule =
        {
            Component : Component
            IsLess : bool
            Operand : int
            Target : Dest
        }

        static member OfString (s : EfficientString) : Choice<Rule, Dest> =
            match s.IndexOf ':' with
            | -1 -> Choice2Of2 (Dest.OfString (s.Slice (0, s.Length)))
            | colon ->

            let dest = Dest.OfString (s.Slice (colon + 1))
            let comp = Component.ofChar s.[0]

            let isLess =
                match s.[1] with
                | '>' -> false
                | '<' -> true
                | c -> failwith $"Bad comparison: %c{c}"

            let operand =
                Int32.Parse (s.Slice (2, colon - 2), NumberStyles.None, NumberFormatInfo.InvariantInfo)

            {
                Component = comp
                IsLess = isLess
                Target = dest
                Operand = operand
            }
            |> Choice1Of2

    let inline matches (rule : Rule) x m a s =
        match rule.Component with
        | Component.A ->
            if (rule.IsLess && a < rule.Operand) || (not rule.IsLess && a > rule.Operand) then
                Some rule.Target
            else
                None
        | Component.X ->
            if (rule.IsLess && x < rule.Operand) || (not rule.IsLess && x > rule.Operand) then
                Some rule.Target
            else
                None
        | Component.M ->
            if (rule.IsLess && m < rule.Operand) || (not rule.IsLess && m > rule.Operand) then
                Some rule.Target
            else
                None
        | Component.S ->
            if (rule.IsLess && s < rule.Operand) || (not rule.IsLess && s > rule.Operand) then
                Some rule.Target
            else
                None
        | _ -> failwith "bad component"

    let rec computePart1 (components : Dictionary<string, Rule ResizeArray * Dest>) x m a s (reg : string) =
        match components.TryGetValue reg with
        | false, _ -> failwith $"no rule matched: %s{reg}"
        | true, (rules, dest) ->
            let mutable result = ValueNone
            let mutable i = 0

            while result.IsNone do
                if i = rules.Count then
                    result <- ValueSome dest
                else

                match matches rules.[i] x m a s with
                | Some dest -> result <- ValueSome dest
                | None -> i <- i + 1

            match result.Value with
            | Register reg -> computePart1 components x m a s reg
            | Accept -> true
            | Reject -> false

    let readWorkflows (rows : byref<StringSplitEnumerator>) =
        let workflows = Dictionary<string, Rule ResizeArray * Dest> ()

        while rows.MoveNext () && not rows.Current.IsEmpty do
            let brace = rows.Current.IndexOf '{'
            let name = rows.Current.Slice(0, brace).ToString ()
            let rules = ResizeArray ()

            for rule in StringSplitEnumerator.make' ',' (rows.Current.Slice(brace + 1).TrimEnd '}') do
                match Rule.OfString rule with
                | Choice1Of2 rule -> rules.Add rule
                | Choice2Of2 dest -> workflows.[name] <- (rules, dest)

        workflows

    let part1 (workflows : _) (rows : byref<StringSplitEnumerator>) =
        let mutable answer = 0

        for row in rows do
            if not row.IsEmpty then
                let mutable x = 0
                let mutable m = 0
                let mutable a = 0
                let mutable s = 0

                for comp in StringSplitEnumerator.make' ',' (row.Slice (1, row.Length - 2)) do
                    let number =
                        Int32.Parse (comp.Slice 2, NumberStyles.None, NumberFormatInfo.InvariantInfo)

                    match comp.[0] with
                    | 'x' -> x <- number
                    | 'm' -> m <- number
                    | 'a' -> a <- number
                    | 's' -> s <- number
                    | c -> failwith $"Bad char: %c{c}"

                if computePart1 workflows x m a s "in" then
                    answer <- answer + x + m + a + s

        answer

    type AcceptanceCriterion =
        | True
        | False
        | Base of Component * low : int * high : int
        | And of AcceptanceCriterion * AcceptanceCriterion
        | Or of AcceptanceCriterion * AcceptanceCriterion

    let rec acAnd a b =
        match a, b with
        | AcceptanceCriterion.Or (a1, a2), _ -> AcceptanceCriterion.Or (acAnd a1 b, acAnd a2 b)
        | AcceptanceCriterion.True, _ -> b
        | AcceptanceCriterion.False, _ -> False
        | _, AcceptanceCriterion.Or (b1, b2) -> AcceptanceCriterion.Or (acAnd a b1, acAnd a b2)
        | _, AcceptanceCriterion.True -> a
        | _, AcceptanceCriterion.False -> False
        | _, _ -> AcceptanceCriterion.And (a, b)

    let inline acOr a b =
        match a, b with
        | AcceptanceCriterion.False, _ -> b
        | AcceptanceCriterion.True, _ -> AcceptanceCriterion.True
        | _, AcceptanceCriterion.False -> a
        | _, AcceptanceCriterion.True -> AcceptanceCriterion.True
        | _, _ -> AcceptanceCriterion.Or (a, b)

    /// "low > high" means "empty interval"
    [<Struct>]
    type Interval =
        {
            Low : int
            High : int
        }

        static member Empty =
            {
                Low = 1
                High = 0
            }

    [<RequireQualifiedAccess>]
    module Interval =
        let size (i : Interval) =
            if i.Low > i.High then 0 else i.High - i.Low + 1

        let intersect (i1 : Interval) (i2 : Interval) =
            if i1.Low > i1.High || i2.Low > i2.High then
                i1
            else if i1.High < i2.Low then
                Interval.Empty
            elif i2.High < i1.Low then
                Interval.Empty
            else
                {
                    Low = max i1.Low i2.Low
                    High = min i1.High i2.High
                }

    type Conjunction =
        {
            X : Interval
            M : Interval
            A : Interval
            S : Interval
        }

        static member All =
            {
                X =
                    {
                        Low = 1
                        High = 4000
                    }
                M =
                    {
                        Low = 1
                        High = 4000
                    }
                A =
                    {
                        Low = 1
                        High = 4000
                    }
                S =
                    {
                        Low = 1
                        High = 4000
                    }
            }

    [<RequireQualifiedAccess>]
    module Conjunction =
        let conjAnd (c1 : Conjunction) (c2 : Conjunction) =
            {
                X = Interval.intersect c1.X c2.X
                M = Interval.intersect c1.M c2.M
                A = Interval.intersect c1.A c2.A
                S = Interval.intersect c1.S c2.S
            }

        /// We rely on the intervals being disjoint by construction.
        let size (x : Conjunction) : uint64 =
            uint64 (Interval.size x.X)
            * uint64 (Interval.size x.M)
            * uint64 (Interval.size x.A)
            * uint64 (Interval.size x.S)

    type UnionOfConjunctions = Conjunction list

    let rec toUnion (ac : AcceptanceCriterion) : UnionOfConjunctions =
        match ac with
        | Base (comp, low, high) ->
            match comp with
            | Component.A ->
                { Conjunction.All with
                    A =
                        {
                            Low = low
                            High = high
                        }
                }
            | Component.M ->
                { Conjunction.All with
                    M =
                        {
                            Low = low
                            High = high
                        }
                }
            | Component.S ->
                { Conjunction.All with
                    S =
                        {
                            Low = low
                            High = high
                        }
                }
            | Component.X ->
                { Conjunction.All with
                    X =
                        {
                            Low = low
                            High = high
                        }
                }
            | _ -> failwith "bad"
            |> List.singleton
        | And (a, b) ->
            [
                Conjunction.conjAnd (List.exactlyOne (toUnion a)) (List.exactlyOne (toUnion b))
            ]
        | Or (a, b) -> toUnion a @ toUnion b
        | True -> failwith "already stripped out"
        | False -> failwith "already stripped out"

    let rec acceptance
        (store : Dictionary<string, AcceptanceCriterion>)
        (workflows : Dictionary<string, ResizeArray<Rule> * Dest>)
        (key : string)
        : AcceptanceCriterion
        =
        match store.TryGetValue key with
        | true, v -> v
        | false, _ ->

        let rules, final = workflows.[key]

        let mutable result =
            match final with
            | Register s -> acceptance store workflows s
            | Accept -> AcceptanceCriterion.True
            | Reject -> AcceptanceCriterion.False

        for i = rules.Count - 1 downto 0 do
            let rule = rules.[i]

            let cond =
                if rule.IsLess then
                    AcceptanceCriterion.Base (rule.Component, 1, rule.Operand - 1)
                else
                    AcceptanceCriterion.Base (rule.Component, rule.Operand + 1, 4000)

            let negCond =
                if rule.IsLess then
                    AcceptanceCriterion.Base (rule.Component, rule.Operand, 4000)
                else
                    AcceptanceCriterion.Base (rule.Component, 1, rule.Operand)

            result <-
                match rule.Target with
                | Register target -> acOr (acAnd cond (acceptance store workflows target)) (acAnd negCond result)
                | Accept -> acOr cond (acAnd negCond result)
                | Reject -> acAnd negCond result

        store.[key] <- result
        result

    let part2 (workflows : _) (rows : byref<StringSplitEnumerator>) : uint64 =
        let acceptanceRanges = Dictionary<string, AcceptanceCriterion> ()

        let a = acceptance acceptanceRanges workflows "in"
        let union = toUnion a

        union |> List.sumBy Conjunction.size
