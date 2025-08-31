using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using StringEnricher.Nodes;
using StringEnricher.Nodes.MarkdownV2.Formatting;

namespace StringEnricher.Benchmarks.Nodes.CompleteStyledStringBuildingBenchmarks;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[RankColumn]
[MinColumn]
[MaxColumn]
[MeanColumn]
[MedianColumn]
public class CompleteStyledStringBuildingBenchmarks
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

    [Benchmark]
    public string BuildStringWithStringBuilder()
    {
        var sb = new System.Text.StringBuilder();

        for (var i = 0; i < Strings.Length; i++)
        {
            sb.Append("*__");
            sb.Append(Strings[i]);
            sb.Append("__*");

            if (i < Strings.Length - 1)
                sb.Append(' ');
        }

        return sb.ToString();
    }

    [Benchmark(Baseline = true)]
    public string BuildStringWithStringBuilderPrecise()
    {
        const int prefixLength = 3; // *__
        const int suffixLength = 3; // __*
        const int spaceLength = 1; // ' '

        var length = (prefixLength + suffixLength) * Strings.Length
                     + spaceLength * (Strings.Length - 1);

        var sb = new System.Text.StringBuilder(length);

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
    public string BuildStringWithConcatenation()
    {
        string finalString = "";

        for (int i = 0; i < Strings.Length - 1; i++)
        {
            finalString += "*__" + Strings[i] + "__*" + " ";
        }

        // Add last string without space
        finalString += "*__" + Strings[^1] + "__*";

        return finalString;
    }

    [Benchmark]
    public string BuildStringWithNodesSpanBased()
    {
        // Pre-calculate total length
        var totalLength = 0;
        for (var i = 0; i < Strings.Length - 1; i++)
        {
            var node = BoldMarkdownV2.Apply(UnderlineMarkdownV2.Apply(Strings[i]));
            totalLength += node.TotalLength + 1; // +1 for space
        }

        // Add last string without space
        var lastNode = BoldMarkdownV2.Apply(UnderlineMarkdownV2.Apply(Strings[^1]));
        totalLength += lastNode.TotalLength;

        // Create string with exact capacity and write directly to span
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
    public string BuildStringWithNodesMessageBuilderBased()
    {
        // Pre-calculate total length
        var totalLength = 0;
        for (var i = 0; i < Strings.Length - 1; i++)
        {
            var node = BoldMarkdownV2.Apply(UnderlineMarkdownV2.Apply(Strings[i]));
            totalLength += node.TotalLength + 1; // +1 for space
        }

        // Add last string without space
        var lastNode = BoldMarkdownV2.Apply(UnderlineMarkdownV2.Apply(Strings[^1]));
        totalLength += lastNode.TotalLength;
        
        var messageBuilder = new MessageBuilder(totalLength);

        // Create string with exact capacity and write directly to span
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
    public string BuildStringWithNodesSpanBasedCustomLengthCalculation()
    {
        // Most allocation-free approach: calculate total length first
        var totalLength = 0;

        for (var i = 0; i < Strings.Length - 1; i++)
        {
            totalLength += 3 + Strings[i].Length + 3 + 1; // "*__" + text + "__*" + " " = 3 + text.Length + 3 + 1
        }

        // Add last string without space
        totalLength += 3 + Strings[^1].Length + 3; // "*__" + text + "__*" = 3 + text.Length + 3

        // Write directly to span using individual nodes (no accumulation = no type nesting)
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
}