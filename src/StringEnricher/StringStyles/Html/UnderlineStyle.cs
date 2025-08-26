namespace StringEnricher.StringStyles.Html;

public static class UnderlineHtml
{
    public static UnderlineStyle<PlainTextStyle> Apply(string text) =>
        UnderlineStyle<PlainTextStyle>.Apply(text);

    public static UnderlineStyle<T> Apply<T>(T style) where T : IStyle =>
        UnderlineStyle<T>.Apply(style);
}

public readonly struct UnderlineStyle<TInner> : IStyle
    where TInner : IStyle
{
    public const string Prefix = "<u>";
    public const string Suffix = "</u>";

    private readonly TInner _innerText;

    public UnderlineStyle(TInner inner)
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

    public static UnderlineStyle<TInner> Apply(TInner innerStyle) => new(innerStyle);
}