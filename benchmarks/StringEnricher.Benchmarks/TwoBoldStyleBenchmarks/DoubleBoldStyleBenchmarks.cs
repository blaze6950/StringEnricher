using System.Text;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using StringEnricher.Benchmarks.AnalogueImplementations;
using StringEnricher.StringStyles.MarkdownV2;

namespace StringEnricher.Benchmarks.TwoBoldStyleBenchmarks;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[RankColumn]
[MinColumn]
[MaxColumn]
[MeanColumn]
[MedianColumn]
public class DoubleBoldStyleBenchmarks
{
    private string textToBold = "bold text";

    public DoubleBoldStyleBenchmarks()
    {
        textToBold = "bold text1";
        textToBold = "bold text";
    }

    [Benchmark]
    public string BoldMarkdownV2_Apply() => BoldMarkdownV2.Apply(
        BoldMarkdownV2.Apply(textToBold)
    ).ToString();

    [Benchmark]
    public string MessageTextStyleLambda_Bold() => MessageTextStyleLambda.Bold.Build(
        MessageTextStyleLambda.Bold.Build(textToBold)
    );

    [Benchmark]
    public string MessageTextStyleStringHandler_Bold() => MessageTextStyleStringHandler.Bold.Build(
        MessageTextStyleLambda.Bold.Build(textToBold)
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
