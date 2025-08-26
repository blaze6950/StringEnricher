namespace StringEnricher.StringStyles.MarkdownV2;

public static class TgEmojiMarkdownV2
{
    public static TgEmojiStyle<PlainTextStyle> Apply(string defaultEmoji, string customEmojiId) =>
        TgEmojiStyle<PlainTextStyle>.Apply(defaultEmoji, customEmojiId);

    public static TgEmojiStyle<T> Apply<T>(T defaultEmoji, T customEmojiId) where T : IStyle =>
        TgEmojiStyle<T>.Apply(defaultEmoji, customEmojiId);
}

public readonly struct TgEmojiStyle<TInner> : IStyle
    where TInner : IStyle
{
    public const string Prefix = "![";
    public const string Separator = "](tg://emoji?id=";
    public const string Suffix = ")";

    private readonly TInner _defaultEmoji;
    private readonly TInner _customEmojiId;

    public TgEmojiStyle(TInner defaultEmoji, TInner customEmojiId)
    {
        _defaultEmoji = defaultEmoji;
        _customEmojiId = customEmojiId;
    }

    public override string ToString() => string.Create(TotalLength, this, static (span, style) => style.CopyTo(span));

    public int InnerLength => _defaultEmoji.TotalLength + _customEmojiId.TotalLength;
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

        _defaultEmoji.CopyTo(destination.Slice(pos, _defaultEmoji.TotalLength));
        pos += _defaultEmoji.TotalLength;

        Separator.AsSpan().CopyTo(destination.Slice(pos, Separator.Length));
        pos += Separator.Length;

        _customEmojiId.CopyTo(destination.Slice(pos, _customEmojiId.TotalLength));
        pos += _customEmojiId.TotalLength;

        Suffix.AsSpan().CopyTo(destination.Slice(pos, Suffix.Length));

        return totalLength;
    }

    public static TgEmojiStyle<TInner> Apply(TInner defaultEmoji, TInner customEmojiId) => new(defaultEmoji, customEmojiId);
}