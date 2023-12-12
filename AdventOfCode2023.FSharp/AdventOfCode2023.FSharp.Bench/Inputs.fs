namespace AdventOfCode2023

open System.IO
open System.Reflection

[<RequireQualifiedAccess>]
module Inputs =
    let days =
        let mutable dir = Assembly.GetEntryAssembly().Location |> FileInfo |> _.Directory

        while not (dir.EnumerateDirectories () |> Seq.exists (fun i -> i.Name = "inputs")) do
            dir <- dir.Parent

            if isNull dir then
                failwith "reached root of filesystem without finding inputs dir"

        Array.init 12 (fun day -> Path.Combine (dir.FullName, "inputs", $"day%i{day + 1}.txt") |> File.ReadAllText)

    let inline day (i : int) = days.[i - 1]
