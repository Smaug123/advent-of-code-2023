# Advent of Code 2023

[Puzzle site](https://adventofcode.com/2023).

# Speed

Ahead-of-time compiled with `PublishAot`, M1 Max.
The format is: "answer part1\ntime\nanswer part2\ntime\n...", with possible extra lines indicating how long it took to parse the input if I happen to have split that out.

After day 3:

```
54304
0.549458ms
54418
0.710375ms
2727
0.119959ms
56580
0.155708ms
0.1395ms parse
540131
0.1395ms
86879020
0.840791ms
4.144166ms total
```

# Building yourself

Note that `PublishAot` assumes a lot of stuff about your environment, which is not necessarily true.
The given flake should allow you to complete the publish except for a linking stage at the end: the publish will print out a failed command line, and you'll have to strip out some `-o` flags from it and run it manually.
Then run `dotnet publish` again and it should succeed.
