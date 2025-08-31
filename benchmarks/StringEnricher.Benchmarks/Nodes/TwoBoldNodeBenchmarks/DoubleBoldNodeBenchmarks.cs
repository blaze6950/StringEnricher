using System.Text;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using StringEnricher.Benchmarks.AnalogueImplementations;
using StringEnricher.Nodes.MarkdownV2;
using StringEnricher.Nodes.MarkdownV2.Formatting;

namespace StringEnricher.Benchmarks.Nodes.TwoBoldNodeBenchmarks;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[RankColumn]
[MinColumn]
[MaxColumn]
[MeanColumn]
[MedianColumn]
public class DoubleBoldNodeBenchmarks
{
    private string textToBold = "bold text";

    public DoubleBoldNodeBenchmarks()
    {
        textToBold = "bold text1";
        textToBold = "bold text";
    }

    [Benchmark]
    public string BoldMarkdownV2_Apply() => BoldMarkdownV2.Apply(
        BoldMarkdownV2.Apply(textToBold)
    ).ToString();

    [Benchmark]
    public string MessageTextStyleLambda_Bold() => MessageTextNodeLambda.Bold.Build(
        MessageTextNodeLambda.Bold.Build(textToBold)
    );

    [Benchmark]
    public string MessageTextStyleStringHandler_Bold() => MessageTextNodeStringHandler.Bold.Build(
        MessageTextNodeLambda.Bold.Build(textToBold)
    );

    [Benchmark(Baseline = true)]
    public string InterpolatedString()
    {
        var onceBoldedText = $"*{textToBold}*";
        return $"*{onceBoldedText}*";
    }

    [Benchmark]
    public string StringFormat()
    {
        var onceBoldedText = string.Format("*{0}*", textToBold);
        return string.Format("*{0}*", onceBoldedText);
    }

    [Benchmark]
    public string Concatenation()
    {
        var onceBoldedText = "*" + textToBold + "*";
        return "*" + onceBoldedText + "*";
    }

    [Benchmark]
    public string StringJoin()
    {
        var onceBoldedText = string.Join("", "*", textToBold, "*");
        return string.Join("", "*", onceBoldedText, "*");
    }

    [Benchmark]
    public string StringConcat()
    {
        var onceBoldedText = string.Concat("*", textToBold, "*");
        return string.Concat("*", onceBoldedText, "*");
    }

    [Benchmark]
    public string StringBuilder_Default()
    {
        var sb = new StringBuilder();
        sb.Append("*");
        sb.Append("*");
        sb.Append(textToBold);
        sb.Append("*");
        sb.Append("*");
        return sb.ToString();
    }

    [Benchmark]
    public string StringBuilder_PreciseSize()
    {
        var sb = new StringBuilder(1 + 1 + textToBold.Length + 1 + 1);
        sb.Append("*");
        sb.Append("*");
        sb.Append(textToBold);
        sb.Append("*");
        sb.Append("*");
        return sb.ToString();
    }

    [Benchmark]
    public string StringBuilder_Reserved100()
    {
        var sb = new StringBuilder(100);
        sb.Append("*");
        sb.Append("*");
        sb.Append(textToBold);
        sb.Append("*");
        sb.Append("*");
        return sb.ToString();
    }
}
