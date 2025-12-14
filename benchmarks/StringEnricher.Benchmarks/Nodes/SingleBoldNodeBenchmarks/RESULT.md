# Overview
This benchmark compares various methods of applying single bold styling to a string (means "text to bold" -> "\*text to bold\*").

The methods include using a custom `BoldMarkdownV2` class, string concatenation, interpolated strings, `StringBuilder`, and others. The goal is to evaluate the performance and memory allocation of each approach.

# Benchmark Results

| Method                             | Mean     | Error    | StdDev   | Min      | Max      | Median   | Ratio | RatioSD | Rank | Gen0   | Allocated | Alloc Ratio |
|----------------------------------- |---------:|---------:|---------:|---------:|---------:|---------:|------:|--------:|-----:|-------:|----------:|------------:|
| BoldMarkdownV2_Apply               | 13.19 ns | 0.322 ns | 0.419 ns | 12.19 ns | 13.94 ns | 13.31 ns |  0.96 |    0.03 |    1 | 0.0115 |      48 B |        1.00 |
| StringConcat                       | 13.73 ns | 0.335 ns | 0.385 ns | 12.91 ns | 14.35 ns | 13.73 ns |  1.00 |    0.03 |    1 | 0.0115 |      48 B |        1.00 |
| InterpolatedString                 | 13.78 ns | 0.297 ns | 0.248 ns | 13.52 ns | 14.41 ns | 13.70 ns |  1.00 |    0.02 |    1 | 0.0115 |      48 B |        1.00 |
| Concatenation                      | 13.87 ns | 0.329 ns | 0.492 ns | 13.05 ns | 14.92 ns | 13.92 ns |  1.01 |    0.04 |    1 | 0.0115 |      48 B |        1.00 |
| MessageTextStyleLambda_Bold        | 19.39 ns | 0.216 ns | 0.202 ns | 19.06 ns | 19.69 ns | 19.39 ns |  1.41 |    0.03 |    2 | 0.0191 |      80 B |        1.67 |
| StringBuilder_Default              | 21.11 ns | 0.480 ns | 0.514 ns | 19.85 ns | 21.77 ns | 21.25 ns |  1.53 |    0.04 |    3 | 0.0363 |     152 B |        3.17 |
| StringBuilder_PreciseSize          | 22.91 ns | 0.514 ns | 0.631 ns | 21.72 ns | 23.84 ns | 23.06 ns |  1.66 |    0.05 |    4 | 0.0344 |     144 B |        3.00 |
| StringJoin                         | 25.64 ns | 0.299 ns | 0.280 ns | 25.26 ns | 26.26 ns | 25.61 ns |  1.86 |    0.04 |    5 | 0.0115 |      48 B |        1.00 |
| MessageTextStyleStringHandler_Bold | 28.48 ns | 0.626 ns | 0.918 ns | 26.51 ns | 30.27 ns | 28.37 ns |  2.07 |    0.07 |    6 | 0.0191 |      80 B |        1.67 |
| StringBuilder_Reserved100          | 30.78 ns | 0.570 ns | 0.533 ns | 30.10 ns | 31.72 ns | 30.87 ns |  2.24 |    0.05 |    7 | 0.0765 |     320 B |        6.67 |
| StringFormat                       | 36.14 ns | 0.782 ns | 1.017 ns | 33.86 ns | 38.05 ns | 36.32 ns |  2.62 |    0.09 |    8 | 0.0114 |      48 B |        1.00 |

# Analysis

The benchmark results demonstrate that the custom `BoldMarkdownV2_Apply` implementation achieves the fastest mean execution time (13.19 ns) for applying bold styling in MarkdownV2 format. However, the performance difference compared to native string operations is marginal.

## Top Performers (Rank 1)
The top four methods—`BoldMarkdownV2_Apply`, `StringConcat`, `InterpolatedString`, and `Concatenation`—all perform within 0.68 ns of each other (13.19-13.87 ns) and share identical memory allocation (48 B). The performance differences are within the margin of error, making them essentially equivalent in real-world scenarios.

## Key Observations

- **Custom Implementation Advantage**: While `BoldMarkdownV2_Apply` is technically the fastest, the ~4% difference is negligible for most applications
- **Memory Efficiency**: All simple string operations allocate the same 48 B, making memory usage identical across the fastest approaches
- **Abstraction Overhead**: Lambda-based abstractions (`MessageTextStyleLambda_Bold`) add ~47% overhead (6.2 ns) and 67% more allocations (80 B)
- **StringBuilder Penalty**: Even with precise sizing, `StringBuilder` is 74% slower and allocates 3x more memory—unsuitable for simple operations
- **String.Format Cost**: The slowest approach at 36.14 ns (174% slower), despite identical memory allocation

**Recommendation:**

For applying bold MarkdownV2 styling, the custom `BoldMarkdownV2.Apply` method offers marginally better performance while providing type safety and semantic clarity. However, if you prefer standard C# idioms, native string concatenation, interpolation, or `String.Concat` deliver virtually identical performance. The choice should prioritize code maintainability and readability over the sub-nanosecond performance differences.

Avoid `StringBuilder`, `String.Format`, and abstraction layers for this simple task—they introduce unnecessary overhead without meaningful benefits.
