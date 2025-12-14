# Overview
This benchmark compares various methods for building a string that contains a single bold-styled date-time value. The goal is to evaluate the performance of different string construction techniques, including string interpolation, concatenation, StringBuilder usage, and custom message builders with styling capabilities.
This benchmark differs from the SingleBoldNodeBenchmarks by styling date time value instead of a simple string, which adds complexity due to formatting requirements. The date time is a type that is wrapped implicitly by the primitive DateTimeNode in the StringEnricher library. This allows to avoid redundant heap allocations when formatting the date time value.

# Benchmark Results
| Method                             | Mean     | Error   | StdDev  | Min      | Max      | Median   | Ratio | RatioSD | Rank | Gen0   | Allocated | Alloc Ratio |
|----------------------------------- |---------:|--------:|--------:|---------:|---------:|---------:|------:|--------:|-----:|-------:|----------:|------------:|
| Concatenation                      | 133.8 ns | 2.71 ns | 4.22 ns | 124.9 ns | 141.4 ns | 134.4 ns |  0.96 |    0.04 |    1 | 0.0324 |     136 B |        1.89 |
| InterpolatedString                 | 139.4 ns | 2.73 ns | 2.68 ns | 131.5 ns | 142.4 ns | 140.0 ns |  1.00 |    0.03 |    1 | 0.0172 |      72 B |        1.00 |
| MessageTextStyleLambda_Bold        | 144.0 ns | 2.91 ns | 3.35 ns | 136.0 ns | 148.5 ns | 144.4 ns |  1.03 |    0.03 |    1 | 0.0401 |     168 B |        2.33 |
| StringConcat                       | 150.3 ns | 3.06 ns | 3.40 ns | 140.8 ns | 157.0 ns | 149.7 ns |  1.08 |    0.03 |    1 | 0.0381 |     160 B |        2.22 |
| StringFormat                       | 153.3 ns | 3.03 ns | 4.15 ns | 144.9 ns | 160.8 ns | 154.0 ns |  1.10 |    0.04 |    1 | 0.0229 |      96 B |        1.33 |
| MessageTextStyleStringHandler_Bold | 158.6 ns | 3.20 ns | 5.26 ns | 145.8 ns | 167.4 ns | 158.1 ns |  1.14 |    0.04 |    1 | 0.0401 |     168 B |        2.33 |
| StringBuilder_Reserved100          | 160.9 ns | 3.25 ns | 6.10 ns | 149.3 ns | 175.3 ns | 161.5 ns |  1.15 |    0.05 |    1 | 0.0975 |     408 B |        5.67 |
| StringBuilder_PreciseSize          | 162.1 ns | 3.26 ns | 8.93 ns | 144.7 ns | 186.3 ns | 160.7 ns |  1.16 |    0.07 |    1 | 0.0610 |     256 B |        3.56 |
| StringJoin                         | 165.3 ns | 3.35 ns | 6.21 ns | 150.3 ns | 182.1 ns | 164.5 ns |  1.19 |    0.05 |    1 | 0.0381 |     160 B |        2.22 |
| StringBuilder_Default              | 176.5 ns | 3.34 ns | 6.59 ns | 163.7 ns | 189.4 ns | 176.8 ns |  1.27 |    0.05 |    2 | 0.0823 |     344 B |        4.78 |
| BoldMarkdownV2_Apply               | 264.7 ns | 5.20 ns | 9.24 ns | 247.9 ns | 281.6 ns | 266.5 ns |  1.90 |    0.08 |    3 | 0.0172 |      72 B |        1.00 |

# Analysis

## Performance Overview

The latest run shifts the performance picture compared to the earlier results: overall timings are lower across the board and the relative ordering at the top changes slightly.

### Top Performers (~133–165 ns)
- **Concatenation**: 133.8 ns (fastest in this run), 136 B allocated — slightly faster than interpolated strings in this environment
- **InterpolatedString** (baseline): 139.4 ns, 72 B allocated
- **MessageTextStyleLambda_Bold**: 144.0 ns, 168 B allocated
- **StringConcat, StringFormat, MessageTextStyleStringHandler_Bold, StringBuilder (reserved/precise), StringJoin**: 150–165 ns range, with allocations between 96 B and 408 B depending on approach

### Lower Performers (~176–265 ns)
- **StringBuilder_Default**: 176.5 ns, 344 B allocated
- **BoldMarkdownV2_Apply** (StringEnricher): 264.7 ns, 72 B allocated — still the slowest measured method in this benchmark but with the same low allocation as the interpolated baseline

## The StringEnricher Approach: BoldMarkdownV2_Apply

`BoldMarkdownV2_Apply` remains notably more expensive in execution time compared to the baseline interpolated string (264.7 ns vs 139.4 ns). The measured overhead in this run is ~125 ns (roughly a 1.90× ratio), which matches the table's Ratio column (1.90).

However, the crucial trade-offs remain unchanged and are worth repeating with the updated numbers.

### 1. Memory Efficiency (unchanged)
`BoldMarkdownV2_Apply` allocates 72 B — identical to the `InterpolatedString` baseline and significantly lower than many alternatives (for example, many MessageTextStyle and StringBuilder approaches allocate 96–408 B). This low allocation is achieved through value-type nodes and span-based, zero-allocation string creation.

### 2. Type Safety and API Guarantees
StringEnricher continues to offer a type-safe, composable API (`BoldMarkdownV2.Apply(dateTime)` returning a typed node) and implicit handling of DateTime values (so you avoid manual `.ToString()` formatting everywhere).

### 3. Built-in Formatting Support
The library provides overloads for custom format strings and format providers, which reduces boilerplate and the chance of inconsistent date formatting across the codebase.

### 4. Future-Proof, Span-based Implementation
`DateTimeNode` uses `TryFormat`, pre-calculated lengths, and direct span copying — enabling zero-allocation construction and allowing future runtime improvements to benefit the library automatically.

### 5. Behavior in Complex Scenarios
This benchmark is a simple single-node case. The fixed overhead (now ~125 ns per styled DateTime node) becomes relatively smaller as message complexity grows (multiple styled nodes, nested composition, or mixed content). In multi-node scenarios, the memory efficiency and composability of StringEnricher often yield a better overall trade-off.

## Performance in Context

1. Absolute values are small: 264.7 ns is ~0.26 µs. For user-facing applications (bots, UIs) this is negligible compared to network latency and I/O.
2. Relative cost: the ~125 ns overhead buys you type safety, formatting support, and zero-allocation composition.
3. Break-even: messages with multiple styled parts (3–5+) will amortize the per-node overhead, and the minimal allocations reduce GC pressure under load.

## When to Choose Each Approach

- Use raw interpolation/concatenation when you need the absolute lowest latency for very small, trivial messages and have no formatting or composition needs.
- Use StringEnricher (`BoldMarkdownV2.Apply`) when you want type safety, consistent formatting, composability, and minimal allocations — especially valuable as message complexity increases or when allocation pressure matters.

## Conclusion

The new run keeps the core conclusion: `BoldMarkdownV2_Apply` trades some CPU time (~125 ns extra vs interpolated) for markedly better memory behavior (same minimal allocation as the baseline) and for a safer, extensible API. Depending on your workload (number of styled nodes per message, allocation sensitivity, need for formatting/culture support), StringEnricher remains a strong choice despite the per-node CPU overhead measured here.