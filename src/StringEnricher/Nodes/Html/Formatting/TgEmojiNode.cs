using StringEnricher.Nodes.Shared;

namespace StringEnricher.Nodes.Html.Formatting;

/// <summary>
/// Represents Telegram emoji text in HTML format.
/// Example: "&lt;tg-emoji emoji-id="id"&gt;emoji&lt;/tg-emoji&gt;"
/// </summary>
public readonly struct TgEmojiNode : INode
{
    /// <summary>
    /// The opening Telegram emoji tag and emoji-id attribute.
    /// </summary>
    public const string Prefix = "<tg-emoji emoji-id=\"";

    /// <summary>
    /// The separator between the emoji-id and the emoji.
    /// </summary>
    public const string Separator = "\">";

    /// <summary>
    /// The closing Telegram emoji tag.
    /// </summary>
    public const string Suffix = "</tg-emoji>";

    private readonly PlainTextNode _defaultEmoji;
    private readonly LongNode _customEmojiId;

    /// <summary>
    /// Initializes a new instance of the <see cref="TgEmojiNode"/> struct.
    /// </summary>
    /// <param name="defaultEmoji">The default emoji.</param>
    /// <param name="customEmojiId">The custom emoji ID.</param>
    public TgEmojiNode(PlainTextNode defaultEmoji, LongNode customEmojiId)
    {
        _defaultEmoji = defaultEmoji;
        _customEmojiId = customEmojiId;
    }

    /// <inheritdoc/>
    public override string ToString() => string.Create(TotalLength, this, static (span, style) => style.CopyTo(span));

    /// <summary>
    /// Gets the length of the default and custom emoji.
    /// </summary>
    public int InnerLength => _defaultEmoji.TotalLength + _customEmojiId.TotalLength;

    /// <summary>
    /// Gets the total length of the HTML Telegram emoji syntax.
    /// </summary>
    public int SyntaxLength => Prefix.Length + Separator.Length + Suffix.Length;

    /// <summary>
    /// Gets the total length of the formatted text.
    /// </summary>
    public int TotalLength => SyntaxLength + InnerLength;

    /// <summary>
    /// Copies the formatted Telegram emoji text to the provided span.
    /// </summary>
    /// <param name="destination">The span to copy the formatted text into.</param>
    /// <returns>The total length of the formatted text.</returns>
    /// <exception cref="ArgumentException">Thrown if the destination span is too small.</exception>
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

        _customEmojiId.CopyTo(destination.Slice(pos, _customEmojiId.TotalLength));
        pos += _customEmojiId.TotalLength;

        Separator.AsSpan().CopyTo(destination.Slice(pos, Separator.Length));
        pos += Separator.Length;

        _defaultEmoji.CopyTo(destination.Slice(pos, _defaultEmoji.TotalLength));
        pos += _defaultEmoji.TotalLength;

        Suffix.AsSpan().CopyTo(destination.Slice(pos, Suffix.Length));

        return totalLength;
    }

    /// <inheritdoc />
    public bool TryGetChar(int index, out char character)
    {
        var totalLength = TotalLength;
        if (index < 0 || index >= totalLength)
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

        if (index < _customEmojiId.TotalLength)
        {
            return _customEmojiId.TryGetChar(index, out character);
        }

        index -= _customEmojiId.TotalLength;

        if (index < Separator.Length)
        {
            character = Separator[index];
            return true;
        }

        index -= Separator.Length;

        if (index < _defaultEmoji.TotalLength)
        {
            return _defaultEmoji.TryGetChar(index, out character);
        }

        index -= _defaultEmoji.TotalLength;

        // At this point, index must be within the Suffix
        character = Suffix[index];
        return true;
    }

    /// <summary>
    /// Applies Telegram emoji style to the given default and custom emoji.
    /// </summary>
    /// <param name="defaultEmoji">The default emoji to be wrapped with Telegram emoji HTML tags.</param>
    /// <param name="customEmoji">The custom emoji ID to be used in the emoji-id attribute.</param>
    /// <returns>A new instance of <see cref="TgEmojiNode"/> wrapping the provided emoji and ID.</returns>
    public static TgEmojiNode Apply(PlainTextNode defaultEmoji, LongNode customEmoji) => new(defaultEmoji, customEmoji);
}