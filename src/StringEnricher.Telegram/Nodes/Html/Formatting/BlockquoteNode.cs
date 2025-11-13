using StringEnricher.Nodes;

namespace StringEnricher.Telegram.Nodes.Html.Formatting;

/// <summary>
/// Represents blockquote text in HTML format.
/// Example: "&lt;blockquote&gt;quoted text&lt;/blockquote&gt;"
/// </summary>
public readonly struct BlockquoteNode<TInner> : INode
    where TInner : INode
{
    /// <summary>
    /// The opening blockquote tag.
    /// </summary>
    public const string Prefix = "<blockquote>";
    /// <summary>
    /// The closing blockquote tag.
    /// </summary>
    public const string Suffix = "</blockquote>";

    private readonly TInner _innerText;

    /// <summary>
    /// Initializes a new instance of the <see cref="BlockquoteNode{TInner}"/> struct.
    /// </summary>
    /// <param name="inner">The inner style to be wrapped with blockquote HTML tags.</param>
    public BlockquoteNode(TInner inner)
    {
        _innerText = inner;
    }

    /// <inheritdoc/>
    public override string ToString() => string.Create(TotalLength, this, static (span, style) => style.CopyTo(span));

    /// <summary>
    /// Gets the length of the inner text.
    /// </summary>
    public int InnerLength => _innerText.TotalLength;
    /// <summary>
    /// Gets the total length of the HTML blockquote syntax.
    /// </summary>
    public int SyntaxLength => Prefix.Length + Suffix.Length;
    /// <summary>
    /// Gets the total length of the formatted text.
    /// </summary>
    public int TotalLength => SyntaxLength + InnerLength;

    /// <summary>
    /// Copies the formatted blockquote text to the provided span.
    /// </summary>
    /// <param name="destination">The span to copy the formatted text into.</param>
    /// <returns>The total length of the formatted text.</returns>
    /// <exception cref="ArgumentException">Thrown if the destination span is too small.</exception>
    public int CopyTo(Span<char> destination)
    {
        var writtenChars = 0;
        Prefix.AsSpan().CopyTo(destination.Slice(writtenChars, Prefix.Length));
        writtenChars += Prefix.Length;

        writtenChars += _innerText.CopyTo(destination[writtenChars..]);

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

        if (index < InnerLength)
        {
            return _innerText.TryGetChar(index, out character);
        }

        index -= InnerLength;

        character = Suffix[index];
        return true;
    }

    public static BlockquoteNode<TInner> Apply(TInner innerStyle) => new(innerStyle);
}