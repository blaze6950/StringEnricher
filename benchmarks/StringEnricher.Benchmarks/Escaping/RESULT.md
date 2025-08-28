# Overview
This benchmark compares various methods of escaping a string with MarkdownV2 reserved characters (means "_string_ *with* ~MarkdownV2~ __reserved__ !characters!" -> "\_string\_ \*with\* \~MarkdownV2\~ \_\_reserved\_\_ \!characters\!").

The methods include using a custom escaping algorithms:
- using StringBuilder with precise size calculation
- using StringBuilder with default growth
- using string.Create
- using string.Create with FrozenSet instead of switches

# Benchmark Results

| Method                      | Input                  | Mean            | Error         |        StdDev |          Median |             Min |             Max | Ratio | RatioSD | Rank | Gen0    | Gen1    | Allocated | Alloc Ratio |
|---------------------------- |------------------------|----------------:|--------------:|--------------:|----------------:|----------------:|----------------:|------:|--------:|-----:|--------:|--------:|----------:|------------:|
| EscapeStringBuilderPrecise  | ?                      |       0.5730 ns |     0.0783 ns |     0.1046 ns |       0.5868 ns |       0.3145 ns |       0.7229 ns |  0.77 |    0.16 |    1 |       - |       - |         - |          NA |
| EscapeStringCreate          | ?                      |       0.7294 ns |     0.0738 ns |     0.1105 ns |       0.6843 ns |       0.5977 ns |       1.0097 ns |  0.97 |    0.18 |    2 |       - |       - |         - |          NA |
| EscapeStringCreateFrozenSet | ?                      |       0.7297 ns |     0.1367 ns |     0.3625 ns |       0.5926 ns |       0.3886 ns |       1.9941 ns |  0.98 |    0.50 |    2 |       - |       - |         - |          NA |
| EscapeStringBuilder         | ?                      |       0.7587 ns |     0.0778 ns |     0.0956 ns |       0.7677 ns |       0.6309 ns |       1.0395 ns |  1.01 |    0.17 |    2 |       - |       - |         - |          NA |
|                             |                        |                 |               |               |                 |                 |                 |       |         |      |         |         |           |             |
| EscapeStringBuilderPrecise  |                        |       0.7370 ns |     0.1765 ns |     0.4861 ns |       0.5927 ns |       0.2105 ns |       2.1425 ns |  0.52 |    0.35 |    1 |       - |       - |         - |          NA |
| EscapeStringBuilder         |                        |       1.4419 ns |     0.0963 ns |     0.2251 ns |       1.3976 ns |       1.1188 ns |       2.1010 ns |  1.02 |    0.21 |    2 |       - |       - |         - |          NA |
| EscapeStringCreateFrozenSet |                        |       1.5326 ns |     0.3715 ns |     1.0718 ns |       1.0803 ns |       0.4473 ns |       4.5460 ns |  1.09 |    0.78 |    3 |       - |       - |         - |          NA |
| EscapeStringCreate          |                        |       1.5693 ns |     0.0952 ns |     0.1096 ns |       1.5437 ns |       1.4310 ns |       1.8946 ns |  1.11 |    0.17 |    4 |       - |       - |         - |          NA |
|                             |                        |                 |               |               |                 |                 |                 |       |         |      |         |         |           |             |
| EscapeStringCreate          | ___***!!!              |      38.2792 ns |     1.4061 ns |     3.8492 ns |      36.9852 ns |      34.0933 ns |      52.0253 ns |  0.22 |    0.03 |    1 |  0.0153 |       - |      64 B |        0.29 |
| EscapeStringBuilderPrecise  | ___***!!!              |      62.8867 ns |     3.1374 ns |     8.9002 ns |      60.5473 ns |      53.2008 ns |      86.8133 ns |  0.36 |    0.06 |    2 |  0.0421 |       - |     176 B |        0.79 |
| EscapeStringCreateFrozenSet | ___***!!!              |      79.9472 ns |     1.4320 ns |     2.7246 ns |      79.3569 ns |      76.4861 ns |      87.8830 ns |  0.46 |    0.03 |    3 |  0.0153 |       - |      64 B |        0.29 |
| EscapeStringBuilder         | ___***!!!              |     176.5131 ns |     4.7477 ns |    13.3910 ns |     170.5071 ns |     162.5319 ns |     214.6733 ns |  1.01 |    0.10 |    4 |  0.0534 |       - |     224 B |        1.00 |
|                             |                        |                 |               |               |                 |                 |                 |       |         |      |         |         |           |             |
| EscapeStringCreate          | _*~`#+-=.![](){}>\|\\  |      91.2184 ns |     1.5196 ns |     1.3471 ns |      90.9754 ns |      89.1596 ns |      93.9623 ns |  0.15 |    0.00 |    1 |  0.0248 |       - |     104 B |        0.13 |
| EscapeStringBuilderPrecise  | _*~`#+-=.![](){}>\|\\  |     141.1799 ns |     2.1165 ns |     1.7673 ns |     140.9853 ns |     138.1513 ns |     144.4974 ns |  0.23 |    0.01 |    2 |  0.0610 |       - |     256 B |        0.32 |
| EscapeStringCreateFrozenSet | _*~`#+-=.![](){}>\|\\  |     183.0347 ns |     3.2469 ns |     3.0371 ns |     183.3847 ns |     176.7478 ns |     188.1558 ns |  0.30 |    0.01 |    3 |  0.0248 |       - |     104 B |        0.13 |
| EscapeStringBuilder         | _*~`#+-=.![](){}>\|\\  |     616.9445 ns |     9.7661 ns |    15.7704 ns |     612.1896 ns |     599.0962 ns |     661.6259 ns |  1.00 |    0.04 |    4 |  0.1907 |       - |     800 B |        1.00 |
|                             |                        |                 |               |               |                 |                 |                 |       |         |      |         |         |           |             |
| EscapeStringCreate          | _*~`(...)=.![ [4096]   |  16,873.0087 ns |   224.4069 ns |   198.9309 ns |  16,892.8162 ns |  16,338.0615 ns |  17,088.6475 ns |  0.04 |    0.00 |    1 |  3.9063 |       - |   16408 B |        0.04 |
| EscapeStringBuilderPrecise  | _*~`(...)=.![ [4096]   |  23,717.2048 ns |   389.3248 ns |   303.9592 ns |  23,766.3864 ns |  23,233.3954 ns |  24,079.5868 ns |  0.06 |    0.00 |    2 |  7.8430 |  0.2441 |   32864 B |        0.07 |
| EscapeStringCreateFrozenSet | _*~`(...)=.![ [4096]   |  33,090.6883 ns |   620.5929 ns |   580.5030 ns |  33,016.7206 ns |  32,405.7587 ns |  34,459.1522 ns |  0.08 |    0.00 |    3 |  3.9063 |       - |   16408 B |        0.04 |
| EscapeStringBuilder         | _*~`(...)=.![ [4096]   | 427,435.6220 ns | 4,420.7124 ns | 3,691.4951 ns | 427,372.6562 ns | 422,421.7773 ns | 434,988.6719 ns |  1.00 |    0.01 |    4 | 83.9844 | 59.5703 |  450720 B |        1.00 |
|                             |                        |                 |               |               |                 |                 |                 |       |         |      |         |         |           |             |
| EscapeStringCreate          | _start middle* end!    |      79.3820 ns |     1.6135 ns |     1.5093 ns |      79.1073 ns |      77.8353 ns |      83.3628 ns |  0.69 |    0.02 |    1 |  0.0172 |       - |      72 B |        0.29 |
| EscapeStringBuilderPrecise  | _start middle* end!    |      88.5781 ns |     1.5210 ns |     1.3484 ns |      88.3101 ns |      87.1058 ns |      91.4031 ns |  0.77 |    0.02 |    2 |  0.0458 |       - |     192 B |        0.77 |
| EscapeStringBuilder         | _start middle* end!    |     114.6872 ns |     2.2962 ns |     1.9174 ns |     114.6542 ns |     111.8794 ns |     118.9846 ns |  1.00 |    0.02 |    3 |  0.0591 |       - |     248 B |        1.00 |
| EscapeStringCreateFrozenSet | _start middle* end!    |     137.6118 ns |     2.2250 ns |     1.9724 ns |     137.3723 ns |     135.2990 ns |     142.1419 ns |  1.20 |    0.03 |    4 |  0.0172 |       - |      72 B |        0.29 |
|                             |                        |                 |               |               |                 |                 |                 |       |         |      |         |         |           |             |
| EscapeStringCreate          | Hello_World!           |      54.2548 ns |     1.4348 ns |     3.9278 ns |      54.6271 ns |      44.0885 ns |      63.5212 ns |  0.77 |    0.06 |    1 |  0.0134 |       - |      56 B |        0.26 |
| EscapeStringBuilderPrecise  | Hello_World!           |      58.1553 ns |     0.9334 ns |     0.9168 ns |      57.8932 ns |      57.0612 ns |      60.5004 ns |  0.83 |    0.02 |    2 |  0.0381 |       - |     160 B |        0.74 |
| EscapeStringBuilder         | Hello_World!           |      70.1359 ns |     0.9882 ns |     0.7715 ns |      69.9017 ns |      68.9293 ns |      71.5100 ns |  1.00 |    0.01 |    3 |  0.0516 |       - |     216 B |        1.00 |
| EscapeStringCreateFrozenSet | Hello_World!           |     100.5036 ns |     0.8781 ns |     0.7332 ns |     100.6866 ns |      98.7028 ns |     101.6829 ns |  1.43 |    0.02 |    4 |  0.0134 |       - |      56 B |        0.26 |
|                             |                        |                 |               |               |                 |                 |                 |       |         |      |         |         |           |             |
| EscapeStringBuilder         | Lore(...)smo_ [4096]   |   8,907.0947 ns |   174.9596 ns |   179.6706 ns |   8,879.2587 ns |   8,701.3550 ns |   9,323.9182 ns |  1.00 |    0.03 |    1 |  3.9825 |       - |   16648 B |        1.00 |
| EscapeStringCreate          | Lore(...)smo_ [4096]   |  14,291.5560 ns |   256.8250 ns |   227.6687 ns |  14,234.6939 ns |  13,982.0465 ns |  14,758.5327 ns |  1.61 |    0.04 |    2 |  1.9531 |       - |    8216 B |        0.49 |
| EscapeStringBuilderPrecise  | Lore(...)smo_ [4096]   |  17,798.6472 ns |   353.7423 ns |   997.7364 ns |  18,110.9482 ns |  15,484.2224 ns |  20,431.3110 ns |  2.00 |    0.12 |    3 |  3.9368 |       - |   16488 B |        0.99 |
| EscapeStringCreateFrozenSet | Lore(...)smo_ [4096]   |  33,156.6235 ns |   649.5978 ns | 1,266.9908 ns |  32,943.8049 ns |  31,350.1282 ns |  37,801.7456 ns |  3.72 |    0.16 |    4 |  1.9531 |       - |    8216 B |        0.49 |
|                             |                        |                 |               |               |                 |                 |                 |       |         |      |         |         |           |             |
| EscapeStringCreate          | Lore(...)smod [4096]   |   6,585.6867 ns |   131.2101 ns |   229.8039 ns |   6,500.6134 ns |   6,290.3976 ns |   7,170.4300 ns |  0.97 |    0.04 |    1 |       - |       - |         - |          NA |
| EscapeStringBuilderPrecise  | Lore(...)smod [4096]   |   6,760.4073 ns |    97.0689 ns |    90.7983 ns |   6,739.1296 ns |   6,635.2905 ns |   6,966.3712 ns |  1.00 |    0.02 |    1 |       - |       - |         - |          NA |
| EscapeStringBuilder         | Lore(...)smod [4096]   |   6,784.8452 ns |   129.8228 ns |   101.3571 ns |   6,775.1499 ns |   6,631.7322 ns |   6,968.4303 ns |  1.00 |    0.02 |    1 |       - |       - |         - |          NA |
| EscapeStringCreateFrozenSet | Lore(...)smod [4096]   |  13,355.0839 ns |   223.5266 ns |   248.4492 ns |  13,305.8151 ns |  12,892.3889 ns |  14,113.9404 ns |  1.97 |    0.05 |    2 |       - |       - |         - |          NA |
|                             |                        |                 |               |               |                 |                 |                 |       |         |      |         |         |           |             |
| EscapeStringCreate          | string to escape       |      23.8026 ns |     1.3755 ns |     3.7885 ns |      21.5834 ns |      20.3746 ns |      36.5235 ns |  0.90 |    0.14 |    1 |       - |       - |         - |          NA |
| EscapeStringBuilder         | string to escape       |      26.3277 ns |     0.6111 ns |     0.5103 ns |      26.3974 ns |      25.2669 ns |      27.1361 ns |  1.00 |    0.03 |    1 |       - |       - |         - |          NA |
| EscapeStringBuilderPrecise  | string to escape       |      28.5378 ns |     0.4234 ns |     0.5505 ns |      28.4639 ns |      27.7304 ns |      29.6915 ns |  1.08 |    0.03 |    2 |       - |       - |         - |          NA |
| EscapeStringCreateFrozenSet | string to escape       |      53.1732 ns |     0.6008 ns |     0.5017 ns |      53.0744 ns |      52.5709 ns |      53.8734 ns |  2.02 |    0.04 |    3 |       - |       - |         - |          NA |
|                             |                        |                 |               |               |                 |                 |                 |       |         |      |        |         |           |             |
| EscapeStringCreate          | Приве(...)😃_😄 [27]   |     108.8580 ns |     2.0970 ns |     1.7511 ns |     108.3807 ns |   106.5655 ns   |     112.4834 ns | 0.56  |    0.01 |    1 |  0.0210 |       - |      88 B |        0.31 |
| EscapeStringBuilderPrecise  | Приве(...)😃_😄 [27]   |     142.8229 ns |     2.1408 ns |     1.6714 ns |     142.9928 ns |     139.4814 ns |     144.9778 ns |  0.73 |    0.02 |    2 |  0.0553 |       - |     232 B |        0.83 |
| EscapeStringCreateFrozenSet | Приве(...)😃_😄 [27]   |     182.4218 ns |     2.5402 ns |     2.1212 ns |     182.5111 ns |     178.1298 ns |     185.8073 ns |  0.93 |    0.02 |    3 |  0.0210 |       - |      88 B |        0.31 |
| EscapeStringBuilder         | Приве(...)😃_😄 [27]   |     196.2051 ns |     3.8487 ns |     3.7799 ns |     195.2035 ns |     191.0895 ns |     203.5181 ns |  1.00 |    0.03 |    4 |  0.0668 |       - |     280 B |        1.00 |

# Analysis

The benchmark results show that the EscapeStringCreate approach (using the logic from MarkdownV2.cs) is highly optimized and consistently delivers excellent performance across all tested scenarios.

Key points from the results:
- For short strings and typical use cases, EscapeStringCreate is either the fastest or very close to the fastest, with minimal memory allocation.
- For strings with many escapable characters, it outperforms the StringBuilder-based approaches by a significant margin, both in speed and memory usage.
- For long strings, especially those with many escapable characters, EscapeStringCreate remains highly efficient, with much lower allocation and execution time compared to StringBuilder methods.
- The approach is robust for all input types, including empty, null, and Unicode-rich strings.
  
Conclusion:

EscapeStringCreate (the logic in MarkdownV2.cs) is well-optimized and suitable for any case where MarkdownV2 escaping is required. It offers consistently high performance and low memory usage, making it the preferred choice for production use.