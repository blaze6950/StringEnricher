namespace StringEnricher.Nodes.Html.Formatting;

/// <summary>
/// Provides methods to apply inline link styling in HTML format.
/// Example: "<a href="url">title</a>"
/// </summary>
public static class InlineLinkHtml
{
    /// <summary>
    /// Applies inline link style to the given title and URL.
    /// </summary>
    /// <param name="linkTitle">The link title to be wrapped with anchor HTML tags.</param>
    /// <param name="linkUrl">The link URL to be used in the anchor tag.</param>
    /// <returns>A new instance of <see cref="InlineLinkNode{TInner}"/> wrapping the provided title and URL.</returns>
    public static InlineLinkNode<PlainTextNode> Apply(string linkTitle, string linkUrl) =>
        InlineLinkNode<PlainTextNode>.Apply(linkTitle, linkUrl);

    /// <summary>
    /// Applies inline link style to the given styled title and URL.
    /// </summary>
    /// <param name="linkTitle">The styled link title to be wrapped with anchor HTML tags.</param>
    /// <param name="linkUrl">The styled link URL to be used in the anchor tag.</param>
    /// <typeparam name="T">The type of the inner style that implements <see cref="INode"/>.</typeparam>
    /// <returns>A new instance of <see cref="InlineLinkNode{TInner}"/> wrapping the provided styled title and URL.</returns>
    public static InlineLinkNode<T> Apply<T>(T linkTitle, T linkUrl) where T : INode =>
        InlineLinkNode<T>.Apply(linkTitle, linkUrl);
}

/// <summary>
/// Represents inline link text in HTML format.
/// Example: "<a href="url">title</a>"
/// </summary>
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
    private readonly TInner _linkUrl;

    /// <summary>
    /// Initializes a new instance of the <see cref="InlineLinkNode{TInner}"/> struct.
    /// </summary>
    /// <param name="linkTitle">The styled link title.</param>
    /// <param name="linkUrl">The styled link URL.</param>
    public InlineLinkNode(TInner linkTitle, TInner linkUrl)
    {
        _linkTitle = linkTitle;
        _linkUrl = linkUrl;
    }

    /// <inheritdoc/>
    public override string ToString() => string.Create(TotalLength, this, static (span, style) => style.CopyTo(span));

    /// <summary>
    /// Gets the length of the inner title and URL.
    /// </summary>
    public int InnerLength => _linkTitle.TotalLength + _linkUrl.TotalLength;

    /// <summary>
    /// Gets the total length of the HTML anchor syntax.
    /// </summary>
    public int SyntaxLength => Prefix.Length + LinkSeparator.Length + Suffix.Length;

    /// <summary>
    /// Gets the total length of the formatted text.
    /// </summary>
    public int TotalLength => SyntaxLength + InnerLength;

    /// <summary>
    /// Copies the formatted inline link text to the provided span.
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

        _linkUrl.CopyTo(destination.Slice(pos, _linkUrl.TotalLength));
        pos += _linkUrl.TotalLength;

        LinkSeparator.AsSpan().CopyTo(destination.Slice(pos, LinkSeparator.Length));
        pos += LinkSeparator.Length;

        _linkTitle.CopyTo(destination.Slice(pos, _linkTitle.TotalLength));
        pos += _linkTitle.TotalLength;

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

        if (index < _linkUrl.TotalLength)
        {
            return _linkUrl.TryGetChar(index, out character);
        }

        index -= _linkUrl.TotalLength;

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
    public static InlineLinkNode<TInner> Apply(TInner linkUrl, TInner linkTitle) => new(linkUrl, linkTitle);
}