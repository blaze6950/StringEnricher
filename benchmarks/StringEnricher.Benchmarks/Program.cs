using BenchmarkDotNet.Running;
using StringEnricher.Benchmarks.Nodes.CompositeNodeBenchmarks;

namespace StringEnricher.Benchmarks;

public class Program
{
    public static void Main(string[] args)
    {
        // Choose the benchmark to run
        // Uncomment the needed one
        // Comment all others

        //BenchmarkRunner.Run<BoldNodeBenchmarks>();
        //BenchmarkRunner.Run<DoubleBoldNodeBenchmarks>();
        //BenchmarkRunner.Run<TenfoldBoldNodeBenchmarks.TenfoldBoldNodeBenchmarks>();
        //BenchmarkRunner.Run<HardcoreTenfoldBoldNodeBenchmarks>();
        //BenchmarkRunner.Run<MarkdownV2EscapeBenchmarks>();
        //BenchmarkRunner.Run<MethodExtractionBenchmarks>();
        BenchmarkRunner.Run<CompositeNodeBenchmarks>();
    }
}