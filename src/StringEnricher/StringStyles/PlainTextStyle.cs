namespace StringEnricher.StringStyles;

public readonly struct PlainTextStyle : IStyle
{
    private readonly string _text;

    public PlainTextStyle(string text)
    {
        ArgumentNullException.ThrowIfNull(text);
        _text = text;
    }

    public int SyntaxLength => 0;
    public int TotalLength => _text.Length;

    public int CopyTo(Span<char> destination)
    {
        var textLength = _text.Length;
        if (destination.Length < textLength)
        {
            throw new ArgumentException("Destination span too small.");
        }

        _text.AsSpan().CopyTo(destination);

        return textLength;
    }

    public static implicit operator PlainTextStyle(string text) => new(text);
}