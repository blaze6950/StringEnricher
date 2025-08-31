namespace StringEnricher.Nodes.MarkdownV2.Formatting;

/// <summary>
/// Provides methods to create TG emoji styles in MarkdownV2 format.
/// Example: "![👍](tg://emoji?id=123456)"
/// </summary>
public static class TgEmojiMarkdownV2
{
    /// <summary>
    /// Creates a new instance of the <see cref="TgEmojiNode{TInner}"/> struct.
    /// </summary>
    /// <param name="defaultEmoji">
    /// The default emoji to display if the custom emoji is not available.
    /// </param>
    /// <param name="customEmojiId">
    /// The unique identifier of the custom emoji.
    /// </param>
    /// <returns>
    /// The created instance of the <see cref="TgEmojiNode{TInner}"/> struct.
    /// </returns>
    public static TgEmojiNode<PlainTextNode> Apply(string defaultEmoji, string customEmojiId) =>
        TgEmojiNode<PlainTextNode>.Apply(defaultEmoji, customEmojiId);

    /// <summary>
    /// Creates a new instance of the <see cref="TgEmojiNode{TInner}"/> struct.
    /// </summary>
    /// <param name="defaultEmoji">
    /// The default emoji to display if the custom emoji is not available.
    /// </param>
    /// <param name="customEmojiId">
    /// The unique identifier of the custom emoji.
    /// </param>
    /// <typeparam name="T">
    /// The type of the inner style that will be wrapped with TG emoji syntax.
    /// </typeparam>
    /// <returns>
    /// The created instance of the <see cref="TgEmojiNode{TInner}"/> struct.
    /// </returns>
    public static TgEmojiNode<T> Apply<T>(T defaultEmoji, T customEmojiId) where T : INode =>
        TgEmojiNode<T>.Apply(defaultEmoji, customEmojiId);
}

/// <summary>
/// Represents TG emoji text in MarkdownV2 format.
/// Example: "![👍](tg://emoji?id=123456)"
/// </summary>
/// <typeparam name="TInner">
/// The type of the inner style that will be wrapped with TG emoji syntax.
/// </typeparam>
public readonly struct TgEmojiNode<TInner> : INode
    where TInner : INode
{
    /// <summary>
    /// The prefix of the TG emoji style in MarkdownV2 format.
    /// </summary>
    public const string Prefix = "![";

    /// <summary>
    /// The separator of the TG emoji style in MarkdownV2 format.
    /// </summary>
    public const string Separator = "](tg://emoji?id=";

    /// <summary>
    /// The suffix of the TG emoji style in MarkdownV2 format.
    /// </summary>
    public const string Suffix = ")";

    private readonly TInner _defaultEmoji;
    private readonly TInner _customEmojiId;

    /// <summary>
    /// Initializes a new instance of the <see cref="TgEmojiNode{TInner}"/> struct.
    /// </summary>
    /// <param name="defaultEmoji">
    /// The default emoji to display if the custom emoji is not available.
    /// </param>
    /// <param name="customEmojiId">
    /// The unique identifier of the custom emoji.
    /// </param>
    public TgEmojiNode(TInner defaultEmoji, TInner customEmojiId)
    {
        _defaultEmoji = defaultEmoji;
        _customEmojiId = customEmojiId;
    }

    /// <summary>
    /// Returns the string representation of the TG emoji style in MarkdownV2 format.
    /// Note: This method allocates a new string in the most efficient way possible.
    /// Use this method when you finished all styling operations and need the final string.
    /// </summary>
    /// <returns>The created string representation</returns>
    public override string ToString() => string.Create(TotalLength, this, static (span, style) => style.CopyTo(span));

    /// <summary>
    /// Gets the length of the inner styled text.
    /// </summary>
    public int InnerLength => _defaultEmoji.TotalLength + _customEmojiId.TotalLength;

    /// <inheritdoc />
    public int SyntaxLength => Prefix.Length + Separator.Length + Suffix.Length;

    /// <inheritdoc />
    public int TotalLength => SyntaxLength + InnerLength;

    /// <inheritdoc />
    public int CopyTo(Span<char> destination)
    {
        var totalLength = TotalLength;
        if (destination.Length < totalLength)
        {
            throw new ArgumentException("The destination span is too small to hold the formatted text.");
        }

        var pos = 0;

        // Copy Prefix
        Prefix.AsSpan().CopyTo(destination.Slice(pos, Prefix.Length));
        pos += Prefix.Length;

        // Copy Default Emoji
        _defaultEmoji.CopyTo(destination.Slice(pos, _defaultEmoji.TotalLength));
        pos += _defaultEmoji.TotalLength;

        // Copy Separator
        Separator.AsSpan().CopyTo(destination.Slice(pos, Separator.Length));
        pos += Separator.Length;

        // Copy Custom Emoji ID
        _customEmojiId.CopyTo(destination.Slice(pos, _customEmojiId.TotalLength));
        pos += _customEmojiId.TotalLength;

        // Copy Suffix
        Suffix.AsSpan().CopyTo(destination.Slice(pos, Suffix.Length));

        return totalLength;
    }

    /// <inheritdoc />
    public bool TryGetChar(int index, out char character)
    {
        if (index < 0 || index >= TotalLength)
        {
            character = '\0';
            return false;
        }

        if (index < Prefix.Length)
        {
            character = Prefix[index];
            return true;
        }

        index -= Prefix.Length;

        if (index < _defaultEmoji.TotalLength)
        {
            return _defaultEmoji.TryGetChar(index, out character);
        }

        index -= _defaultEmoji.TotalLength;

        if (index < Separator.Length)
        {
            character = Separator[index];
            return true;
        }

        index -= Separator.Length;

        if (index < _customEmojiId.TotalLength)
        {
            return _customEmojiId.TryGetChar(index, out character);
        }

        index -= _customEmojiId.TotalLength;

        // At this point, index must be within the Suffix
        character = Suffix[index];
        return true;
    }

    /// <summary>
    /// Creates a new instance of the <see cref="TgEmojiNode{TInner}"/> struct.
    /// </summary>
    /// <param name="defaultEmoji">
    /// The default emoji to display if the custom emoji is not available.
    /// </param>
    /// <param name="customEmojiId">
    /// The unique identifier of the custom emoji.
    /// </param>
    /// <returns>
    /// The created instance of the <see cref="TgEmojiNode{TInner}"/> struct.
    /// </returns>
    public static TgEmojiNode<TInner> Apply(TInner defaultEmoji, TInner customEmojiId) =>
        new(defaultEmoji, customEmojiId);
}