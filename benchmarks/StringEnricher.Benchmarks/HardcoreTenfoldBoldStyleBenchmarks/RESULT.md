# Overview
Attention! This is not fair benchmark, because default string operations in this benchmark have been simplified dramatically to achieve best possible performance. In real-world scenarios, string operations are usually more complex and involve more overhead.
To see the fair benchmark, please refer to [TenfoldBoldStyleBenchmarks/RESULT.md](./TenfoldBoldStyleBenchmarks/RESULT.md).

This benchmark compares various methods of applying 10x times bold styling to a string (means "text to bold 10x times" -> "\*\*\*\*\*\*\*\*\*\*text to bold 10x times\*\*\*\*\*\*\*\*\*\*").

The methods include using a custom `BoldMarkdownV2` class, string concatenation, interpolated strings, `StringBuilder`, and others. The goal is to evaluate the performance and memory allocation of each approach.

# Benchmark Results

| Method                             | Mean      | Error    | StdDev   | Median    | Min       | Max       | Ratio | RatioSD | Rank | Gen0   | Allocated | Alloc Ratio |
|----------------------------------- |----------:|---------:|---------:|----------:|----------:|----------:|------:|--------:|-----:|-------:|----------:|------------:|
| Concatenation                      |  15.30 ns | 0.327 ns | 0.255 ns |  15.34 ns |  14.88 ns |  15.70 ns |  0.98 |    0.03 |    1 | 0.0191 |      80 B |        1.00 |
| InterpolatedString                 |  15.63 ns | 0.362 ns | 0.431 ns |  15.55 ns |  14.92 ns |  16.41 ns |  1.00 |    0.04 |    1 | 0.0191 |      80 B |        1.00 |
| StringConcat                       |  15.71 ns | 0.376 ns | 0.733 ns |  15.35 ns |  14.88 ns |  18.00 ns |  1.01 |    0.05 |    1 | 0.0191 |      80 B |        1.00 |
| StringJoin                         |  30.44 ns | 0.675 ns | 0.947 ns |  30.43 ns |  29.00 ns |  32.10 ns |  1.95 |    0.08 |    2 | 0.0191 |      80 B |        1.00 |
| StringBuilder_PreciseSize          |  31.15 ns | 0.671 ns | 0.799 ns |  31.10 ns |  29.89 ns |  33.10 ns |  1.99 |    0.07 |    2 | 0.0516 |     216 B |        2.70 |
| StringBuilder_Reserved100          |  36.81 ns | 0.776 ns | 0.863 ns |  36.65 ns |  35.84 ns |  38.70 ns |  2.36 |    0.08 |    3 | 0.0842 |     352 B |        4.40 |
| StringFormat                       |  43.76 ns | 0.944 ns | 0.837 ns |  43.47 ns |  42.78 ns |  45.88 ns |  2.80 |    0.09 |    4 | 0.0191 |      80 B |        1.00 |
| StringBuilder_Default              |  51.30 ns | 0.739 ns | 0.655 ns |  51.40 ns |  50.01 ns |  52.10 ns |  3.28 |    0.10 |    5 | 0.0688 |     288 B |        3.60 |
| BoldMarkdownV2_Apply               |  57.34 ns | 1.162 ns | 1.087 ns |  57.02 ns |  55.42 ns |  59.23 ns |  3.67 |    0.12 |    6 | 0.0191 |      80 B |        1.00 |
| MessageTextStyleLambda_Bold        | 220.00 ns | 4.472 ns | 5.150 ns | 219.71 ns | 211.06 ns | 231.88 ns | 14.09 |    0.50 |    7 | 0.2294 |     960 B |       12.00 |
| MessageTextStyleStringHandler_Bold | 312.45 ns | 4.398 ns | 3.898 ns | 310.73 ns | 306.79 ns | 321.27 ns | 20.01 |    0.59 |    8 | 0.2294 |     960 B |       12.00 |

# Analysis

The presented benchmark is intentionally not fair. As noted in the overview, the default string operations (such as concatenation, interpolation, and formatting) have been dramatically simplified to achieve the best possible performance. In real-world scenarios, string operations are typically more complex and involve additional overhead, such as escaping, validation, or handling edge cases.
Despite these unfair conditions, the benchmark serves to demonstrate a key point: even when compared against highly optimized and simplified string operations, the custom BoldMarkdownV2_Apply approach performs well. Specifically, it achieves competitive execution times and, most importantly, allocates the minimum possible memory—matching the most efficient string operations in terms of memory usage.
While the direct string operations are faster due to their simplicity, BoldMarkdownV2_Apply still maintains reasonable performance and does not introduce unnecessary memory allocations. This result highlights the efficiency of the custom implementation, especially considering that in real-world applications, its additional logic would be necessary for correctness and robustness.
In summary, although the benchmark is not fair and was designed to favor the simplest string operations, it successfully demonstrates that BoldMarkdownV2_Apply is both performant and memory-efficient, even under these conditions.