using System.Runtime.CompilerServices;

namespace StringEnricher.Benchmarks.AnalogueImplementations;

/// <summary>
/// Implementation with extracted method for character checking using MethodImplOptions.AggressiveInlining
/// </summary>
public static class MarkdownV2StringCreateAggressiveInline
{
    /// <summary>
    /// Checks if a character needs to be escaped in MarkdownV2
    /// </summary>
    /// <param name="c">Character to check</param>
    /// <returns>True if the character needs escaping, false otherwise</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
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
