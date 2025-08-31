namespace StringEnricher.Helpers.Html;

/// <summary>
/// Provides methods to escape special characters in a string for HTML formatting.
/// </summary>
public static class Html
{
    /// <summary>
    /// Escapes HTML reserved characters in the input string.
    /// All &lt;, &gt; and &amp; symbols are replaced with their corresponding HTML entities.
    /// </summary>
    /// <param name="input">A string to escape.</param>
    /// <returns>The provided string but with escaped HTML characters.</returns>
    public static string Escape(string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            // nothing to escape, return the original string
            return input;
        }

        var additionalCharsNeeded = CountAdditionalCharsNeeded(input);

        if (additionalCharsNeeded == 0)
        {
            // nothing to escape, return the original string
            return input;
        }

        // estimate the final capacity: original length + additional characters needed for entities
        var estimatedCapacity = input.Length + additionalCharsNeeded;

        // use string.Create to allocate the exact required size and fill it in one go
        return string.Create(estimatedCapacity, input,
            static (span, stringToEscape) => EscapeStringToSpan(stringToEscape, span));

        // local functions

        static int CountAdditionalCharsNeeded(string input)
        {
            var additionalLength = 0;
            for (var index = 0; index < input.Length; index++)
            {
                switch (input[index])
                {
                    case '<':
                        additionalLength += 3; // "&lt;" - 1 (original char) = 3 additional chars
                        break;
                    case '>':
                        additionalLength += 3; // "&gt;" - 1 (original char) = 3 additional chars
                        break;
                    case '&':
                        additionalLength += 4; // "&amp;" - 1 (original char) = 4 additional chars
                        break;
                }
            }

            return additionalLength;
        }

        static void EscapeStringToSpan(string stringToEscape, Span<char> span)
        {
            var pos = 0; // current position in the span
            for (var index = 0; index < stringToEscape.Length; index++)
            {
                // index - current position in the original string
                // check if the current character needs to be escaped
                switch (stringToEscape[index])
                {
                    case '<':
                        span[pos++] = '&';
                        span[pos++] = 'l';
                        span[pos++] = 't';
                        span[pos++] = ';';
                        break;
                    case '>':
                        span[pos++] = '&';
                        span[pos++] = 'g';
                        span[pos++] = 't';
                        span[pos++] = ';';
                        break;
                    case '&':
                        span[pos++] = '&';
                        span[pos++] = 'a';
                        span[pos++] = 'm';
                        span[pos++] = 'p';
                        span[pos++] = ';';
                        break;
                    default:
                        // copy the original character
                        span[pos++] = stringToEscape[index];
                        break;
                }
            }
        }
    }
}
