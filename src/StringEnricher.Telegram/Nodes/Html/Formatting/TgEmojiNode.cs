using System.Diagnostics;
using StringEnricher.Configuration;
using StringEnricher.Extensions;
using StringEnricher.Nodes;
using StringEnricher.Nodes.Shared;

namespace StringEnricher.Telegram.Nodes.Html.Formatting;

/// <summary>
/// Represents Telegram emoji text in HTML format.
/// Example: "&lt;tg-emoji emoji-id="id"&gt;emoji&lt;/tg-emoji&gt;"
/// </summary>
[DebuggerDisplay("{typeof(TgEmojiNode).Name,nq} Prefix={Prefix} InnerType={typeof(TInner).Name,nq} Suffix={Suffix}")]
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

    /// <inheritdoc />
    public string ToString(string? format, IFormatProvider? provider)
    {
        var length = this.GetSpanFormattableLength(
            nodeSettings: StringEnricherSettings.Extensions.StringBuilder,
            format: format,
            provider: provider
        );

        return string.Create(
            length: length,
            state: ValueTuple.Create(this, format, provider),
            action: static (span, state) =>
            {
                if (!state.Item1.TryFormat(span, out _, state.Item2, state.Item3))
                {
                    throw new InvalidOperationException("Formatting failed unexpectedly.");
                }
            }
        );
    }

    /// <inheritdoc />
    public bool TryFormat(
        Span<char> destination,
        out int charsWritten,
        ReadOnlySpan<char> format = default,
        IFormatProvider? provider = null
    )
    {
        charsWritten = 0;

        // Copy prefix
        if (!Prefix.AsSpan().TryCopyTo(destination.Slice(charsWritten, Prefix.Length)))
        {
            return false;
        }

        charsWritten += Prefix.Length;

        // Copy custom emoji ID
        var isCustomEmojiIdFormatSuccess = _customEmojiId.TryFormat(
            destination[charsWritten..],
            out var innerCharsWritten,
            format,
            provider
        );

        if (!isCustomEmojiIdFormatSuccess)
        {
            return false;
        }

        charsWritten += innerCharsWritten;

        // Copy separator
        if (!Separator.AsSpan().TryCopyTo(destination.Slice(charsWritten, Separator.Length)))
        {
            return false;
        }

        charsWritten += Separator.Length;

        // Copy default emoji
        var isDefaultEmojiFormatSuccess = _defaultEmoji.TryFormat(
            destination[charsWritten..],
            out innerCharsWritten,
            format,
            provider
        );

        if (!isDefaultEmojiFormatSuccess)
        {
            return false;
        }

        charsWritten += innerCharsWritten;

        // Copy suffix
        if (!Suffix.AsSpan().TryCopyTo(destination.Slice(charsWritten, Suffix.Length)))
        {
            return false;
        }

        charsWritten += Suffix.Length;

        return true;
    }

    /// <summary>
    /// Gets the length of the default and custom emoji.
    /// </summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public int InnerLength => _defaultEmoji.TotalLength + _customEmojiId.TotalLength;

    /// <summary>
    /// Gets the total length of the HTML Telegram emoji syntax.
    /// </summary>
    public int SyntaxLength => Prefix.Length + Separator.Length + Suffix.Length;

    /// <summary>
    /// Gets the total length of the formatted text.
    /// </summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public int TotalLength => SyntaxLength + InnerLength;

    /// <summary>
    /// Copies the formatted Telegram emoji text to the provided span.
    /// </summary>
    /// <param name="destination">The span to copy the formatted text into.</param>
    /// <returns>The total length of the formatted text.</returns>
    /// <exception cref="ArgumentException">Thrown if the destination span is too small.</exception>
    public int CopyTo(Span<char> destination)
    {
        var writtenChars = 0;
        Prefix.AsSpan().CopyTo(destination.Slice(writtenChars, Prefix.Length));
        writtenChars += Prefix.Length;

        writtenChars += _customEmojiId.CopyTo(destination[writtenChars..]);

        Separator.AsSpan().CopyTo(destination.Slice(writtenChars, Separator.Length));
        writtenChars += Separator.Length;

        writtenChars += _defaultEmoji.CopyTo(destination[writtenChars..]);

        Suffix.AsSpan().CopyTo(destination.Slice(writtenChars, Suffix.Length));
        writtenChars += Suffix.Length;

        return writtenChars;
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