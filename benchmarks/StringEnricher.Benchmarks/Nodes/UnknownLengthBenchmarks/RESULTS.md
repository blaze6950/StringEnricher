# Unknown Length String Building Benchmarks

This benchmark suite measures approaches to building a fully styled string (each word wrapped in `*__word__*`) when the final length is **not known upfront**. In this scenario, either the length must be computed explicitly (manual / MessageBuilder paths) or implicitly tracked during building (AutoMessageBuilder).

## Environment
BenchmarkDotNet v0.15.2  
Windows 10 (10.0.19045.6456 / 22H2 / 2022Update)  
Intel Core i7-1065G7 CPU 1.30GHz (Max: 1.50GHz), 1 CPU, 8 logical / 4 physical cores  
.NET SDK 9.0.101  
Runtime: .NET 9.0.0 (9.0.24.52809), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

## Scenario Definition
Input: long lorem-like text split into words. Each word is transformed into:  
`word` → `*__word__*` (with an intervening space between formatted words except for the last).  
Final output is one composite styled string.

Here, the time includes:
1. (For certain methods) Explicit length pre-calculation loop.
2. Construction of the final string.
3. Any internal dynamic growth (when capacity not specified upfront).

## Results
```
| Method                               | Mean     | Error     | StdDev    | Min      | Max      | Median   | Ratio | RatioSD | Rank | Gen0   | Gen1   | Allocated | Alloc Ratio |
|------------------------------------- |---------:|----------:|----------:|---------:|---------:|---------:|------:|--------:|-----:|-------:|-------:|----------:|------------:|
| StringBuilder_WithCalculatedCapacity | 4.597 μs | 0.0907 μs | 0.1384 μs | 4.345 μs | 4.918 μs | 4.564 μs |  0.78 |    0.07 |    1 | 5.6000 |      - |  22.88 KB |        0.81 |
| StringCreate_WithCalculatedLength    | 5.807 μs | 0.0892 μs | 0.1363 μs | 5.655 μs | 6.268 μs | 5.778 μs |  0.99 |    0.08 |    2 | 2.7847 |      - |  11.41 KB |        0.41 |
| StringBuilder_WithoutCapacity        | 5.909 μs | 0.1997 μs | 0.5226 μs | 5.001 μs | 7.975 μs | 5.847 μs |  1.01 |    0.12 |    2 | 6.8817 | 0.0153 |  28.12 KB |        1.00 |
| AutoMessageBuilder_NonStatic         | 7.268 μs | 0.1163 μs | 0.1031 μs | 7.147 μs | 7.492 μs | 7.236 μs |  1.24 |    0.10 |    3 | 2.7847 |      - |  11.41 KB |        0.41 |
| AutoMessageBuilder_Simple            | 7.290 μs | 0.1350 μs | 0.1263 μs | 7.113 μs | 7.553 μs | 7.278 μs |  1.24 |    0.10 |    3 | 2.7847 |      - |  11.41 KB |        0.41 |
| MessageBuilder_WithCalculatedLength  | 7.936 μs | 0.1550 μs | 0.2122 μs | 7.687 μs | 8.576 μs | 7.887 μs |  1.35 |    0.12 |    4 | 2.7771 |      - |  11.41 KB |        0.41 |
```
Baseline (ratio≈1.00) is `StringBuilder_WithoutCapacity` per the `[Benchmark(Baseline = true)]` attribute. Minor deviation (1.01) reflects measurement noise.

## Quick Summary
- Fastest overall: `StringBuilder_WithCalculatedCapacity` (4.60 μs) – benefits from manual pre-computed capacity despite extra length loop.
- Best memory efficiency: All node-based / builder abstractions allocating 11.41 KB (≈59% reduction vs baseline memory usage of 28.12 KB).
- Highest variance: `StringBuilder_WithoutCapacity` (StdDev 0.52 μs, wide Min/Max spread) due to internal buffer growth + GC interaction.
- AutoMessageBuilder static vs non-static: Practically identical (difference well within noise).

## Detailed Analysis
### 1. Baseline: StringBuilder_WithoutCapacity
- Simplicity: No length pre-calculation; relies on internal growth.
- Cost: Highest allocation (28.12 KB) and second-slowest among fast methods.
- Variability: Wide performance spread (5.0–8.0 μs) indicates occasional buffer expand or GC side-effects.

### 2. StringBuilder_WithCalculatedCapacity
- Pattern: Explicit length pre-pass + single build pass.
- Despite doing "more work" (two loops), total time is the fastest because avoids internal resizing overhead.
- Allocations reduced by ~18% compared to baseline (22.88 KB vs 28.12 KB) but still ~2× higher than node-based approaches.

