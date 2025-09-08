using System.Text;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using StringEnricher.Benchmarks.AnalogueImplementations;
using StringEnricher.Helpers.MarkdownV2;
using StringEnricher.Nodes.MarkdownV2.Formatting;

namespace StringEnricher.Benchmarks.Nodes.TenfoldBoldNodeBenchmarks;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[RankColumn]
[MinColumn]
[MaxColumn]
[MeanColumn]
[MedianColumn]
public class TenfoldBoldNodeBenchmarks
{
    private string textToBold = "bold text";

    public TenfoldBoldNodeBenchmarks()
    {
        textToBold = "bold text1";
        textToBold = "bold text";
    }

    [Benchmark]
    public string BoldMarkdownV2_Apply()
    {
        var first = BoldMarkdownV2.Apply(textToBold);
        var second = BoldMarkdownV2.Apply(first);
        var third = BoldMarkdownV2.Apply(second);
        var fourth = BoldMarkdownV2.Apply(third);
        var fifth = BoldMarkdownV2.Apply(fourth);
        var sixth = BoldMarkdownV2.Apply(fifth);
        var seventh = BoldMarkdownV2.Apply(sixth);
        var eighth = BoldMarkdownV2.Apply(seventh);
        var ninth = BoldMarkdownV2.Apply(eighth);
        var tenth = BoldMarkdownV2.Apply(ninth);
        return tenth.ToString();
    }

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
    public string InterpolatedString()
    {
        var first = $"*{textToBold}*";
        var second = $"*{first}*";
        var third = $"*{second}*";
        var fourth = $"*{third}*";
        var fifth = $"*{fourth}*";
        var sixth = $"*{fifth}*";
        var seventh = $"*{sixth}*";
        var eighth = $"*{seventh}*";
        var ninth = $"*{eighth}*";
        var tenth = $"*{ninth}*";
        return tenth;
    }

    [Benchmark]
    public string StringFormat()
    {
        var first = string.Format("*{0}*", textToBold);
        var second = string.Format("*{0}*", first);
        var third = string.Format("*{0}*", second);
        var fourth = string.Format("*{0}*", third);
        var fifth = string.Format("*{0}*", fourth);
        var sixth = string.Format("*{0}*", fifth);
        var seventh = string.Format("*{0}*", sixth);
        var eighth = string.Format("*{0}*", seventh);
        var ninth = string.Format("*{0}*", eighth);
        var tenth = string.Format("*{0}*", ninth);
        return tenth;
    }

    [Benchmark]
    public string Concatenation()
    {
        var first = "*" + textToBold + "*";
        var second = "*" + first + "*";
        var third = "*" + second + "*";
        var fourth = "*" + third + "*";
        var fifth = "*" + fourth + "*";
        var sixth = "*" + fifth + "*";
        var seventh = "*" + sixth + "*";
        var eighth = "*" + seventh + "*";
        var ninth = "*" + eighth + "*";
        var tenth = "*" + ninth + "*";
        return tenth;
    }

    [Benchmark]
    public string StringJoin()
    {
        var first = string.Join(string.Empty, "*", textToBold, "*");
        var second = string.Join(string.Empty, "*", first, "*");
        var third = string.Join(string.Empty, "*", second, "*");
        var fourth = string.Join(string.Empty, "*", third, "*");
        var fifth = string.Join(string.Empty, "*", fourth, "*");
        var sixth = string.Join(string.Empty, "*", fifth, "*");
        var seventh = string.Join(string.Empty, "*", sixth, "*");
        var eighth = string.Join(string.Empty, "*", seventh, "*");
        var ninth = string.Join(string.Empty, "*", eighth, "*");
        var tenth = string.Join(string.Empty, "*", ninth, "*");
        return tenth;
    }

    [Benchmark]
    public string StringConcat()
    {
        var first = string.Concat("*", textToBold, "*");
        var second = string.Concat("*", first, "*");
        var third = string.Concat("*", second, "*");
        var fourth = string.Concat("*", third, "*");
        var fifth = string.Concat("*", fourth, "*");
        var sixth = string.Concat("*", fifth, "*");
        var seventh = string.Concat("*", sixth, "*");
        var eighth = string.Concat("*", seventh, "*");
        var ninth = string.Concat("*", eighth, "*");
        var tenth = string.Concat("*", ninth, "*");
        return tenth;
    }

    [Benchmark]
    public string StringBuilder_Default()
    {
        string result = textToBold;
        for (int i = 0; i < 10; i++)
        {
            var sb = new StringBuilder();
            sb.Append("*");
            sb.Append(result);
            sb.Append("*");
            result = sb.ToString();
        }
        return result;
    }

    [Benchmark]
    public string StringBuilder_PreciseSize()
    {
        string result = textToBold;
        for (int i = 0; i < 10; i++)
        {
            var sb = new StringBuilder(2 + result.Length);
            sb.Append("*");
            sb.Append(result);
            sb.Append("*");
            result = sb.ToString();
        }
        return result;
    }

    [Benchmark]
    public string StringBuilder_Reserved100()
    {
        string result = textToBold;
        for (int i = 0; i < 10; i++)
        {
            var sb = new StringBuilder(100);
            sb.Append("*");
            sb.Append(result);
            sb.Append("*");
            result = sb.ToString();
        }
        return result;
    }
}