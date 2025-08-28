# Overview
This benchmark compares various methods of applying double bold styling to a string (means "text to double bold" -> "\*\*text to double bold\*\*").

The methods include using a custom `BoldMarkdownV2` class, string concatenation, interpolated strings, `StringBuilder`, and others. The goal is to evaluate the performance and memory allocation of each approach.

# Benchmark Results

| Method                             | Mean     | Error    | StdDev   | Median   | Min      | Max      | Ratio | RatioSD | Rank | Gen0   | Allocated | Alloc Ratio |
|----------------------------------- |---------:|---------:|---------:|---------:|---------:|---------:|------:|--------:|-----:|-------:|----------:|------------:|
| BoldMarkdownV2_Apply               | 18.99 ns | 0.446 ns | 0.758 ns | 18.59 ns | 18.10 ns | 21.28 ns |  0.63 |    0.03 |    1 | 0.0115 |      48 B |        0.50 |
| StringBuilder_Default              | 25.13 ns | 0.571 ns | 0.680 ns | 25.37 ns | 23.71 ns | 26.09 ns |  0.84 |    0.03 |    2 | 0.0363 |     152 B |        1.58 |
| StringBuilder_PreciseSize          | 26.11 ns | 0.310 ns | 0.242 ns | 26.16 ns | 25.64 ns | 26.50 ns |  0.87 |    0.02 |    2 | 0.0363 |     152 B |        1.58 |
| Concatenation                      | 29.90 ns | 0.607 ns | 0.506 ns | 29.90 ns | 29.06 ns | 31.16 ns |  1.00 |    0.03 |    3 | 0.0229 |      96 B |        1.00 |
| InterpolatedString                 | 30.03 ns | 0.648 ns | 0.746 ns | 29.96 ns | 28.62 ns | 31.48 ns |  1.00 |    0.03 |    3 | 0.0229 |      96 B |        1.00 |
| StringConcat                       | 30.03 ns | 0.663 ns | 0.862 ns | 29.86 ns | 28.48 ns | 31.60 ns |  1.00 |    0.04 |    3 | 0.0229 |      96 B |        1.00 |
| StringBuilder_Reserved100          | 40.56 ns | 1.311 ns | 3.761 ns | 39.12 ns | 34.86 ns | 51.39 ns |  1.35 |    0.13 |    4 | 0.0765 |     320 B |        3.33 |
| MessageTextStyleLambda_Bold        | 42.75 ns | 0.912 ns | 1.218 ns | 42.63 ns | 40.83 ns | 44.88 ns |  1.42 |    0.05 |    4 | 0.0382 |     160 B |        1.67 |
| MessageTextStyleStringHandler_Bold | 54.08 ns | 1.134 ns | 1.553 ns | 53.69 ns | 52.29 ns | 58.14 ns |  1.80 |    0.07 |    5 | 0.0382 |     160 B |        1.67 |
| StringJoin                         | 58.27 ns | 1.155 ns | 1.135 ns | 57.82 ns | 57.15 ns | 60.80 ns |  1.94 |    0.06 |    6 | 0.0229 |      96 B |        1.00 |
| StringFormat                       | 78.44 ns | 1.545 ns | 1.518 ns | 78.60 ns | 76.09 ns | 80.46 ns |  2.61 |    0.08 |    7 | 0.0229 |      96 B |        1.00 |

# Analysis

The benchmark results clearly demonstrate that the most performant way to apply double bold styling is by using the custom BoldMarkdownV2.Apply method. This approach not only achieves the lowest mean execution time (18.99 ns) but also minimizes memory allocation (48 B), outperforming all standard .NET string manipulation techniques such as concatenation, interpolation, StringBuilder, and formatting.
Standard methods like string concatenation, interpolation, and String.Concat are reliable and familiar to most developers, but they consistently show higher execution times and memory allocations compared to the custom implementation. Even optimized usages of StringBuilder and string.Join do not match the efficiency of the custom code.
For other developers, this benchmark proves that investing in a well-designed custom solution for string styling can yield significant performance and memory benefits, especially in scenarios where such operations are frequent or performance-critical. The results validate the use of BoldMarkdownV2.Apply as the recommended approach for double bold styling in Markdown V2 contexts.