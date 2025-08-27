using BenchmarkDotNet.Running;
using StringEnricher.Benchmarks.SingleBoldStyleBenchmarks;

namespace StringEnricher.Benchmarks;

public class Program
{
    public static void Main(string[] args)
    {
        // Choose the bench,ark to run
        // Uncomment the needed one
        // Comment all others

        BenchmarkRunner.Run<BoldStyleBenchmarks>();
    }
}