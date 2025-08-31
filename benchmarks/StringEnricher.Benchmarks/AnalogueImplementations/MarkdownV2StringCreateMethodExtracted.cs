using System;

namespace StringEnricher.Benchmarks.AnalogueImplementations;

/// <summary>
/// Implementation with extracted method for character checking to test performance impact
/// </summary>
public static class MarkdownV2StringCreateMethodExtracted
{
    /// <summary>
    /// Checks if a character needs to be escaped in MarkdownV2
    /// </summary>
    /// <param name="c">Character to check</param>
    /// <returns>True if the character needs escaping, false otherwise</returns>
    private static bool NeedsEscaping(char c)
    {
        switch (c)
        {
            case '_':
            case '*':
            case '~':
            case '`':
            case '#':
            case '+':
            case '-':
            case '=':
            case '.':
            case '!':
            case '[':
            case ']':
            case '(':
            case ')':
            case '{':
            case '}':
            case '>':
            case '|':
            case '\\':
                return true;
            default:
                return false;
        }
    }

    public static string Escape(string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            return input;
        }

        var charsToEscape = CountCharsToEscape(input);

        if (charsToEscape == 0)
        {
            return input;
        }

        var estimatedCapacity = input.Length + charsToEscape;

        return string.Create(estimatedCapacity, input,
            static (span, stringToEscape) => EscapeStringToSpan(stringToEscape, span));

        // local functions

        static int CountCharsToEscape(string input)
        {
            var i = 0;
            for (var index = 0; index < input.Length; index++)
            {
                if (NeedsEscaping(input[index]))
                {
                    i++;
                }
            }

            return i;
        }

        static void EscapeStringToSpan(string stringToEscape, Span<char> span)
        {
            var pos = 0;
            for (var index = 0; index < stringToEscape.Length; index++)
            {
                if (NeedsEscaping(stringToEscape[index]))
                {
                    span[pos++] = '\\';
                }

                span[pos++] = stringToEscape[index];
            }
        }
    }
}
