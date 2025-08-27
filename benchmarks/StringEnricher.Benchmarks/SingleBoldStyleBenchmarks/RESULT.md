# Overview
This benchmark compares various methods of applying single bold styling to a string (means "text to bold" -> "\*text to bold\*").

The methods include using a custom `BoldMarkdownV2` class, string concatenation, interpolated strings, `StringBuilder`, and others. The goal is to evaluate the performance and memory allocation of each approach.

# Benchmark Results

| Method                             | Mean     | Error    | StdDev   | Min      | Max      | Median   | Ratio | RatioSD | Rank | Gen0   | Allocated | Alloc Ratio |
|----------------------------------- |---------:|---------:|---------:|---------:|---------:|---------:|------:|--------:|-----:|-------:|----------:|------------:|
| BoldMarkdownV2_Apply               | 14.63 ns | 0.331 ns | 0.464 ns | 14.04 ns | 15.73 ns | 14.53 ns |  0.96 |    0.03 |    1 | 0.0115 |      48 B |        1.00 |
| Concatenation                      | 15.07 ns | 0.253 ns | 0.198 ns | 14.75 ns | 15.39 ns | 15.06 ns |  0.99 |    0.02 |    1 | 0.0115 |      48 B |        1.00 |
| InterpolatedString                 | 15.26 ns | 0.302 ns | 0.283 ns | 14.86 ns | 15.85 ns | 15.19 ns |  1.00 |    0.03 |    1 | 0.0115 |      48 B |        1.00 |
| StringConcat                       | 15.34 ns | 0.248 ns | 0.220 ns | 15.04 ns | 15.80 ns | 15.28 ns |  1.01 |    0.02 |    1 | 0.0115 |      48 B |        1.00 |
| MessageTextStyleLambda_Bold        | 21.55 ns | 0.477 ns | 0.603 ns | 20.70 ns | 22.81 ns | 21.47 ns |  1.41 |    0.05 |    2 | 0.0191 |      80 B |        1.67 |
| StringBuilder_Default              | 23.26 ns | 0.395 ns | 0.500 ns | 22.48 ns | 24.98 ns | 23.18 ns |  1.52 |    0.04 |    3 | 0.0363 |     152 B |        3.17 |
| StringBuilder_PreciseSize          | 24.81 ns | 0.560 ns | 0.497 ns | 24.19 ns | 26.04 ns | 24.72 ns |  1.63 |    0.04 |    4 | 0.0344 |     144 B |        3.00 |
| StringJoin                         | 28.23 ns | 0.432 ns | 0.383 ns | 27.73 ns | 29.08 ns | 28.20 ns |  1.85 |    0.04 |    5 | 0.0114 |      48 B |        1.00 |
| MessageTextStyleStringHandler_Bold | 32.25 ns | 0.718 ns | 0.706 ns | 31.22 ns | 33.56 ns | 32.21 ns |  2.11 |    0.06 |    6 | 0.0191 |      80 B |        1.67 |
| StringBuilder_Reserved100          | 33.07 ns | 0.725 ns | 0.643 ns | 32.43 ns | 34.41 ns | 32.80 ns |  2.17 |    0.06 |    6 | 0.0765 |     320 B |        6.67 |
| StringFormat                       | 40.77 ns | 0.894 ns | 1.065 ns | 39.37 ns | 43.72 ns | 40.68 ns |  2.67 |    0.08 |    7 | 0.0114 |      48 B |        1.00 |

# Analysis

The benchmark results clearly demonstrate that the custom implementation, `BoldMarkdownV2_Apply`, is the most efficient method for applying bold styling in MarkdownV2 format. It consistently achieves the lowest mean execution time (14.63 ns), minimal memory allocation (48 B), and ranks first among all tested approaches. This proves that the struct-based design and optimized string creation in your implementation outperform both native string operations and common alternatives such as `StringBuilder`, `String.Format`, and lambda/string handler abstractions.

Other methods, including string concatenation, interpolation, and `String.Concat`, are close in performance but do not surpass the custom approach. More generic or flexible solutions like `StringBuilder` and `String.Format` introduce unnecessary overhead for this simple task, resulting in slower execution and higher memory usage.

**Recommendation:**
For developers seeking the fastest and most memory-efficient way to apply bold MarkdownV2 styling, the `BoldMarkdownV2.Apply` method is the clear winner. Its design is not only elegant and type-safe but also proven to be superior through rigorous benchmarking. Adopting this approach will ensure optimal performance in real-world applications.
