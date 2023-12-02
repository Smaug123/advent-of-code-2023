(* ::Package:: *)

parseLine[s_] :=
  Block[{a, b},
    {a, b} = StringSplit[s, ": "];
    {FromDigits @ Last @ StringSplit[a], Rule @@@ (Reverse /@ MapAt[FromDigits,
       StringSplit[StringSplit[#, ", "], " "], {All, 1}])& /@ StringSplit[b,
       "; "]}
  ]


With[{games = parseLine /@ StringSplit[s, "\n"]},
    First /@ Select[games, AllTrue[#[[2]], And @@ Thread[({"red", "green",
       "blue"} /. #) <= {12, 13, 14}] /. {"red" -> 0, "green" -> 0, "blue" 
      -> 0}&]&]
  ] // Total


With[{games = parseLine /@ StringSplit[s, "\n"]},
    Times @@ Max /@ Transpose[{"red", "green", "blue"} /. #[[2]]] /. 
      _String -> 0& /@ games
  ] // Total
