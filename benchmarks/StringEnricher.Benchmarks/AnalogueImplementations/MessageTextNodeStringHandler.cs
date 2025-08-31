using System.Runtime.CompilerServices;

namespace StringEnricher.Benchmarks.AnalogueImplementations;

public record MessageTextNodeStringHandler
{
    private readonly MessageTextInterpolatedStringHandler _stringHandler;

    public MessageTextNodeStringHandler(MessageTextInterpolatedStringHandler stringHandler)
    {
        _stringHandler = stringHandler;
    }

    public string Build(params string[] parameters)
    {
        if (parameters.Length != _stringHandler.AddedFormatted)
        {
            throw new ArgumentException(
                $"The number of parameters does not match the number of formatted items in the template. Expected parameters: {_stringHandler.AddedFormatted}");
        }

        var totalLength = _stringHandler.LiteralLength;
        for (int i = 0; i < parameters.Length; i++)
        {
            totalLength += parameters[i].Length;
        }

        return string.Create(totalLength, (_stringHandler, parameters), static (span, state) =>
        {
            var (stringHandler, parameters) = state;
            var pos = 0;

            var totalStringPartCount = stringHandler.AddedLiterals + stringHandler.AddedFormatted;
            var currentFormattedIndex = 0;
            var currentLiteralIndex = 0;

            for (int i = 0; i < totalStringPartCount; i++)
            {
                if (stringHandler.FormattedPositions[currentFormattedIndex] == i)
                {
                    // current position is for formatted
                    CopyString(parameters[currentFormattedIndex], ref pos, span);

                    if (parameters.Length - 1 != currentFormattedIndex)
                    {
                        currentFormattedIndex++;
                    }
                }
                else
                {
                    // current position is for literal
                    CopyString(stringHandler.Literals[currentLiteralIndex], ref pos, span);

                    if (stringHandler.Literals.Length - 1 != currentLiteralIndex)
                    {
                        currentLiteralIndex++;
                    }
                }
            }

            return;

            // Local helper to copy a ReadOnlySpan<char> into the destination.
            static void CopyString(ReadOnlySpan<char> source, ref int pos, Span<char> dest)
            {
                source.CopyTo(dest.Slice(pos, source.Length));
                pos += source.Length;
            }
        });
    }

    public static readonly MessageTextNodeStringHandler Bold = new($"*{0}*");
    public static readonly MessageTextNodeStringHandler BoldHtml = new($"<b>{0}</b>");
    public static readonly MessageTextNodeStringHandler Italic = new($"_{0}_");
    public static readonly MessageTextNodeStringHandler Underline = new($"__{0}__");
    public static readonly MessageTextNodeStringHandler Spoiler = new($"||{0}||");
    public static readonly MessageTextNodeStringHandler Strikethrough = new($"~{0}~");
    public static readonly MessageTextNodeStringHandler InlineUrl = new($"[{0}]({1})");
}

[InterpolatedStringHandler]
public struct MessageTextInterpolatedStringHandler
{
    public readonly int LiteralLength;

    public readonly string[] Literals;
    public int AddedLiterals = 0;

    public readonly int[] FormattedPositions;
    public int AddedFormatted = 0;

    public MessageTextInterpolatedStringHandler(int literalLength, int formattedCount)
    {
        LiteralLength = literalLength;
        Literals = new string[formattedCount + 1];
        FormattedPositions = new int[formattedCount];
    }

    public void AppendLiteral(string s)
    {
        Literals[AddedLiterals] = s;
        AddedLiterals++;
    }

    public void AppendFormatted<T>(T t)
    {
        FormattedPositions[AddedFormatted] = AddedLiterals + AddedFormatted;
        AddedFormatted++;
    }

    internal string GetFormattedText() => throw new InvalidOperationException(
        "Use this string formatter only as a template parser from the interpolated string for the MessageTextStyle");
}