namespace StringEnricher.StringStyles.Html;

public static class TgEmojiHtml
{
    public static TgEmojiStyle<PlainTextStyle> Apply(string linkTitle, string linkUrl) =>
        TgEmojiStyle<PlainTextStyle>.Apply(linkTitle, linkUrl);

    public static TgEmojiStyle<T> Apply<T>(T linkTitle, T linkUrl) where T : IStyle =>
        TgEmojiStyle<T>.Apply(linkTitle, linkUrl);
}

public readonly struct TgEmojiStyle<TInner> : IStyle
    where TInner : IStyle
{
    public const string Prefix = "<tg-emoji emoji-id=\"";
    public const string Separator = "\">";
    public const string Suffix = "</tg-emoji>";

    private readonly TInner _defaultEmoji;
    private readonly TInner _customEmoji;

    public TgEmojiStyle(TInner defaultEmoji, TInner customEmoji)
    {
        _defaultEmoji = defaultEmoji;
        _customEmoji = customEmoji;
    }

    public override string ToString() => string.Create(TotalLength, this, static (span, style) => style.CopyTo(span));

    public int InnerLength => _defaultEmoji.TotalLength + _customEmoji.TotalLength;
    public int SyntaxLength => Prefix.Length + Separator.Length + Suffix.Length;
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

        _customEmoji.CopyTo(destination.Slice(pos, _customEmoji.TotalLength));
        pos += _customEmoji.TotalLength;

        Separator.AsSpan().CopyTo(destination.Slice(pos, Separator.Length));
        pos += Separator.Length;

        _defaultEmoji.CopyTo(destination.Slice(pos, _defaultEmoji.TotalLength));
        pos += _defaultEmoji.TotalLength;

        Suffix.AsSpan().CopyTo(destination.Slice(pos, Suffix.Length));

        return totalLength;
    }

    public static TgEmojiStyle<TInner> Apply(TInner linkTitle, TInner linkUrl) => new(linkTitle, linkUrl);
}