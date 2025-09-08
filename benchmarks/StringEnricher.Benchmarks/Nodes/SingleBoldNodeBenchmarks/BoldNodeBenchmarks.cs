using System.Text;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using StringEnricher.Benchmarks.AnalogueImplementations;
using StringEnricher.Helpers.MarkdownV2;

namespace StringEnricher.Benchmarks.Nodes.SingleBoldNodeBenchmarks;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[RankColumn]
[MinColumn]
[MaxColumn]
[MeanColumn]
[MedianColumn]
public class BoldNodeBenchmarks
{
    private string textToBold = "bold text";

    public BoldNodeBenchmarks()
    {
        textToBold = "bold text1";
        textToBold = "bold text";
    }

    [Benchmark]
    public string BoldMarkdownV2_Apply()
    {
        return BoldMarkdownV2.Apply(textToBold).ToString();
    }

    [Benchmark]
    public string MessageTextStyleLambda_Bold()
    {
        return MessageTextNodeLambda.Bold.Build(textToBold);
    }

    [Benchmark]
    public string MessageTextStyleStringHandler_Bold()
    {
        return MessageTextNodeStringHandler.Bold.Build(textToBold);
    }

    [Benchmark(Baseline = true)]
    public string InterpolatedString()
    {
        return $"*{textToBold}*";
    }

    [Benchmark]
    public string StringFormat()
    {
        return string.Format("*{0}*", textToBold);
    }

    [Benchmark]
    public string Concatenation()
    {
        return "*" + textToBold + "*";
    }

    [Benchmark]
    public string StringJoin()
    {
        return string.Join("", "*", textToBold, "*");
    }

    [Benchmark]
    public string StringConcat()
    {
        return string.Concat("*", textToBold, "*");
    }

    [Benchmark]
    public string StringBuilder_Default()
    {
        var sb = new StringBuilder();
        sb.Append("*");
        sb.Append(textToBold);
        sb.Append("*");
        return sb.ToString();
    }

    [Benchmark]
    public string StringBuilder_PreciseSize()
    {
        var sb = new StringBuilder(1 + textToBold.Length + 1);
        sb.Append("*");
        sb.Append(textToBold);
        sb.Append("*");
        return sb.ToString();
    }

    [Benchmark]
    public string StringBuilder_Reserved100()
    {
        var sb = new StringBuilder(100);
        sb.Append("*");
        sb.Append(textToBold);
        sb.Append("*");
        return sb.ToString();
    }
}