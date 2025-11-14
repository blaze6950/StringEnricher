using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using StringEnricher.Builders;
using StringEnricher.Telegram.Helpers.MarkdownV2;

namespace StringEnricher.Benchmarks.Nodes.KnownLengthBenchmarks;

/// <summary>
/// Benchmarks string building when the final length is already known.
/// This scenario highlights MessageBuilder's efficiency by using string.Create() directly
/// without measuring the length calculation overhead.
/// </summary>
[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[RankColumn]
[MinColumn]
[MaxColumn]
[MeanColumn]
[MedianColumn]
public class KnownLengthBenchmarks
{
    public const string Input =
        "Lorem ipsum dolor sit amet, consectetur adipiscing elit Sed do eiusmod tempor incididunt ut labore et" +
        "doloremagna aliqua Lorem ipsum dolor sit amet, consectetur adipiscing elit Sed do eiusmod tempor incididunt" +
        "ut labore et dolore magna aliqua Lorem ipsum dolor sit amet, consectetur adipiscing elit Sed do eiusmod" +
        "tempor incididunt ut labore et dolore magna aliqua Lorem ipsum dolor sit amet, consectetur adipiscing elit" +
        "Sed do eiusmod tempor incididunt ut labore et dolore magna aliqua Lorem ipsum dolor sit amet, consectetur" +
        "adipiscing elit Sed do eiusmod tempor incididunt ut labore et dolore magna aliqua Lorem ipsum dolor sit amet," +
        "consectetur adipiscing elit Sed do eiusmod tempor incididunt ut labore et dolore magna aliqua Lorem ipsum" +
        "dolor sit amet, consectetur adipiscing elit Sed do eiusmod tempor incididunt ut labore et dolore magna aliqua" +
        "Lorem ipsum dolor sit amet, consectetur adipiscing elit Sed do eiusmod tempor incididunt ut labore et dolore" +
        "magna aliqua Lorem ipsum dolor sit amet, consectetur adipiscing elit Sed do eiusmod tempor incididunt ut" +
        "labore et dolore magna aliqua Lorem ipsum dolor sit amet, consectetur adipiscing elit Sed do eiusmod tempor" +
        "incididunt ut labore et dolore magna aliqua Lorem ipsum dolor sit amet, consectetur adipiscing elit Sed do" +
        "eiusmod tempor incididunt ut labore et dolore magna aliqua Lorem ipsum dolor sit amet, consectetur adipiscing" +
        "elit Sed do eiusmod tempor incididunt ut labore et dolore magna aliqua Lorem ipsum dolor sit amet, consectetur" +
        "adipiscing elit Sed do eiusmod tempor incididunt ut labore et dolore magna aliqua Lorem ipsum dolor sit amet," +
        "consectetur adipiscing elit Sed do eiusmod tempor incididunt ut labore et dolore magna aliqua Lorem ipsum" +
        "dolor sit amet, consectetur adipiscing elit Sed do eiusmod tempor incididunt ut labore et dolore magna aliqua" +
        "Lorem ipsum dolor sit amet, consectetur adipiscing elit Sed do eiusmod tempor incididunt ut labore et dolore" +
        "magna aliqua Lorem ipsum dolor sit amet, consectetur adipiscing elit Sed do eiusmod tempor incididunt ut" +
        "labore et dolore magna aliqua Lorem ipsum dolor sit amet, consectetur adipiscing elit Sed do eiusmod tempor" +
        "incididunt ut labore et dolore magna aliqua Lorem ipsum dolor sit amet, consectetur adipiscing elit Sed do" +
        "eiusmod tempor incididunt ut labore et dolore magna aliqua Lorem ipsum dolor sit amet, consectetur adipiscing" +
        "elit Sed do eiusmod tempor incididunt ut labore et dolore magna aliqua Lorem ipsum dolor sit amet, consectetur" +
        "adipiscing elit Sed do eiusmod tempor incididunt ut labore et dolore magna aliqua Lorem ipsum dolor sit amet," +
        "consectetur adipiscing elit Sed do eiusmod tempor incididunt ut labore et dolore magna aliqua Lorem ipsum" +
        "dolor sit amet, consectetur adipiscing elit Sed do eiusmod tempor incididunt ut labore et dolore magna aliqua" +
        "Lorem ipsum dolor sit amet, consectetur adipiscing elit Sed do eiusmod tempor incididunt ut labore et dolore" +
        "magna aliqua Lorem ipsum dolor sit amet, consectetur adipiscing elit Sed do eiusmod tempor incididunt ut labore" +
        "et dolore magna aliqua Lorem ipsum dolor sit amet, consectetur adipiscing elit Sed do eiusmod";

    public readonly static string[] Strings = Input.Split(' ');

    // Pre-calculated final length (done once during setup, not measured)
    private int _finalLength;

    [GlobalSetup]
    public void Setup()
    {
        // Calculate the final length once - this is NOT part of the benchmark measurements
        _finalLength = 0;
        for (var i = 0; i < Strings.Length - 1; i++)
        {
            var node = BoldMarkdownV2.Apply(UnderlineMarkdownV2.Apply(Strings[i]));
            _finalLength += node.TotalLength + 1; // +1 for space
        }

        var lastNode = BoldMarkdownV2.Apply(UnderlineMarkdownV2.Apply(Strings[^1]));
        _finalLength += lastNode.TotalLength;
    }

    [Benchmark(Baseline = true)]
    public string StringBuilder_WithPreCalculatedCapacity()
    {
        var sb = new System.Text.StringBuilder(_finalLength);

        for (var i = 0; i < Strings.Length - 1; i++)
        {
            sb.Append("*__");
            sb.Append(Strings[i]);
            sb.Append("__*");
            sb.Append(' ');
        }

        // Add last string without space
        sb.Append("*__");
        sb.Append(Strings[^1]);
        sb.Append("__*");

        return sb.ToString();
    }

    [Benchmark]
    public string StringCreate_WithNodes()
    {
        // Direct string.Create with known length - most efficient approach
        return string.Create(_finalLength, Strings, static (span, strings) =>
        {
            var position = 0;

            for (var i = 0; i < strings.Length - 1; i++)
            {
                var node = BoldMarkdownV2.Apply(UnderlineMarkdownV2.Apply(strings[i]));
                position += node.CopyTo(span[position..]);
                span[position++] = ' ';
            }

            // Add last string without space
            var lastNode = BoldMarkdownV2.Apply(UnderlineMarkdownV2.Apply(strings[^1]));
            lastNode.CopyTo(span[position..]);
        });
    }

    [Benchmark]
    public string MessageBuilder_WithPreCalculatedLength()
    {
        // MessageBuilder uses string.Create internally - this shows its optimal use case
        var messageBuilder = new MessageBuilder(_finalLength);

        return messageBuilder.Create(Strings, static (strings, context) =>
        {
            for (var i = 0; i < strings.Length - 1; i++)
            {
                var node = BoldMarkdownV2.Apply(UnderlineMarkdownV2.Apply(strings[i]));
                context.Append(node);
                context.Append(' ');
            }

            // Add last string without space
            var lastNode = BoldMarkdownV2.Apply(UnderlineMarkdownV2.Apply(strings[^1]));
            context.Append(lastNode);
        });
    }
}
