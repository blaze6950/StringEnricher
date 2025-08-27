using System.Text;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using StringEnricher.Benchmarks.AnalogueImplementations;
using StringEnricher.StringStyles.MarkdownV2;

namespace StringEnricher.Benchmarks.TenfoldBoldStyleBenchmarks;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[RankColumn]
[MinColumn]
[MaxColumn]
[MeanColumn]
[MedianColumn]
public class TenfoldBoldStyleBenchmarks
{
    private string textToBold = "bold text";

    public TenfoldBoldStyleBenchmarks()
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