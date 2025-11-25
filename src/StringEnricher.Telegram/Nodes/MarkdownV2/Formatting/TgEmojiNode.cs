using System.Diagnostics;
using System.Runtime.CompilerServices;
using StringEnricher.Configuration;
using StringEnricher.Extensions;
using StringEnricher.Nodes;
using StringEnricher.Nodes.Shared;

namespace StringEnricher.Telegram.Nodes.MarkdownV2.Formatting;

/// <summary>
/// Represents TG emoji text in MarkdownV2 format.
/// Example: "![👍](tg://emoji?id=123456)"
/// </summary>
[DebuggerDisplay("{typeof(TgEmojiNode).Name,nq} Prefix={Prefix} InnerType={typeof(TInner).Name,nq} Suffix={Suffix}")]
public readonly struct TgEmojiNode : INode
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

    private readonly PlainTextNode _defaultEmoji;
    private readonly LongNode _customEmojiId;

    /// <summary>
    /// Initializes a new instance of the <see cref="TgEmojiNode"/> struct.
    /// </summary>
    /// <param name="defaultEmoji">
    /// The default emoji to display if the custom emoji is not available.
    /// </param>
    /// <param name="customEmojiId">
    /// The unique identifier of the custom emoji.
    /// </param>
    public TgEmojiNode(PlainTextNode defaultEmoji, LongNode customEmojiId)
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
        if (!Prefix.AsSpan().TryCopyTo(destination))
        {
            return false;
        }

        charsWritten += Prefix.Length;

        // Copy default emoji
        var isDefaultEmojiFormatSuccess = _defaultEmoji.TryFormat(
            destination.SliceSafe(charsWritten),
            out var innerCharsWritten,
            format,
            provider
        );

        if (!isDefaultEmojiFormatSuccess)
        {
            return false;
        }

        charsWritten += innerCharsWritten;

        // Copy separator
        if (!Separator.AsSpan().TryCopyTo(destination.SliceSafe(charsWritten)))
        {
            return false;
        }

        charsWritten += Separator.Length;

        // Copy custom emoji ID
        var isCustomEmojiIdFormatSuccess = _customEmojiId.TryFormat(
            destination.SliceSafe(charsWritten),
            out var customEmojiIdCharsWritten,
            format,
            provider
        );

        if (!isCustomEmojiIdFormatSuccess)
        {
            return false;
        }

        charsWritten += customEmojiIdCharsWritten;

        // Copy suffix
        if (!Suffix.AsSpan().TryCopyTo(destination.SliceSafe(charsWritten)))
        {
            return false;
        }

        charsWritten += Suffix.Length;

        return true;
    }

    /// <summary>
    /// Gets the length of the inner styled text.
    /// </summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public int InnerLength => _defaultEmoji.TotalLength + _customEmojiId.TotalLength;

    /// <inheritdoc />
    public int SyntaxLength => Prefix.Length + Separator.Length + Suffix.Length;

    /// <inheritdoc />
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public int TotalLength => SyntaxLength + InnerLength;

    /// <inheritdoc />
    public int CopyTo(Span<char> destination)
    {
        var writtenChars = 0;

        // Copy Prefix
        Prefix.AsSpan().CopyTo(destination);
        writtenChars += Prefix.Length;

        // Copy Default Emoji
        writtenChars += _defaultEmoji.CopyTo(destination[writtenChars..]);

        // Copy Separator
        Separator.AsSpan().CopyTo(destination[writtenChars..]);
        writtenChars += Separator.Length;

        // Copy Custom Emoji ID
        writtenChars += _customEmojiId.CopyTo(destination[writtenChars..]);

        // Copy Suffix
        Suffix.AsSpan().CopyTo(destination[writtenChars..]);
        writtenChars += Suffix.Length;

        return writtenChars;
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
    /// Creates a new instance of the <see cref="TgEmojiNode"/> struct.
    /// </summary>
    /// <param name="defaultEmoji">
    /// The default emoji to display if the custom emoji is not available.
    /// </param>
    /// <param name="customEmojiId">
    /// The unique identifier of the custom emoji.
    /// </param>
    /// <returns>
    /// The created instance of the <see cref="TgEmojiNode"/> struct.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TgEmojiNode Apply(PlainTextNode defaultEmoji, LongNode customEmojiId) => new(defaultEmoji, customEmojiId);
}