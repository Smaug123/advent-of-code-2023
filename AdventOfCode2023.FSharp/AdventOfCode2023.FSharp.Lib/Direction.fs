namespace AdventOfCode2023

type Direction =
    | Left = 0
    | Right = 1
    | Up = 2
    | Down = 3

module Direction =
    let inline toUInt (d : Direction) =
        match d with
        | Direction.Left -> 0us
        | Direction.Right -> 1us
        | Direction.Up -> 2us
        | Direction.Down -> 3us
        | _ -> failwith "Bad"

    let inline ofChar (c : char) : Direction =
        match c with
        | 'L' -> Direction.Left
        | 'R' -> Direction.Right
        | 'U' -> Direction.Up
        | 'D' -> Direction.Down
        | c -> failwith $"Bad: %c{c}"
