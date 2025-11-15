using System.Diagnostics;
using StringEnricher.Nodes;

namespace StringEnricher.Discord.Nodes.Markdown.Formatting;

/// <summary>
/// Represents inline link text in Discord markdown format.
/// Example: "[test](https://example.com)"
/// </summary>
/// <typeparam name="TInner">
/// The type of the inner style that will be wrapped with inline link syntax.
/// </typeparam>
[DebuggerDisplay("{typeof(InlineLinkNode).Name,nq} Prefix={Prefix} InnerType={typeof(TInner).Name,nq} LinkSeparator={LinkSeparator} Suffix={Suffix}")]
public readonly struct InlineLinkNode<TInner> : INode
    where TInner : INode
{
    /// <summary>
    /// The prefix for the inline link style in Discord markdown format.
    /// </summary>
    public const string Prefix = "[";

    /// <summary>
    /// The separator between the link title and link URL in Discord markdown format.
    /// </summary>
    public const string LinkSeparator = "](";

    /// <summary>
    /// The suffix for the inline link style in Discord markdown format.
    /// </summary>
    public const string Suffix = ")";

    private readonly TInner _linkTitle;
    private readonly string _linkUrl;

    /// <summary>
    /// Initializes a new instance of the <see cref="InlineLinkNode{TInner}"/> struct.
    /// </summary>
    /// <param name="linkTitle">
    /// The inner style representing the link title.
    /// </param>
    /// <param name="linkUrl">
    /// The inner style representing the link URL.
    /// </param>
    public InlineLinkNode(TInner linkTitle, string linkUrl)
    {
        _linkTitle = linkTitle;
        _linkUrl = linkUrl;
    }

    /// <summary>
    /// Returns the string representation of the inline link style in Discord markdown format.
    /// Note: This method allocates a new string in the most efficient way possible.
    /// Use this method when you finished all styling operations and need the final string.
    /// </summary>
    /// <returns>The created string representation</returns>
    public override string ToString() => string.Create(TotalLength, this, static (span, style) => style.CopyTo(span));

    /// <summary>
    /// Gets the length of the inner content (link title + link URL).
    /// </summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public int InnerLength => _linkTitle.TotalLength + _linkUrl.Length;

    /// <inheritdoc />
    public int SyntaxLength => Prefix.Length + LinkSeparator.Length + Suffix.Length;

    /// <inheritdoc />
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public int TotalLength => SyntaxLength + InnerLength;

    /// <inheritdoc />
    public int CopyTo(Span<char> destination)
    {
        var writtenChars = 0;

        // Copy Prefix
        Prefix.AsSpan().CopyTo(destination.Slice(writtenChars, Prefix.Length));
        writtenChars += Prefix.Length;

        // Copy Link Title
        writtenChars += _linkTitle.CopyTo(destination[writtenChars..]);

        // Copy Link Separator
        LinkSeparator.AsSpan().CopyTo(destination.Slice(writtenChars, LinkSeparator.Length));
        writtenChars += LinkSeparator.Length;

        // Copy Link URL
        _linkUrl.CopyTo(destination.Slice(writtenChars, _linkUrl.Length));
        writtenChars += _linkUrl.Length;

        // Copy Suffix
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

        if (index < _linkUrl.Length)
        {
            character = _linkUrl[index];
            return true;
        }

        index -= _linkUrl.Length;

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
    /// The link URL.
    /// </param>
    /// <returns>
    /// A new instance of the <see cref="InlineLinkNode{TInner}"/> struct.
    /// </returns>
    public static InlineLinkNode<TInner> Apply(TInner linkTitle, string linkUrl) =>
        new(linkTitle, linkUrl);
}