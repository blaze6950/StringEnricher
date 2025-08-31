# Overview
This benchmark compares various methods for dynamically styling a huge string with underline and bold style for every word. (means "source text to process" -> "\*\_\_source\_\_\* \*\_\_text\_\_\* \*\_\_to\_\_\* \*\_\_process\_\_\*").

# Benchmark Results

| Method                                               | Mean       | Error      | StdDev     | Median     | Min        | Max       | Ratio | RatioSD | Rank | Gen0     | Gen1   | Allocated  | Alloc Ratio |
|----------------------------------------------------- |-----------:|-----------:|-----------:|-----------:|-----------:|----------:|------:|--------:|-----:|---------:|-------:|-----------:|------------:|
| BuildStringWithStringBuilderPrecise                  |   7.518 μs |  0.4746 μs |   1.291 μs |   7.192 μs |   5.278 μs |  11.73 μs |  1.03 |    0.24 |    1 |   5.8823 |      - |   24.05 KB |        1.00 |
| BuildStringWithStringBuilder                         |   8.971 μs |  0.5715 μs |   1.640 μs |   8.723 μs |   6.345 μs |  12.91 μs |  1.23 |    0.30 |    2 |   6.8817 | 0.0153 |   28.12 KB |        1.17 |
| BuildStringWithNodesSpanBasedCustomLengthCalculation |   9.232 μs |  0.6251 μs |   1.803 μs |   9.202 μs |   6.059 μs |  13.66 μs |  1.26 |    0.32 |    2 |   2.7771 |      - |   11.41 KB |        0.47 |
| BuildStringWithNodesSpanBased                        |   9.334 μs |  0.6173 μs |   1.771 μs |   8.962 μs |   6.352 μs |  13.97 μs |  1.27 |    0.32 |    2 |   2.7847 |      - |   11.41 KB |        0.47 |
| BuildStringWithNodesMessageBuilderBased              |  12.937 μs |  0.6812 μs |   1.933 μs |  12.729 μs |   9.501 μs |  18.37 μs |  1.77 |    0.38 |    3 |   2.7771 |      - |   11.41 KB |        0.47 |
| BuildStringWithConcatenation                         | 376.112 μs | 38.8544 μs | 114.563 μs | 338.370 μs | 242.153 μs | 650.44 μs | 51.37 |   17.70 |    4 | 641.6016 |      - | 2623.44 KB |      109.06 |

# Analysis

## Performance Summary

The benchmark results reveal significant differences in both execution time and memory allocation across different string building approaches:

### 🏆 **Top Performers (Speed)**
1. **StringBuilder with Precise Capacity** - 7.518 μs (baseline, fastest)
2. **StringBuilder (default)** - 8.971 μs (19% slower than baseline)
3. **Span-based with Custom Length Calculation** - 9.232 μs (23% slower)
4. **Span-based with Node Length** - 9.334 μs (24% slower)

### 💾 **Memory Efficiency Champions**
1. **All Node-based approaches** - 11.41 KB (53% less allocation than baseline)
2. **StringBuilder with Precise Capacity** - 24.05 KB (baseline)
3. **StringBuilder (default)** - 28.12 KB (17% more than baseline)

### 🎯 **Best Library API Experience**
- **MessageBuilder** - 12.937 μs with excellent memory efficiency (11.41 KB) and intuitive, composable API aligned with library design

### ⚠️ **Performance Anti-Pattern**
- **String Concatenation** - 376.112 μs and 2623.44 KB (51x slower, 109x more allocations)

## Key Insights

### 1. **StringBuilder Dominance in Raw Speed**
The traditional StringBuilder approach, especially with precise capacity pre-calculation, remains the fastest method. Pre-calculating the exact capacity provides a meaningful ~19% performance improvement over the default StringBuilder approach by eliminating internal buffer resizing.

### 2. **Node-based Approaches Excel in Memory Efficiency**
All three node-based approaches (`string.Create` with spans) achieve remarkable memory efficiency - using only 47% of the memory compared to StringBuilder. This is achieved through:
- **Zero Intermediate Allocations**: Nodes represent styled segments without creating intermediate strings.
- **Single Final Allocation**: The final string is created in one go, minimizing heap usage
- **Span Utilization**: Leveraging spans allows for efficient memory access and manipulation.
- **Custom Length Calculation**: Pre-calculating the total length of the final string further optimizes memory allocation.
- **Trade-off**: These methods are slightly slower (23-24% slower than StringBuilder with precise capacity) but offer significant memory savings, making them ideal for scenarios where memory usage is a concern.
- **MessageBuilder**: While slightly slower (39% slower than StringBuilder with precise capacity), it offers a great balance of performance, memory efficiency, and developer experience with its fluent API.
  - 39% looks like a big number, but in absolute terms, it's just ~5.4 μs slower than the fastest method, which is negligible in many real-world applications. 1ms = ~5.4 μs is a 185x difference.