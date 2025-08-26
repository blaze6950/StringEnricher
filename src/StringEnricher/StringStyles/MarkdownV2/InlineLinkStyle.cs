namespace StringEnricher.StringStyles.MarkdownV2;

public static class InlineLinkMarkdownV2
{
    public static InlineLinkStyle<PlainTextStyle> Apply(string linkTitle, string linkUrl) =>
        InlineLinkStyle<PlainTextStyle>.Apply(linkTitle, linkUrl);

    public static InlineLinkStyle<T> Apply<T>(T linkTitle, T linkUrl) where T : IStyle =>
        InlineLinkStyle<T>.Apply(linkTitle, linkUrl);
}

public readonly struct InlineLinkStyle<TInner> : IStyle
    where TInner : IStyle
{
    public const string Prefix = "[";
    public const string LinkSeparator = "](";
    public const string Suffix = ")";

    private readonly TInner _linkTitle;
    private readonly TInner _linkUrl;

    public InlineLinkStyle(TInner linkTitle, TInner linkUrl)
    {
        _linkTitle = linkTitle;
        _linkUrl = linkUrl;
    }

    public override string ToString() => string.Create(TotalLength, this, static (span, style) => style.CopyTo(span));

    public int InnerLength => _linkTitle.TotalLength + _linkUrl.TotalLength;
    public int SyntaxLength => Prefix.Length + LinkSeparator.Length + Suffix.Length;
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

        _linkTitle.CopyTo(destination.Slice(pos, _linkTitle.TotalLength));
        pos += _linkTitle.TotalLength;

        LinkSeparator.AsSpan().CopyTo(destination.Slice(pos, LinkSeparator.Length));
        pos += LinkSeparator.Length;

        _linkUrl.CopyTo(destination.Slice(pos, _linkUrl.TotalLength));
        pos += _linkUrl.TotalLength;

        Suffix.AsSpan().CopyTo(destination.Slice(pos, Suffix.Length));

        return totalLength;
    }

    public static InlineLinkStyle<TInner> Apply(TInner linkTitle, TInner linkUrl) => new(linkTitle, linkUrl);
}