using System.Diagnostics;
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
        Prefix.AsSpan().CopyTo(destination.Slice(writtenChars, Prefix.Length));
        writtenChars += Prefix.Length;

        _linkUrl.CopyTo(destination.Slice(writtenChars, _linkUrl.Length));
        writtenChars += _linkUrl.Length;

        LinkSeparator.AsSpan().CopyTo(destination.Slice(writtenChars, LinkSeparator.Length));
        writtenChars += LinkSeparator.Length;

        writtenChars += _linkTitle.CopyTo(destination[writtenChars..]);

        Suffix.AsSpan().CopyTo(destination.Slice(writtenChars, Suffix.Length));
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
    public static InlineLinkNode<TInner> Apply(TInner linkTitle, string linkUrl) => new(linkTitle, linkUrl);
}