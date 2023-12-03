namespace AdventOfCode2023

#if DEBUG
#else
#nowarn "9"
#endif

open System.Diagnostics
open System.IO

module Program =

    let inline toUs (ticks : int64) =
        1_000_000.0 * float ticks / float Stopwatch.Frequency

    [<EntryPoint>]
    let main argv =
        let endToEnd = Stopwatch.StartNew ()
        endToEnd.Restart ()

        let sw = Stopwatch.StartNew ()
        sw.Restart ()
        let contents = File.ReadAllBytes argv.[0]
        sw.Stop ()
        System.Console.Error.WriteLine ("Reading file (us): " + (toUs sw.ElapsedTicks).ToString ())

        sw.Restart ()
        let resultArr, len, lineCount = Day3.parse contents

        sw.Stop ()
        System.Console.Error.WriteLine ("Populating array (us): " + (toUs sw.ElapsedTicks).ToString ())

#if DEBUG
        let contents =
            {
                Elements = Array.take len resultArr
                Width = len / lineCount
            }
#else
        use ptr = fixed resultArr
        let contents =
            {
                Elements = ptr
                Length = len
                Width = len / lineCount
            }
#endif
        // |> Array.map (fun s -> Array.init s.Length (fun i -> if s.[i] = '.' then 100uy elif s.[i] = '*' then 255uy elif '0' <= s.[i] && s.[i] <= '9' then byte s.[i] - byte '0' else 254uy))

        sw.Restart ()
        let part1 = Day3.part1 contents
        sw.Stop ()
        System.Console.Error.WriteLine ("Part 1 (us): " + (toUs sw.ElapsedTicks).ToString ())
        System.Console.WriteLine (part1.ToString ())

        sw.Restart ()
        let part2 = Day3.part2 contents
        sw.Stop ()
        System.Console.Error.WriteLine ("Part 2 (us): " + (toUs sw.ElapsedTicks).ToString ())
        System.Console.WriteLine (part2.ToString ())

        endToEnd.Stop ()
        System.Console.Error.WriteLine ("Total (us): " + (toUs endToEnd.ElapsedTicks).ToString ())

        0