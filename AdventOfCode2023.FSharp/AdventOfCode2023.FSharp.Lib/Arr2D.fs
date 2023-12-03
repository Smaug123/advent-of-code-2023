namespace AdventOfCode2023

#if DEBUG
#else
#nowarn "9"
#endif

open Microsoft.FSharp.NativeInterop

[<Struct>]
#if DEBUG
type Arr2D<'a> =
    {
        Elements : 'a array
        Width : int
    }

    member this.Height = this.Elements.Length / this.Width
#else
type Arr2D<'a when 'a : unmanaged> =
    {
        Elements : nativeptr<'a>
        Length : int
        Width : int
    }

    member this.Height = this.Length / this.Width
#endif

[<RequireQualifiedAccess>]
module Arr2D =

    /// It's faster to iterate forward over the first argument, `x`.
    let inline get (arr : Arr2D<'a>) (x : int) (y : int) : 'a =
#if DEBUG
        arr.Elements.[y * arr.Width + x]
#else
        NativePtr.get arr.Elements (y * arr.Width + x)
#endif

    let inline set (arr : Arr2D<'a>) (x : int) (y : int) (newVal : 'a) : unit =
#if DEBUG
        arr.Elements.[y * arr.Width + x] <- newVal
#else
        NativePtr.write (NativePtr.add arr.Elements (y * arr.Width + x)) newVal
#endif

#if DEBUG
    let create (width : int) (height : int) (value : 'a) : Arr2D<'a> =
        let arr = Array.create (width * height) value

        {
            Width = width
            Elements = arr
        }
#else
    /// The input array must be at least of size width * height
    let create (arr : nativeptr<'a>) (width : int) (height : int) (value : 'a) : Arr2D<'a> =
        {
            Width = width
            Elements = arr
            Length = width * height
        }
#endif

    [<RequiresExplicitTypeArguments>]
#if DEBUG
    let zeroCreate<'a when 'a : unmanaged> (width : int) (height : int) : Arr2D<'a> =
        {
            Elements = Array.zeroCreate (width * height)
            Width = width
        }
#else
    let zeroCreate<'a when 'a : unmanaged> (elts : nativeptr<'a>) (width : int) (height : int) : Arr2D<'a> =
        {
            Elements = elts
            Width = width
            Length = width * height
        }
#endif

    /// The closure is given x and then y.
#if DEBUG
    let inline init (width : int) (height : int) (f : int -> int -> 'a) : Arr2D<'a> =
        let result = zeroCreate<'a> width height
#else
    let inline init (arr : nativeptr<'a>) (width : int) (height : int) (f : int -> int -> 'a) : Arr2D<'a> =
        let result = zeroCreate<'a> arr width height
#endif

        for y = 0 to height - 1 do
            for x = 0 to width - 1 do
                set result x y (f x y)

        result

    let inline clear (a : Arr2D<'a>) : unit =
#if DEBUG
        System.Array.Clear a.Elements
#else
        NativePtr.initBlock a.Elements 0uy (uint32 sizeof<'a> * uint32 a.Length)
#endif
