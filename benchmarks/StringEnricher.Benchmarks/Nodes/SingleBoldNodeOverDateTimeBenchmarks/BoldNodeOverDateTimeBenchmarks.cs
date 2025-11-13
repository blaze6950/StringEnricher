using System.Text;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using StringEnricher.Benchmarks.AnalogueImplementations;
using StringEnricher.Telegram.Helpers.MarkdownV2;

namespace StringEnricher.Benchmarks.Nodes.SingleBoldNodeOverDateTimeBenchmarks;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[RankColumn]
[MinColumn]
[MaxColumn]
[MeanColumn]
[MedianColumn]
public class BoldNodeOverDateTimeBenchmarks
{
    private DateTime dateTimeToBold;

    public BoldNodeOverDateTimeBenchmarks()
    {
        dateTimeToBold = DateTime.MinValue;
        dateTimeToBold = DateTime.Parse("11/12/2025 1:52:41 PM");
    }

    [Benchmark]
    public string BoldMarkdownV2_Apply()
    {
        return BoldMarkdownV2.Apply(dateTimeToBold).ToString();
    }

    [Benchmark]
    public string MessageTextStyleLambda_Bold()
    {
        return MessageTextNodeLambda.Bold.Build(dateTimeToBold.ToString());
    }

    [Benchmark]
    public string MessageTextStyleStringHandler_Bold()
    {
        return MessageTextNodeStringHandler.Bold.Build(dateTimeToBold.ToString());
    }

    [Benchmark(Baseline = true)]
    public string InterpolatedString()
    {
        return $"*{dateTimeToBold}*";
    }

    [Benchmark]
    public string StringFormat()
    {
        return string.Format("*{0}*", dateTimeToBold);
    }

    [Benchmark]
    public string Concatenation()
    {
        return "*" + dateTimeToBold + "*";
    }

    [Benchmark]
    public string StringJoin()
    {
        return string.Join("", "*", dateTimeToBold, "*");
    }

    [Benchmark]
    public string StringConcat()
    {
        return string.Concat("*", dateTimeToBold, "*");
    }

    [Benchmark]
    public string StringBuilder_Default()
    {
        var sb = new StringBuilder();
        sb.Append("*");
        sb.Append(dateTimeToBold);
        sb.Append("*");
        return sb.ToString();
    }

    [Benchmark]
    public string StringBuilder_PreciseSize()
    {
        var sb = new StringBuilder(1 + 21 + 1); // manually calculated size
        sb.Append("*");
        sb.Append(dateTimeToBold);
        sb.Append("*");
        return sb.ToString();
    }

    [Benchmark]
    public string StringBuilder_Reserved100()
    {
        var sb = new StringBuilder(100);
        sb.Append("*");
        sb.Append(dateTimeToBold);
        sb.Append("*");
        return sb.ToString();
    }
}