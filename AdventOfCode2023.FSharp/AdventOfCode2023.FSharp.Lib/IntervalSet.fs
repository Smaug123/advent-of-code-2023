namespace AdventOfCode2023

type IntervalSet = private | IntervalSet of (int * int) list

[<RequireQualifiedAccess>]
module IntervalSet =
    let empty = IntervalSet []

    let private add' (low : int) (high : int) intervals : _ list =
        let rec go (low : int) (high : int) (intervals : (int * int) list) =
            match intervals with
            | [] -> [ low, high ]
            | (lowExisting, highExisting) :: intervals ->
                if low > highExisting then
                    (lowExisting, highExisting) :: go low high intervals
                elif high < lowExisting then
                    (low, high) :: (lowExisting, highExisting) :: intervals
                elif high = lowExisting then
                    (low, highExisting) :: intervals
                elif high <= highExisting then
                    (min low lowExisting, highExisting) :: intervals
                else
                    // low <= highExisting, highExisting < high
                    (min low lowExisting, highExisting) :: go (highExisting + 1) high intervals

        go low high intervals

    let add (low : int) (high : int) (IntervalSet intervals) : IntervalSet = add' low high intervals |> IntervalSet

    let contains (x : int) (IntervalSet intervals) : bool =
        let rec go (intervals : (int * int) list) =
            match intervals with
            | [] -> false
            | (low, high) :: intervals ->
                if low > x then false
                elif x <= high then true
                else go intervals

        go intervals

    let private union' i1 i2 =
        (i2, i1) ||> List.fold (fun i (low, high) -> add' low high i)

    let union (IntervalSet i1) (IntervalSet i2) : IntervalSet = union' i1 i2 |> IntervalSet

    let private intersectionHelper (low : int) (high : int) (ints : (int * int) list) =
        let rec go (low : int) (high : int) (ints : (int * int) list) =
            match ints with
            | [] -> []
            | (lowExisting, highExisting) :: ints ->
                if low > highExisting then
                    go low high ints
                elif high < lowExisting then
                    []
                elif high <= highExisting then
                    [ max low lowExisting, high ]
                else
                    (max low lowExisting, highExisting) :: go (highExisting + 1) high ints

        go low high ints

    let private intersection' i1 i2 =
        // a int (b U c) = (a int b) U (a int c)
        ([], i1)
        ||> List.fold (fun soFar (low, high) -> union' soFar (intersectionHelper low high i2))

    let intersection (IntervalSet i1) (IntervalSet i2) : IntervalSet = intersection' i1 i2 |> IntervalSet

    let count (IntervalSet i) =
        i |> List.sumBy (fun (low, high) -> high - low + 1)
