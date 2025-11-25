using System.Diagnostics;
using System.Runtime.CompilerServices;
using StringEnricher.Configuration;
using StringEnricher.Extensions;
using StringEnricher.Nodes;

namespace StringEnricher.Telegram.Nodes.Html.Formatting;

/// <summary>
/// Represents inline link text in HTML format.
/// Example: "&lt;a href="url"&gt;title&lt;/a&gt;"
/// </summary>
[DebuggerDisplay("{typeof(InlineLinkNode).Name,nq} Prefix={Prefix} InnerType={typeof(TInner).Name,nq} Suffix={Suffix}")]
public readonly struct InlineLinkNode<TInner> : INode
    where TInner : INode
{
    /// <summary>
    /// The opening anchor tag and href attribute.
    /// </summary>
    public const string Prefix = "<a href=\"";

    /// <summary>
    /// The separator between the URL and the link title.
    /// </summary>
    public const string LinkSeparator = "\">";

    /// <summary>
    /// The closing anchor tag.
    /// </summary>
    public const string Suffix = "</a>";

    private readonly TInner _linkTitle;
    private readonly string _linkUrl;

    /// <summary>
    /// Initializes a new instance of the <see cref="InlineLinkNode{TInner}"/> struct.
    /// </summary>
    /// <param name="linkTitle">The styled link title.</param>
    /// <param name="linkUrl">The styled link URL.</param>
    public InlineLinkNode(TInner linkTitle, string linkUrl)
    {
        _linkTitle = linkTitle;
        _linkUrl = linkUrl;
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
        if (!Prefix.AsSpan().TryCopyTo(destination))
        {
            return false;
        }

        charsWritten += Prefix.Length;

        // Copy inner text
        var isInnerTextFormatSuccess = _linkUrl.TryCopyTo(
            destination.SliceSafe(charsWritten)
        );

        if (!isInnerTextFormatSuccess)
        {
            return false;
        }

        charsWritten += _linkUrl.Length;

        // Copy link separator
        if (!LinkSeparator.AsSpan().TryCopyTo(destination.SliceSafe(charsWritten)))
        {
            return false;
        }

        charsWritten += LinkSeparator.Length;

        // Copy link title
        var isLinkTitleFormatSuccess = _linkTitle.TryFormat(
            destination.SliceSafe(charsWritten),
            out var innerCharsWritten,
            format,
            provider
        );

        if (!isLinkTitleFormatSuccess)
        {
            return false;
        }

        charsWritten += innerCharsWritten;

        // Copy suffix
        if (!Suffix.AsSpan().TryCopyTo(destination.SliceSafe(charsWritten)))
        {
            return false;
        }

        charsWritten += Suffix.Length;

        return true;
    }

    /// <summary>
    /// Gets the length of the inner title and URL.
    /// </summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public int InnerLength => _linkTitle.TotalLength + _linkUrl.Length;

    /// <summary>
    /// Gets the total length of the HTML anchor syntax.
    /// </summary>
    public int SyntaxLength => Prefix.Length + LinkSeparator.Length + Suffix.Length;

    /// <summary>
    /// Gets the total length of the formatted text.
    /// </summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public int TotalLength => SyntaxLength + InnerLength;

    /// <summary>
    /// Copies the formatted inline link text to the provided span.
    /// </summary>
    /// <param name="destination">The span to copy the formatted text into.</param>
    /// <returns>The total length of the formatted text.</returns>
    /// <exception cref="ArgumentException">Thrown if the destination span is too small.</exception>
    public int CopyTo(Span<char> destination)
    {
        var writtenChars = 0;
        Prefix.AsSpan().CopyTo(destination);
        writtenChars += Prefix.Length;

        _linkUrl.CopyTo(destination[writtenChars..]);
        writtenChars += _linkUrl.Length;

        LinkSeparator.AsSpan().CopyTo(destination[writtenChars..]);
        writtenChars += LinkSeparator.Length;

        writtenChars += _linkTitle.CopyTo(destination[writtenChars..]);

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

        if (index < _linkUrl.Length)
        {
            character = _linkUrl[index];
            return true;
        }

        index -= _linkUrl.Length;

        if (index < LinkSeparator.Length)
        {
            character = LinkSeparator[index];
            return true;
        }

        index -= LinkSeparator.Length;

        if (index < _linkTitle.TotalLength)
        {
            return _linkTitle.TryGetChar(index, out character);
        }

        index -= _linkTitle.TotalLength;

        // At this point, index must be within the Suffix
        character = Suffix[index];
        return true;
    }

    /// <summary>
    /// Applies inline link style to the given link URL and title.
    /// </summary>
    /// <param name="linkUrl">The link URL to be wrapped with inline link HTML tags.</param>
    /// <param name="linkTitle">The link title to be used as the link text.</param>
    /// <returns>A new instance of <see cref="InlineLinkNode{TInner}"/> wrapping the provided URL and title.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static InlineLinkNode<TInner> Apply(TInner linkTitle, string linkUrl) => new(linkTitle, linkUrl);
}