(* ::Package:: *)

FromDigits @*
  Extract[{{1}, {-1}}] @*
  StringCases[x_?DigitQ :> FromDigits[x]] /@ StringSplit[s, "\n"] // Total


{10, 1} .
  Extract[{{1}, {-1}}] @
  StringCases[
      #,
      {"one" -> 1, "two" -> 2, "three" -> 3, "four" -> 4, "five" -> 5, "six" -> 6, "seven" -> 7, "eight" -> 8, "nine" -> 9, x_?DigitQ :> FromDigits[x]}, 
      Overlaps -> True
   ]& /@ StringSplit[s, "\n"] // Total
