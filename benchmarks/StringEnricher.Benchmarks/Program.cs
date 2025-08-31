using BenchmarkDotNet.Running;
using StringEnricher.Benchmarks.Escaping;

namespace StringEnricher.Benchmarks;

public class Program
{
    public static void Main(string[] args)
    {
        // Choose the benchmark to run
        // Uncomment the needed one
        // Comment all others

        //BenchmarkRunner.Run<BoldStyleBenchmarks>();
        //BenchmarkRunner.Run<DoubleBoldStyleBenchmarks>();
        //BenchmarkRunner.Run<TenfoldBoldStyleBenchmarks.TenfoldBoldStyleBenchmarks>();
        //BenchmarkRunner.Run<HardcoreTenfoldBoldStyleBenchmarks>();
        //BenchmarkRunner.Run<MarkdownV2EscapeBenchmarks>();
        BenchmarkRunner.Run<MethodExtractionBenchmarks>();
    }
}