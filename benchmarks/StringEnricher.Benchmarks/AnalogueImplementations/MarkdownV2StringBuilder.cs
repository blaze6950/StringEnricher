using System.Text;

namespace StringEnricher.Benchmarks.AnalogueImplementations;

public static class MarkdownV2StringBuilder
{
    public static string Escape(string input)
    {
        if (input == null)
        {
            return null;
        }

        StringBuilder sb = null;
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
                    sb ??= new StringBuilder(input, input.Length + 32);
                    sb.Insert(index + added++, '\\');
                    break;
            }
        }

        return sb?.ToString() ?? input;
    }
}