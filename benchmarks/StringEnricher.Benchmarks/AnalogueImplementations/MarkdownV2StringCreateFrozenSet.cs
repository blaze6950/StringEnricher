using System.Collections.Frozen;

namespace StringEnricher.Benchmarks.AnalogueImplementations;

public static class MarkdownV2StringCreateFrozenSet
{
    private static readonly FrozenSet<char> CharsToEscape = FrozenSet.Create('_', '*', '~', '`', '#', '+', '-', '=', '.', '!',
        '[', ']', '(', ')', '{', '}', '>', '|', '\\');

    public static string Escape(string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            return input;
        }

        // calculate the required size of the StringBuilder
        int additionalChars = 0;
        for (int index = 0, added = 0; index < input.Length; index++)
        {
            if (CharsToEscape.Contains(input[index]))
            {
                additionalChars++;
            }
        }

        if (additionalChars == 0)
        {
            return input;
        }

        // use string.Create to allocate the exact required size and fill it in one go
        return string.Create(input.Length + additionalChars, input, static (span, stringToEscape) =>
        {
            var pos = 0;
            for (int index = 0; index < stringToEscape.Length; index++)
            {
                if (CharsToEscape.Contains(stringToEscape[index]))
                {
                    span[pos++] = '\\';
                }

                span[pos++] = stringToEscape[index];
            }
        });
    }
}