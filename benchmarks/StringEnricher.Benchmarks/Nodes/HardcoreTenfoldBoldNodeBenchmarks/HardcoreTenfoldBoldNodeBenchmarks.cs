using System.Text;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using StringEnricher.Benchmarks.AnalogueImplementations;
using StringEnricher.Nodes.MarkdownV2.Formatting;

namespace StringEnricher.Benchmarks.Nodes.HardcoreTenfoldBoldNodeBenchmarks;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[RankColumn]
[MinColumn]
[MaxColumn]
[MeanColumn]
[MedianColumn]
public class HardcoreTenfoldBoldNodeBenchmarks
{
    private string textToBold = "bold text";

    public HardcoreTenfoldBoldNodeBenchmarks()
    {
        textToBold = "bold text1";
        textToBold = "bold text";
    }

    [Benchmark]
    public string BoldMarkdownV2_Apply() =>
        BoldMarkdownV2.Apply( // 10
            BoldMarkdownV2.Apply( // 9
                BoldMarkdownV2.Apply( // 8
                    BoldMarkdownV2.Apply( // 7
                        BoldMarkdownV2.Apply( // 6
                            BoldMarkdownV2.Apply( // 5
                                BoldMarkdownV2.Apply( // 4
                                    BoldMarkdownV2.Apply( // 3
                                        BoldMarkdownV2.Apply( // 2
                                            BoldMarkdownV2.Apply(textToBold) // 1
                                        )
                                    )
                                )
                            )
                        )
                    )
                )
            )
        ).ToString();

    [Benchmark]
    public string MessageTextStyleLambda_Bold()
    {
        var first = MessageTextNodeLambda.Bold.Build(textToBold);
        var second = MessageTextNodeLambda.Bold.Build(first);
        var third = MessageTextNodeLambda.Bold.Build(second);
        var fourth = MessageTextNodeLambda.Bold.Build(third);
        var fifth = MessageTextNodeLambda.Bold.Build(fourth);
        var sixth = MessageTextNodeLambda.Bold.Build(fifth);
        var seventh = MessageTextNodeLambda.Bold.Build(sixth);
        var eighth = MessageTextNodeLambda.Bold.Build(seventh);
        var ninth = MessageTextNodeLambda.Bold.Build(eighth);
        var tenth = MessageTextNodeLambda.Bold.Build(ninth);
        return tenth;
    }

    [Benchmark]
    public string MessageTextStyleStringHandler_Bold()
    {
        var first = MessageTextNodeStringHandler.Bold.Build(textToBold);
        var second = MessageTextNodeStringHandler.Bold.Build(first);
        var third = MessageTextNodeStringHandler.Bold.Build(second);
        var fourth = MessageTextNodeStringHandler.Bold.Build(third);
        var fifth = MessageTextNodeStringHandler.Bold.Build(fourth);
        var sixth = MessageTextNodeStringHandler.Bold.Build(fifth);
        var seventh = MessageTextNodeStringHandler.Bold.Build(sixth);
        var eighth = MessageTextNodeStringHandler.Bold.Build(seventh);
        var ninth = MessageTextNodeStringHandler.Bold.Build(eighth);
        var tenth = MessageTextNodeStringHandler.Bold.Build(ninth);
        return tenth;
    }

    [Benchmark(Baseline = true)]
    public string InterpolatedString() => $"**********{textToBold}**********";

    [Benchmark]
    public string StringFormat() => string.Format("**********{0}**********", textToBold);

    [Benchmark]
    public string Concatenation() => "**********" + textToBold + "**********";

    [Benchmark]
    public string StringJoin() => string.Join(string.Empty, "**********", textToBold, "**********");

    [Benchmark]
    public string StringConcat() => string.Concat("**********", textToBold, "**********");

    [Benchmark]
    public string StringBuilder_Default()
    {
        string result = textToBold;
        var sb = new StringBuilder();
        sb.Append("**********");
        sb.Append(result);
        sb.Append("**********");
        result = sb.ToString();
        return result;
    }

    [Benchmark]
    public string StringBuilder_PreciseSize()
    {
        string result = textToBold;
        var sb = new StringBuilder(10 + result.Length + 10);
        sb.Append("**********");
        sb.Append(result);
        sb.Append("**********");
        result = sb.ToString();
        return result;
    }

    [Benchmark]
    public string StringBuilder_Reserved100()
    {
        string result = textToBold;
        var sb = new StringBuilder(100);
        sb.Append("**********");
        sb.Append(result);
        sb.Append("**********");
        result = sb.ToString();
        return result;
    }
}