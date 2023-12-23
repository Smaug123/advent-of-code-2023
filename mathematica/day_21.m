(* ::Package:: *)

(* ::Input:: *)
(*s="...........*)
(*.....###.#.*)
(*.###.##..#.*)
(*..#.#...#..*)
(*....#.#....*)
(*.##..S####.*)
(*.##..#...#.*)
(*.......##..*)
(*.##.#.####.*)
(*.##..##.##.*)
(*...........";*)


(* ::Input:: *)
(*reachable[grid_,{row_Integer,col_Integer}]:=reachable[grid,{row,col}]=Select[{{row+1,col},{row-1,col},{row,col+1},{row,col-1}},*)
(*1<=#[[1]]<=Length[grid]&&1<=#[[2]]<=Length[First@grid]&&grid[[#[[1]],#[[2]]]]!="#"&*)
(*]*)


(* ::Input:: *)
(*f[grid_,pos_,0]:={pos}*)
(*f[grid_,pos_,timestepsRemaining_Integer]:=*)
(*f[grid,pos,timestepsRemaining]=DeleteDuplicates@Flatten[f[grid,#,timestepsRemaining-1]&/@reachable[grid,pos],1]*)


(* ::Input:: *)
(*With[{grid=Characters/@StringSplit[s,"\n"]},f[grid,FirstPosition[grid,"S"],64]//Length]*)
