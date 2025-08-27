using BenchmarkDotNet.Running;

namespace StringEnricher.Benchmarks;

public class Program
{
    public static void Main(string[] args)
    {
        // Choose the bench,ark to run
        // Uncomment the needed one
        // Comment all others

        //BenchmarkRunner.Run<BoldStyleBenchmarks>();
        //BenchmarkRunner.Run<DoubleBoldStyleBenchmarks>();
        //BenchmarkRunner.Run<TenfoldBoldStyleBenchmarks.TenfoldBoldStyleBenchmarks>();
        BenchmarkRunner.Run<HardcoreTenfoldBoldStyleBenchmarks.HardcoreTenfoldBoldStyleBenchmarks>();
    }
}