### 3. StringCreate_WithCalculatedLength
- Uses `string.Create` for a single final allocation and direct span writes.
- Includes explicit length calculation pass like the previous method.
- Time: 5.81 μs (close to baseline) while dropping memory to 11.41 KB (≈59% savings).
- Excellent balance when explicit length calculation is acceptable.

### 4. AutoMessageBuilder (Static & NonStatic)
- Eliminates manual length pass; internal logic infers required size incrementally.
- Performance: ~7.28 μs (≈24% slower than baseline) but still only ~1.4–1.7 μs behind `string.Create`.
- Memory: Same efficient 11.41 KB.
- Developer simplicity: Single pass + expressive API – ideal for most real-world usage where microsecond deltas are negligible.
- Non-static lambda shows no meaningful penalty here.

### 5. MessageBuilder_WithCalculatedLength
- Requires explicit length determination before creation (two-stage usage).
- Additional overhead vs raw `string.Create` due to abstraction / callback dispatch (~2.1 μs slower).
- Still enjoys minimal allocation footprint (11.41 KB) and solid absolute performance (<8 μs total).

## Memory Perspective
| Category                    | Baseline (KB) | Pre-Capacity SB (KB) | Node/Built (KB) | Reduction vs Baseline |
|---------------------------- |--------------:|---------------------:|----------------:|----------------------:|
| Allocated per operation     | 28.12         | 22.88                | 11.41           | ~59% (node-based)     |

Reducing per-operation allocation by nearly 60% lowers GC pressure significantly in high-throughput paths.

## Variance & Stability
| Method                          | StdDev (μs) | Notes |
|-------------------------------- |------------:|-------|
| StringBuilder_WithoutCapacity   | 0.5226      | Highest volatility due to dynamic growth.
| MessageBuilder_WithCalculated   | 0.2122      | Moderate; abstraction overhead stable.
| AutoMessageBuilder_*            | ~0.11–0.13  | Stable; predictable performance.
| Capacity / Calculated methods   | ~0.13–0.14  | Consistent.

AutoMessageBuilder offers both memory efficiency and stability without manual length logic.

## Practical Throughput (Approximate)
Assuming ideal scaling and ignoring external factors:
- Fastest method (~4.6 μs): ≈217,000 ops/sec/core.
- AutoMessageBuilder (~7.3 μs): ≈137,000 ops/sec/core.
- Difference: ~80k ops/sec. In most applications, surrounding I/O, parsing, or network costs dwarf this gap.

## When to Choose Each Approach
| Scenario / Priority                          | Recommended Method                    | Rationale |
|--------------------------------------------- |-------------------------------------- |---------- |
| Absolute lowest elapsed time                 | StringBuilder_WithCalculatedCapacity  | Fastest even with extra length pass. |
| Lowest allocations with manual control       | StringCreate_WithCalculatedLength     | Single final allocation, explicit control. |
| Simplest API + low allocations               | AutoMessageBuilder (static lambda)    | One pass, no manual length code, efficient. |
| Need richer composition / future extensible  | MessageBuilder_WithCalculatedLength   | Explicit length + structured builder API. |
| Legacy / incremental building (varied steps) | StringBuilder_WithoutCapacity         | Acceptable if simplicity trumps efficiency. |

## Recommendations
- Default to `AutoMessageBuilder` for maintainable production code unless profiling shows this path is a top hotspot requiring sub-6 μs optimization.
- Use `string.Create` or `MessageBuilder` with calculated length for library primitives or batch pipelines where every microsecond and allocation matters.
- Avoid unbounded `StringBuilder` growth in tight loops; add capacity estimation or migrate to MessageBuilder/AutoMessageBuilder for cleaner API + memory savings.

## Observations on Lambda Choice
The static vs non-static delegate difference for AutoMessageBuilder is statistically insignificant here. Prefer static lambdas when possible (removes closure capture risk), but don't over-optimize if it hurts readability.

## How to Run This Benchmark
```powershell
cd benchmarks/StringEnricher.Benchmarks
# Run only the unknown-length benchmark suite
dotnet run -c Release -- --filter *UnknownLengthBenchmarks*
```

## Future Extensions
- Integrate a variant measuring extremely short inputs (e.g., < 8 words) to show overhead proportions.
- Add a version using pooled buffers to compare against GC-managed allocation savings.
- Explore combining multiple style layers (bold + underline + italic + code) to amplify node composition differences.

## Bottom Line
You pay a small time premium for the convenience of AutoMessageBuilder while enjoying large memory savings versus naive StringBuilder usage. Choose the abstraction that keeps your code clean—only revert to hand-tuned patterns when profiling validates the need.
