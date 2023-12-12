# Advent of Code 2023

[Puzzle site](https://adventofcode.com/2023).

# Speed

Ahead-of-time compiled with `PublishAot`, M1 Max.
The format is: "answer part1\ntime\nanswer part2\ntime\n...", with possible extra lines indicating how long it took to parse the input if I happen to have split that out.

BenchmarkDotNet:

```
| Day | IsPartOne | Mean      | Error    | StdDev   |
|---- |---------- |----------:|---------:|---------:|
| 1   | True      |  10.23 us | 0.036 us | 0.032 us |
| 1   | False     |  17.93 us | 0.203 us | 0.180 us |
| 2   | True      |  17.39 us | 0.080 us | 0.075 us |
| 2   | False     |  25.42 us | 0.155 us | 0.145 us |
| 3   | True      |  65.60 us | 0.393 us | 0.328 us |
| 3   | False     | 145.52 us | 0.256 us | 0.200 us |
| 4   | True      | 109.78 us | 0.236 us | 0.209 us |
| 4   | False     | 110.34 us | 0.081 us | 0.063 us |
| 5   | True      |  13.44 us | 0.045 us | 0.042 us |
| 5   | False     |  61.70 us | 0.199 us | 0.177 us |

| Day | IsPartOne | Mean           | Error       | StdDev      |
|---- |---------- |---------------:|------------:|------------:|
| 6   | True      |       314.7 ns |     1.87 ns |     1.65 ns |
| 6   | False     |       316.3 ns |     0.31 ns |     0.26 ns |
| 7   | True      |    89,256.3 ns |   578.24 ns |   540.88 ns |
| 7   | False     |    95,062.7 ns |   921.75 ns |   862.21 ns |
| 8   | True      |   423,461.0 ns | 7,218.95 ns | 6,752.61 ns |
| 8   | False     | 2,045,302.1 ns | 4,338.61 ns | 3,846.06 ns |
| 9   | True      | 1,390,976.2 ns | 2,171.39 ns | 1,813.21 ns |
| 9   | False     | 2,173,468.1 ns | 3,171.04 ns | 2,647.96 ns |
| 10  | True      |    57,460.7 ns | 1,135.45 ns | 2,160.31 ns |
| 10  | False     |   694,553.9 ns | 2,935.74 ns | 2,746.09 ns |

| Day | IsPartOne | Mean      | Error     | StdDev    | Median    |
|---- |---------- |----------:|----------:|----------:|----------:|
| 11  | True      | 39.085 ms | 0.0355 ms | 0.0297 ms | 39.082 ms |
| 11  | False     | 38.608 ms | 0.0270 ms | 0.0211 ms | 38.617 ms |
| 12  | True      |  1.846 ms | 0.0044 ms | 0.0041 ms |  1.845 ms |
| 12  | False     | 18.692 ms | 0.3676 ms | 0.3775 ms | 18.962 ms |
```

After day 12, a single run of the ahead-of-time compiled version:

```
=====Day 1=====
54304
3.418417ms
54418
0.317958ms
=====Day 2=====
2727
0.079917ms
56580
0.107292ms
=====Day 3=====
0.140292ms parse
540131
0.140292ms
86879020
0.664416ms
=====Day 4=====
27454
0.390541ms
6857330
0.360375ms
=====Day 5=====
806029445
0.161917ms
59370572
0.249708ms
=====Day 6=====
32076
0.002917ms
34278221
0.001667ms
=====Day 7=====
250058342
0.409792ms
250506580
0.431167ms
=====Day 8=====
19199
1.192792ms
13663968099527
5.276083ms
=====Day 9=====
1898776583
3.775667ms
1100
5.365875ms
=====Day 10=====
6842
0.201208ms
393
2.226042ms
=====Day 11=====
0.11225ms parse
9947476
48.423ms
519939907614
34.836125ms
=====Day 12=====
7402
4.704375ms
3384337640277
31.825583ms
151.644334ms total
```

# Building yourself

Note that `PublishAot` assumes a lot of stuff about your environment, which is not necessarily true.
The given flake should allow you to complete the publish except for a linking stage at the end: the publish will print out a failed command line, and you'll have to strip out some `-o` flags from it and run it manually.
Then run `dotnet publish` again and it should succeed.
