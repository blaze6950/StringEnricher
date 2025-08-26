namespace StringEnricher.StringStyles.Html;

public static class BoldHtml
{
    public static BoldStyle<PlainTextStyle> Apply(string text) =>
        BoldStyle<PlainTextStyle>.Apply(text);

    public static BoldStyle<T> Apply<T>(T style) where T : IStyle =>
        BoldStyle<T>.Apply(style);
}

public readonly struct BoldStyle<TInner> : IStyle
    where TInner : IStyle
{
    public const string Prefix = "<b>";
    public const string Suffix = "</b>";

    private readonly TInner _innerText;

    public BoldStyle(TInner inner)
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

    public static BoldStyle<TInner> Apply(TInner innerStyle) => new(innerStyle);
}