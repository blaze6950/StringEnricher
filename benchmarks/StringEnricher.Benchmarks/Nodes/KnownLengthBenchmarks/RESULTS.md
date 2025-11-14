# Known Length String Building Benchmarks

This benchmark suite measures different approaches to building a fully styled string (each word wrapped in `*__word__*`), in a scenario where the final length of the output string is already known before construction. This represents the best-case environment for APIs that can exploit pre-allocation (StringBuilder with capacity, MessageBuilder, and direct `string.Create`).

## Environment
BenchmarkDotNet v0.15.2  
Windows 10 (10.0.19045.6456 / 22H2 / 2022Update)  
Intel Core i7-1065G7 CPU 1.30GHz (Max: 1.50GHz), 1 CPU, 8 logical / 4 physical cores  
.NET SDK 9.0.101  
Runtime: .NET 9.0.0 (9.0.24.52809), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

## Benchmark Goal
Transform:  
`source text to process` → `*__source__* *__text__* *__to__* *__process__* ...` for every word in a long input.

Because the final length is known, any length calculation cost is excluded from timing. This spotlights pure writing speed and allocation characteristics.

## Results
```
| Method                                  | Mean     | Error     | StdDev    | Min      | Max      | Median   | Ratio | RatioSD | Rank | Gen0   | Allocated | Alloc Ratio |
|---------------------------------------- |---------:|----------:|----------:|---------:|---------:|---------:|------:|--------:|-----:|-------:|----------:|------------:|
| StringBuilder_WithPreCalculatedCapacity | 4.140 μs | 0.0808 μs | 0.1106 μs | 3.941 μs | 4.358 μs | 4.141 μs |  1.00 |    0.04 |    1 | 5.6000 |  22.88 KB |        1.00 |
| StringCreate_WithNodes                  | 4.934 μs | 0.0887 μs | 0.0692 μs | 4.759 μs | 5.041 μs | 4.951 μs |  1.19 |    0.04 |    2 | 2.7847 |  11.41 KB |        0.50 |
| MessageBuilder_WithPreCalculatedLength  | 7.259 μs | 0.1417 μs | 0.1183 μs | 6.954 μs | 7.418 μs | 7.285 μs |  1.75 |    0.05 |    3 | 2.7847 |  11.41 KB |        0.50 |
```

## Quick Summary
- Fastest: `StringBuilder_WithPreCalculatedCapacity` (baseline) at 4.14 μs.
- Memory winners: Both node-based approaches (`StringCreate_WithNodes` and `MessageBuilder_WithPreCalculatedLength`) allocate 50% of the baseline.
- Trade-off: `string.Create` direct usage offers a strong balance of speed and low allocation without framework overhead.

## Detailed Analysis
### 1. StringBuilder_WithPreCalculatedCapacity (Baseline)
- Uses a single allocation sized exactly to the final output.
- Still incurs per-Append method overhead and formatting logic at runtime.
- Best raw execution speed in this scenario.
- Allocation: 22.88 KB (reference point for allocation ratios).

### 2. StringCreate_WithNodes
- Pre-calculated length + single `string.Create` call writing directly to the target span.
- Eliminates intermediate buffering and repeated capacity checks.
- 19% slower than baseline, but cuts allocations in half (11.41 KB vs 22.88 KB).
- Gen0 collections reflect fewer temporary artifacts; only structured node operations and one final string are realized.
- Excellent choice when you want near-baseline speed but much better memory efficiency.

### 3. MessageBuilder_WithPreCalculatedLength
- Wraps the same underlying concept (`string.Create`) in a higher-level, ergonomic API.
- Additional indirection and callback dispatch add overhead: ~75% slower vs baseline (7.26 μs vs 4.14 μs). The absolute difference is ~3.1 μs (0.0031 ms), which is still extremely small in most application contexts.
- Allocation identical to `StringCreate_WithNodes` (11.41 KB) thanks to single-pass construction and node usage.
- Gains: cleaner code, composability, reduced chances of low-level mistakes.

## Memory Perspective
| Category              | Baseline (KB) | Node-Based (KB) | Reduction |
|-----------------------|---------------|-----------------|-----------|
| Final string + temp   | 22.88         | 11.41           | ~50%      |

A 50% reduction in allocation translates to lower GC pressure in high-throughput or server-side scenarios. If this operation occurs very frequently (e.g. formatting messages for logging, chat rendering, templated notifications), lowering per-call allocation can yield smoother latency under load.

## When to Choose Each Approach
| Scenario                                      | Recommended Method                    | Rationale |
|---------------------------------------------- |-------------------------------------- |---------- |
| Maximum raw micro-performance                 | StringBuilder (capacity pre-known)    | Fastest elapsed time. |
| Best balance of speed + memory optimization   | string.Create with nodes              | Low allocation + still very fast. |
| Developer productivity + maintainability      | MessageBuilder                        | High-level API; performance still strong in absolute terms. |

## Practical Impact of Differences
Even the slowest of these (MessageBuilder) completes in ~7 μs. That means:
- ~140,000 formatted messages per second per core (idealized) vs ~240,000 for baseline.
- In real applications, I/O, networking, and higher-level logic dwarf this difference.

So the choice often hinges less on raw microseconds and more on:
- Code clarity and safety.
- Memory strategy (GC pressure in sustained workloads).
- Reusability and styling composition.

## Recommendations
- Use `MessageBuilder` when you value expressive, reusable formatting code and are not in a hyper-critical performance path.
- Use direct `string.Create` when writing a tight loop or library primitive that must minimize allocations and you can safely manage span logic.
- Reserve manual `StringBuilder` usage for legacy codebases or when incremental mutation patterns are unavoidable.

## How to Run This Benchmark
```powershell
# From the benchmarks project directory
cd benchmarks/StringEnricher.Benchmarks

# Run only the known-length benchmarks
dotnet run -c Release -- --filter *KnownLengthBenchmarks*
```

## Future Extensions
- Add benchmarks including the cost of length calculation (see UnknownLength suite) for end-to-end comparison.
- Compare against an unsafe stackalloc approach for very small inputs.
- Add scenarios with mixed styles (bold + italic + code) to assess node combination overhead.

## Bottom Line
`string.Create` + nodes gives you most of the raw performance benefits with half the memory of a tuned `StringBuilder`, while `MessageBuilder` trades a few microseconds for a cleaner abstraction. All three are valid; pick based on maintainability and throughput needs rather than chasing sub-microsecond differences.
