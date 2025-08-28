using System.Text;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using StringEnricher.Benchmarks.AnalogueImplementations;
using StringEnricher.StringStyles.MarkdownV2;

namespace StringEnricher.Benchmarks.StringStyles.HardcoreTenfoldBoldStyleBenchmarks;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[RankColumn]
[MinColumn]
[MaxColumn]
[MeanColumn]
[MedianColumn]
public class HardcoreTenfoldBoldStyleBenchmarks
{
    private string textToBold = "bold text";

    public HardcoreTenfoldBoldStyleBenchmarks()
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
        var first = MessageTextStyleLambda.Bold.Build(textToBold);
        var second = MessageTextStyleLambda.Bold.Build(first);
        var third = MessageTextStyleLambda.Bold.Build(second);
        var fourth = MessageTextStyleLambda.Bold.Build(third);
        var fifth = MessageTextStyleLambda.Bold.Build(fourth);
        var sixth = MessageTextStyleLambda.Bold.Build(fifth);
        var seventh = MessageTextStyleLambda.Bold.Build(sixth);
        var eighth = MessageTextStyleLambda.Bold.Build(seventh);
        var ninth = MessageTextStyleLambda.Bold.Build(eighth);
        var tenth = MessageTextStyleLambda.Bold.Build(ninth);
        return tenth;
    }

    [Benchmark]
    public string MessageTextStyleStringHandler_Bold()
    {
        var first = MessageTextStyleStringHandler.Bold.Build(textToBold);
        var second = MessageTextStyleStringHandler.Bold.Build(first);
        var third = MessageTextStyleStringHandler.Bold.Build(second);
        var fourth = MessageTextStyleStringHandler.Bold.Build(third);
        var fifth = MessageTextStyleStringHandler.Bold.Build(fourth);
        var sixth = MessageTextStyleStringHandler.Bold.Build(fifth);
        var seventh = MessageTextStyleStringHandler.Bold.Build(sixth);
        var eighth = MessageTextStyleStringHandler.Bold.Build(seventh);
        var ninth = MessageTextStyleStringHandler.Bold.Build(eighth);
        var tenth = MessageTextStyleStringHandler.Bold.Build(ninth);
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