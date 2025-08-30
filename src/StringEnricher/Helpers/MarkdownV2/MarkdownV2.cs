namespace StringEnricher.Helpers.MarkdownV2;

/// <summary>
/// Provides methods to escape special characters in a string for MarkdownV2 formatting.
/// </summary>
public static class MarkdownV2
{
    /// <summary>
    /// Escapes special characters in the input string according to MarkdownV2 rules.
    /// Docs: <see href="https://core.telegram.org/bots/api#markdownv2-style"/>
    /// </summary>
    /// <param name="input">A string to escape.</param>
    /// <returns>The provided string but with escaped MarkdownV2 characters.</returns>
    public static string Escape(string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            // nothing to escape, return the original string
            return input;
        }

        var charsToEscape = CountCharsToEscape(input);

        if (charsToEscape == 0)
        {
            // nothing to escape, return the original string
            return input;
        }

        // estimate the final capacity: original length + number of '\' to be added to escape characters
        var estimatedCapacity = input.Length + charsToEscape;

        // use string.Create to allocate the exact required size and fill it in one go
        return string.Create(estimatedCapacity, input,
            static (span, stringToEscape) => EscapeStringToSpan(stringToEscape, span));

        // local functions

        static int CountCharsToEscape(string input)
        {
            var i = 0;
            for (var index = 0; index < input.Length; index++)
            {
                switch (input[index])
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
                        i++;
                        break;
                }
            }

            return i;
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
                        // prepend the escape character
                        span[pos++] = '\\';
                        break;
                }

                // copy the original character
                span[pos++] = stringToEscape[index];
            }
        }
    }
}