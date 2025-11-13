using StringEnricher.Nodes;

namespace StringEnricher.Discord.Nodes.Markdown.Formatting;

/// <summary>
/// Represents single-line blockquote text in Discord markdown format.
/// Example: "> blockquote text"
/// </summary>
/// <typeparam name="TInner">
/// The type of the inner style that will be wrapped with blockquote syntax.
/// </typeparam>
public readonly struct BlockquoteNode<TInner> : INode
    where TInner : INode
{
    /// <summary>
    /// The prefix used for single-line blockquote in Discord markdown.
    /// </summary>
    public const string Prefix = "> ";

    private readonly TInner _innerText;

    /// <summary>
    /// Initializes a new instance of the <see cref="BlockquoteNode{TInner}"/> struct.
    /// </summary>
    /// <param name="inner">
    /// The inner style that will be wrapped with blockquote syntax.
    /// </param>
    public BlockquoteNode(TInner inner)
    {
        _innerText = inner;
    }

    /// <summary>
    /// Returns the string representation of the blockquote style in Discord markdown format.
    /// Note: This method allocates a new string in the most efficient way possible.
    /// Use this method when you finished all styling operations and need the final string.
    /// </summary>
    /// <returns>The created string representation</returns>
    public override string ToString() => string.Create(TotalLength, this, static (span, style) => style.CopyTo(span));

    /// <summary>
    /// Gets the length of the inner text without the blockquote syntax.
    /// </summary>
    public int InnerLength => _innerText.TotalLength;

    /// <inheritdoc />
    public int SyntaxLength => Prefix.Length;

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

        // Copy prefix
        Prefix.AsSpan().CopyTo(destination.Slice(pos, Prefix.Length));
        pos += Prefix.Length;

        // Copy inner text
        _innerText.CopyTo(destination.Slice(pos, InnerLength));

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

        var innerIndex = index - Prefix.Length;
        return _innerText.TryGetChar(innerIndex, out character);
    }

    /// <summary>
    /// Applies the blockquote style to the given inner style.
    /// </summary>
    /// <param name="innerStyle">
    /// The inner style to be wrapped with blockquote syntax.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="BlockquoteNode{TInner}"/> containing the inner style.
    /// </returns>
    public static BlockquoteNode<TInner> Apply(TInner innerStyle) => new(innerStyle);
}
