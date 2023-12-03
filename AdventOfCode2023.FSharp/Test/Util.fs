namespace AdventOfCode2023.Test

open System.IO
open System.Reflection

type Dummy =
    class
    end

[<RequireQualifiedAccess>]
module Assembly =
    let getEmbeddedResource (assembly : Assembly) (name : string) : string =
        let names = assembly.GetManifestResourceNames ()
        let names = names |> Seq.filter (fun s -> s.EndsWith name)

        use s =
            names
            |> Seq.exactlyOne
            |> assembly.GetManifestResourceStream
            |> fun s -> new StreamReader (s)

        s.ReadToEnd ()
