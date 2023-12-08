namespace AdventOfCode2023

[<RequireQualifiedAccess>]
module Arithmetic =

    [<Struct>]
    type EuclidResult<'a> =
        {
            Hcf : 'a
            A : 'a
            B : 'a
        }

    /// Compute floor(sqrt(i)).
    let inline sqrt (i : ^a) =
        if i <= LanguagePrimitives.GenericOne then
            i
        else
            let rec go start =
                let next = start + LanguagePrimitives.GenericOne
                let sqr = next * next

                if sqr < LanguagePrimitives.GenericZero then
                    // Overflow attempted, so the sqrt is between start and next
                    start
                elif i < sqr then
                    start
                elif i = sqr then
                    next
                else
                    go next

            go LanguagePrimitives.GenericOne

    /// Find Hcf, A, B s.t. A * a + B * b = Hcf, and Hcf is the highest common factor of a and b.
    let inline euclideanAlgorithm (a : ^a) (b : ^a) : EuclidResult< ^a > =
        let rec go rMin1 r sMin1 s tMin1 t =
            if r = LanguagePrimitives.GenericZero then
                {
                    Hcf = rMin1
                    A = sMin1
                    B = tMin1
                }
            else
                let newQ = rMin1 / r
                go r (rMin1 - newQ * r) s (sMin1 - newQ * s) t (tMin1 - newQ * t)

        let maxA = max a b
        let minB = min a b

        let result =
            go
                maxA
                minB
                LanguagePrimitives.GenericOne
                LanguagePrimitives.GenericZero
                LanguagePrimitives.GenericZero
                LanguagePrimitives.GenericOne

        if a = maxA then
            result
        else
            {
                Hcf = result.Hcf
                A = result.B
                B = result.A
            }
