namespace AdventOfCode2023

[<Struct>]
type Rational<'a
    when 'a : (static member (+) : 'a * 'a -> 'a)
    and 'a : (static member (*) : 'a * 'a -> 'a)
    and 'a : (static member (/) : 'a * 'a -> 'a)
    and 'a : (static member (-) : 'a * 'a -> 'a)
    and 'a : (static member Zero : 'a)
    and 'a : (static member One : 'a)
    and 'a : comparison> =
    {
        Numerator : 'a
        Denominator : 'a
    }

    static member inline (+) (a : Rational<'a>, b : Rational<'a>) =
        let numerator = a.Numerator * b.Denominator + b.Numerator * a.Denominator
        let denominator = a.Denominator * b.Denominator
        let hcf = (Arithmetic.euclideanAlgorithm numerator denominator).Hcf

        {
            Numerator = numerator / hcf
            Denominator = denominator / hcf
        }

    static member inline (*) (a : Rational<'a>, b : Rational<'a>) =
        let numerator = a.Numerator * b.Numerator
        let denominator = a.Denominator * b.Denominator
        let hcf = (Arithmetic.euclideanAlgorithm numerator denominator).Hcf

        {
            Numerator = numerator / hcf
            Denominator = denominator / hcf
        }

[<RequireQualifiedAccess>]
module Rational =
    let inline ofInt< ^a
        when 'a : (static member (+) : 'a * 'a -> 'a)
        and 'a : (static member (*) : 'a * 'a -> 'a)
        and 'a : (static member (/) : 'a * 'a -> 'a)
        and 'a : (static member (-) : 'a * 'a -> 'a)
        and 'a : (static member Zero : 'a)
        and 'a : (static member One : 'a)
        and 'a : comparison>
        (a : 'a)
        =
        {
            Numerator = a
            Denominator = LanguagePrimitives.GenericOne
        }

    let inline make< ^a
        when 'a : (static member (+) : 'a * 'a -> 'a)
        and 'a : (static member (*) : 'a * 'a -> 'a)
        and 'a : (static member (/) : 'a * 'a -> 'a)
        and 'a : (static member (-) : 'a * 'a -> 'a)
        and 'a : (static member Zero : 'a)
        and 'a : (static member One : 'a)
        and 'a : comparison>
        (numerator : 'a)
        (denominator : 'a)
        =
        let hcf = (Arithmetic.euclideanAlgorithm numerator denominator).Hcf

        {
            Numerator = numerator / hcf
            Denominator = denominator / hcf
        }

    let inline assertIntegral< ^a
        when 'a : (static member (+) : 'a * 'a -> 'a)
        and 'a : (static member (*) : 'a * 'a -> 'a)
        and 'a : (static member (/) : 'a * 'a -> 'a)
        and 'a : (static member (-) : 'a * 'a -> 'a)
        and 'a : (static member Zero : 'a)
        and 'a : (static member One : 'a)
        and 'a : comparison>
        (r : Rational<'a>)
        =
        r.Numerator
