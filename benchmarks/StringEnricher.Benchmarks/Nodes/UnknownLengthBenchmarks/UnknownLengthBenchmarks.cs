using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using StringEnricher.Builders;
using StringEnricher.Telegram.Helpers.MarkdownV2;

namespace StringEnricher.Benchmarks.Nodes.UnknownLengthBenchmarks;

/// <summary>
/// Benchmarks string building when the final length is unknown and must be calculated.
/// This scenario measures both length calculation and string building,
/// highlighting AutoMessageBuilder's simplicity and good performance without pre-calculation.
/// </summary>
[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[RankColumn]
[MinColumn]
[MaxColumn]
[MeanColumn]
[MedianColumn]
public class UnknownLengthBenchmarks
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

    [Benchmark(Baseline = true)]
    public string StringBuilder_WithoutCapacity()
    {
        var sb = new System.Text.StringBuilder();

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
    public string StringBuilder_WithCalculatedCapacity()
    {
        // Calculate length first, then build
        var totalLength = 0;
        for (var i = 0; i < Strings.Length - 1; i++)
        {
            var node = BoldMarkdownV2.Apply(UnderlineMarkdownV2.Apply(Strings[i]));
            totalLength += node.TotalLength + 1; // +1 for space
        }

        var lastNode = BoldMarkdownV2.Apply(UnderlineMarkdownV2.Apply(Strings[^1]));
        totalLength += lastNode.TotalLength;

        var sb = new System.Text.StringBuilder(totalLength);

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
    public string StringCreate_WithCalculatedLength()
    {
        // Calculate total length first
        var totalLength = 0;
        for (var i = 0; i < Strings.Length - 1; i++)
        {
            var node = BoldMarkdownV2.Apply(UnderlineMarkdownV2.Apply(Strings[i]));
            totalLength += node.TotalLength + 1; // +1 for space
        }

        var lastNode = BoldMarkdownV2.Apply(UnderlineMarkdownV2.Apply(Strings[^1]));
        totalLength += lastNode.TotalLength;

        // Then create string with exact capacity and write directly to span
        return string.Create(totalLength, Strings, static (span, strings) =>
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
    public string MessageBuilder_WithCalculatedLength()
    {
        // Calculate total length first
        var totalLength = 0;
        for (var i = 0; i < Strings.Length - 1; i++)
        {
            var node = BoldMarkdownV2.Apply(UnderlineMarkdownV2.Apply(Strings[i]));
            totalLength += node.TotalLength + 1; // +1 for space
        }

        var lastNode = BoldMarkdownV2.Apply(UnderlineMarkdownV2.Apply(Strings[^1]));
        totalLength += lastNode.TotalLength;

        var messageBuilder = new MessageBuilder(totalLength);

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

    [Benchmark]
    public string AutoMessageBuilder_Simple()
    {
        // No length calculation needed - AutoMessageBuilder handles it automatically
        // This is the simplest approach with good performance
        var autoMessageBuilder = new AutoMessageBuilder();

        return autoMessageBuilder.Create(Strings, static (strings, context) =>
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
            return context.Length;
        });
    }

    [Benchmark]
    public string AutoMessageBuilder_NonStatic()
    {
        // Same as above but with non-static lambda (may have slight overhead due to closure)
        var autoMessageBuilder = new AutoMessageBuilder();

        return autoMessageBuilder.Create(Strings, (strings, context) =>
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
            return context.Length;
        });
    }
}
