namespace AdventOfCode2023.ResizeArray

open System

type ResizeArray<'T> =
    private
        {
            mutable Array : 'T array
            mutable Length : int
        }

    member this.Count = this.Length
    member this.Clear () = this.Length <- 0

    member this.Add (t : 'T) =
        if this.Length < this.Array.Length then
            this.Array.[this.Length] <- t
        else
            let newLength = this.Length * 2
            let newArray = Array.zeroCreate<'T> newLength
            Array.blit this.Array 0 newArray 0 this.Length
            newArray.[this.Length] <- t
            this.Array <- newArray

        this.Length <- this.Length + 1

    member this.Item
        with get (i : int) = this.Array.[i]
        and set (i : int) (t : 'T) = this.Array.[i] <- t

    member this.Sort () =
        Span(this.Array).Slice(0, this.Count).Sort ()

[<RequireQualifiedAccess>]
module ResizeArray =
    let create<'T> (capacity : int) =
        {
            Array = Array.zeroCreate<'T> capacity
            Length = 0
        }
