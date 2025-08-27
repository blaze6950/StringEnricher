namespace StringEnricher.Benchmarks.AnalogueImplementations;

public record MessageTextStyleLambda
{
    private readonly int _numberOfParams;
    private readonly Func<string[], string> _formatter;

    public MessageTextStyleLambda(int numberOfParams, Func<string[], string> formatter)
    {
        _numberOfParams = numberOfParams;
        _formatter = formatter;
    }

    public string Build(params string[] parameters)
    {
        if (_numberOfParams != parameters.Length)
        {
            throw new ArgumentException(
                $"The number of parameters does not match the number of formatted items in the template. Expected parameters: {_numberOfParams}");
        }

        return _formatter(parameters);
    }

    public static readonly MessageTextStyleLambda Bold = new(1, p => $"*{p[0]}*");
    public static readonly MessageTextStyleLambda BoldHtml = new(1, p => $"<b>{p[0]}</b>");
    public static readonly MessageTextStyleLambda Italic = new(1, p => $"_{p[0]}_");
    public static readonly MessageTextStyleLambda Underline = new(1, p => $"__{p[0]}__");
    public static readonly MessageTextStyleLambda Spoiler = new(1, p => $"||{p[0]}||");
    public static readonly MessageTextStyleLambda Strikethrough = new(1, p => $"~{p[0]}~");
    public static readonly MessageTextStyleLambda InlineUrl = new(2, p => $"[{p[0]}]({p[1]})");
}