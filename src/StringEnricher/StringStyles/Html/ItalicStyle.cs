namespace StringEnricher.StringStyles.Html;

public static class ItalicHtml
{
    public static ItalicStyle<PlainTextStyle> Apply(string text) =>
        ItalicStyle<PlainTextStyle>.Apply(text);

    public static ItalicStyle<T> Apply<T>(T style) where T : IStyle =>
        ItalicStyle<T>.Apply(style);
}

public readonly struct ItalicStyle<TInner> : IStyle
    where TInner : IStyle
{
    public const string Prefix = "<i>";
    public const string Suffix = "</i>";

    private readonly TInner _innerText;

    public ItalicStyle(TInner inner)
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

    public static ItalicStyle<TInner> Apply(TInner innerStyle) => new(innerStyle);
}