namespace StringEnricher.Nodes.MarkdownV2.Formatting;

/// <summary>
/// Provides methods to create inline link styles in MarkdownV2 format.
/// Example: "[test](https://example.com)"
/// </summary>
public static class InlineLinkMarkdownV2
{
    /// <summary>
    /// Applies the inline link style to the given link title and link URL using plain text style.
    /// </summary>
    /// <param name="linkTitle">
    /// The link title as a plain text string.
    /// </param>
    /// <param name="linkUrl">
    /// The link URL as a plain text string.
    /// </param>
    /// <returns>
    /// A new instance of the <see cref="InlineLinkNode{TInner}"/> struct
    /// with the specified link title and link URL.
    /// </returns>
    public static InlineLinkNode<PlainTextNode> Apply(string linkTitle, string linkUrl) =>
        InlineLinkNode<PlainTextNode>.Apply(linkTitle, linkUrl);

    public static InlineLinkNode<T> Apply<T>(T linkTitle, T linkUrl) where T : INode =>
        InlineLinkNode<T>.Apply(linkTitle, linkUrl);
}

/// <summary>
/// Represents inline link text in MarkdownV2 format.
/// Example: "[test](https://example.com)"
/// </summary>
/// <typeparam name="TInner">
/// The type of the inner style that will be wrapped with inline link syntax.
/// </typeparam>
public readonly struct InlineLinkNode<TInner> : INode
    where TInner : INode
{
    /// <summary>
    /// The prefix for the inline link style in MarkdownV2 format.
    /// </summary>
    public const string Prefix = "[";

    /// <summary>
    /// The separator between the link title and link URL in MarkdownV2 format.
    /// </summary>
    public const string LinkSeparator = "](";

    /// <summary>
    /// The suffix for the inline link style in MarkdownV2 format.
    /// </summary>
    public const string Suffix = ")";

    private readonly TInner _linkTitle;
    private readonly TInner _linkUrl;

    /// <summary>
    /// Initializes a new instance of the <see cref="InlineLinkNode{TInner}"/> struct.
    /// </summary>
    /// <param name="linkTitle">
    /// The inner style representing the link title.
    /// </param>
    /// <param name="linkUrl">
    /// The inner style representing the link URL.
    /// </param>
    public InlineLinkNode(TInner linkTitle, TInner linkUrl)
    {
        _linkTitle = linkTitle;
        _linkUrl = linkUrl;
    }

    /// <summary>
    /// Returns the string representation of the inline link style in MarkdownV2 format.
    /// Note: This method allocates a new string in the most efficient way possible.
    /// Use this method when you finished all styling operations and need the final string.
    /// </summary>
    /// <returns>The created string representation</returns>
    public override string ToString() => string.Create(TotalLength, this, static (span, style) => style.CopyTo(span));

    /// <summary>
    /// Gets the length of the inner content (link title + link URL).
    /// </summary>
    public int InnerLength => _linkTitle.TotalLength + _linkUrl.TotalLength;

    /// <inheritdoc />
    public int SyntaxLength => Prefix.Length + LinkSeparator.Length + Suffix.Length;

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

        // Copy Link Title
        _linkTitle.CopyTo(destination.Slice(pos, _linkTitle.TotalLength));
        pos += _linkTitle.TotalLength;

        // Copy Link Separator
        LinkSeparator.AsSpan().CopyTo(destination.Slice(pos, LinkSeparator.Length));
        pos += LinkSeparator.Length;

        // Copy Link URL
        _linkUrl.CopyTo(destination.Slice(pos, _linkUrl.TotalLength));
        pos += _linkUrl.TotalLength;

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

        if (index < _linkTitle.TotalLength)
        {
            return _linkTitle.TryGetChar(index, out character);
        }

        index -= _linkTitle.TotalLength;

        if (index < LinkSeparator.Length)
        {
            character = LinkSeparator[index];
            return true;
        }

        index -= LinkSeparator.Length;

        if (index < _linkUrl.TotalLength)
        {
            return _linkUrl.TryGetChar(index, out character);
        }

        index -= _linkUrl.TotalLength;

        // At this point, index must be within the Suffix
        character = Suffix[index];
        return true;
    }

    /// <summary>
    /// Applies the inline link style to the given link title and link URL.
    /// /// </summary>
    /// <param name="linkTitle">
    /// The inner style representing the link title.
    /// </param>
    /// <param name="linkUrl">
    /// The inner style representing the link URL.
    /// </param>
    /// <returns>
    /// A new instance of the <see cref="InlineLinkNode{TInner}"/> struct.
    /// </returns>
    public static InlineLinkNode<TInner> Apply(TInner linkTitle, TInner linkUrl) =>
        new(linkTitle, linkUrl);
}