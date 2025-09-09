using System.Text;

namespace StringEnricher.Benchmarks.AnalogueImplementations;

public static class MarkdownV2StringBuilderPrecise
{
    public static string Escape(string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            return input;
        }

        // calculate the required size of the StringBuilder
        var additionalChars = 0;
        for (int index = 0, added = 0; index < input.Length; index++)
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
                    additionalChars++;
                    break;
            }
        }

        if (additionalChars == 0)
        {
            return input;
        }

        var sb = new StringBuilder(input.Length + additionalChars);
        for (int index = 0, added = 0; index < input.Length; index++)
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
                    sb.Append('\\');
                    break;
            }

            sb.Append(input[index]);
        }

        return sb.ToString();
    }
}