# Overview
This benchmark compares various methods of applying 10x times bold styling to a string (means "text to bold 10x times" -> "\*\*\*\*\*\*\*\*\*\*text to bold 10x times\*\*\*\*\*\*\*\*\*\*").

The methods include using a custom `BoldMarkdownV2` class, string concatenation, interpolated strings, `StringBuilder`, and others. The goal is to evaluate the performance and memory allocation of each approach.

# Benchmark Results

| Method                             | Mean      | Error    | StdDev    | Min       | Max       | Median    | Ratio | RatioSD | Rank | Gen0   | Allocated | Alloc Ratio |
|----------------------------------- |----------:|---------:|----------:|----------:|----------:|----------:|------:|--------:|-----:|-------:|----------:|------------:|
| BoldMarkdownV2_Apply               |  59.88 ns | 1.241 ns |  1.429 ns |  57.61 ns |  63.30 ns |  59.67 ns |  0.36 |    0.01 |    1 | 0.0191 |      80 B |        0.12 |
| StringConcat                       | 164.84 ns | 3.355 ns |  3.729 ns | 159.00 ns | 174.32 ns | 163.45 ns |  0.98 |    0.02 |    2 | 0.1528 |     640 B |        1.00 |
| Concatenation                      | 167.15 ns | 3.330 ns |  3.964 ns | 162.48 ns | 175.47 ns | 165.60 ns |  1.00 |    0.03 |    2 | 0.1528 |     640 B |        1.00 |
| InterpolatedString                 | 167.93 ns | 2.852 ns |  2.227 ns | 165.51 ns | 172.83 ns | 167.24 ns |  1.00 |    0.02 |    2 | 0.1528 |     640 B |        1.00 |
| MessageTextStyleLambda_Bold        | 229.74 ns | 4.660 ns |  6.533 ns | 220.15 ns | 245.37 ns | 229.84 ns |  1.37 |    0.04 |    3 | 0.2294 |     960 B |        1.50 |
| StringJoin                         | 288.12 ns | 5.763 ns |  6.860 ns | 279.24 ns | 305.18 ns | 284.44 ns |  1.72 |    0.05 |    4 | 0.1526 |     640 B |        1.00 |
| StringBuilder_PreciseSize          | 298.59 ns | 5.783 ns |  6.660 ns | 287.19 ns | 311.05 ns | 297.40 ns |  1.78 |    0.04 |    4 | 0.4301 |    1800 B |        2.81 |
| MessageTextStyleStringHandler_Bold | 314.20 ns | 6.312 ns |  7.514 ns | 303.32 ns | 330.47 ns | 311.59 ns |  1.87 |    0.05 |    4 | 0.2294 |     960 B |        1.50 |
| StringBuilder_Reserved100          | 349.32 ns | 6.710 ns | 16.461 ns | 327.91 ns | 395.82 ns | 345.84 ns |  2.08 |    0.10 |    5 | 0.8030 |    3360 B |        5.25 |
| StringFormat                       | 425.01 ns | 5.864 ns |  4.897 ns | 414.28 ns | 433.66 ns | 425.30 ns |  2.53 |    0.04 |    6 | 0.1526 |     640 B |        1.00 |
| StringBuilder_Default              | 445.78 ns | 8.537 ns |  8.767 ns | 427.03 ns | 458.42 ns | 447.31 ns |  2.66 |    0.06 |    6 | 0.5755 |    2408 B |        3.76 |

# Analysis

The benchmark results clearly demonstrate that the custom BoldMarkdownV2.Apply implementation is the most performant method for applying bold styling tenfold to a string. It achieves the lowest mean execution time (59.88 ns), the lowest memory allocation (80 B), and the best overall ranking among all tested approaches.
Compared to standard .NET string manipulation techniques such as concatenation, interpolation, StringBuilder, and string.Format, the custom code is significantly faster and more memory-efficient. For example, the next fastest methods—StringConcat, Concatenation, and InterpolatedString—are nearly three times slower and allocate eight times more memory than BoldMarkdownV2.Apply. More complex or generic approaches like StringBuilder and String.Format are even less efficient, both in terms of speed and memory usage.
This result proves that using the custom BoldMarkdownV2.Apply method is the optimal choice for developers who need high-performance, low-overhead string styling in their applications. Adopting this implementation will yield substantial improvements in both speed and resource consumption compared to standard library methods or alternative custom solutions.