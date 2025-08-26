namespace StringEnricher.StringStyles.MarkdownV2;

public static class InlineCodeMarkdownV2
{
    public static InlineCodeStyle<PlainTextStyle> Apply(string text) =>
        InlineCodeStyle<PlainTextStyle>.Apply(text);

    public static InlineCodeStyle<T> Apply<T>(T style) where T : IStyle =>
        InlineCodeStyle<T>.Apply(style);
}

public readonly struct InlineCodeStyle<TInner> : IStyle
    where TInner : IStyle
{
    public const string Prefix = "`";
    public const string Suffix = "`";

    private readonly TInner _innerText;

    public InlineCodeStyle(TInner inner)
    {
        _innerText = inner;
    }

    public override string ToString() => string.Create(TotalLength, this, static (span, style) => style.CopyTo(span));

    public int InnerLength => _innerText.TotalLength;
    public int SyntaxLength => Prefix.Length + Suffix.Length;
    public int TotalLength => SyntaxLength + InnerLength;

    public int CopyTo(Span<char> destination)
    {
        var totalLength = TotalLength;
        if (destination.Length < totalLength)
        {
            throw new ArgumentException("The destination span is too small to hold the formatted text.");
        }

        var pos = 0;
        Prefix.AsSpan().CopyTo(destination.Slice(pos, Prefix.Length));
        pos += Prefix.Length;

        _innerText.CopyTo(destination.Slice(pos, InnerLength));
        pos += InnerLength;

        Suffix.AsSpan().CopyTo(destination.Slice(pos, Suffix.Length));

        return totalLength;
    }

    public static InlineCodeStyle<TInner> Apply(TInner innerStyle) => new(innerStyle);
}