using System.Diagnostics;
using StringEnricher.Nodes;

namespace StringEnricher.Discord.Nodes.Markdown.Formatting;

/// <summary>
/// Represents multi-line blockquote text in Discord markdown format.
/// Example: ">>> multi-line quote text"
/// </summary>
/// <typeparam name="TInner">
/// The type of the inner style that will be wrapped with multi-line quote syntax.
/// </typeparam>
[DebuggerDisplay("{typeof(MultilineQuoteNode).Name,nq} Prefix={Prefix} InnerType={typeof(TInner).Name,nq}")]
public readonly struct MultilineQuoteNode<TInner> : INode
    where TInner : INode
{
    /// <summary>
    /// The prefix used for multi-line blockquote in Discord markdown.
    /// </summary>
    public const string Prefix = ">>> ";

    private readonly TInner _innerText;

    /// <summary>
    /// Initializes a new instance of the <see cref="MultilineQuoteNode{TInner}"/> struct.
    /// </summary>
    /// <param name="inner">
    /// The inner style that will be wrapped with multi-line quote syntax.
    /// </param>
    public MultilineQuoteNode(TInner inner)
    {
        _innerText = inner;
    }

    /// <summary>
    /// Returns the string representation of the multi-line quote style in Discord markdown format.
    /// Note: This method allocates a new string in the most efficient way possible.
    /// Use this method when you finished all styling operations and need the final string.
    /// </summary>
    /// <returns>The created string representation</returns>
    public override string ToString() => string.Create(TotalLength, this, static (span, style) => style.CopyTo(span));

    /// <summary>
    /// Gets the length of the inner text without the multi-line quote syntax.
    /// </summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public int InnerLength => _innerText.TotalLength;

    /// <inheritdoc />
    public int SyntaxLength => Prefix.Length;

    /// <inheritdoc />
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public int TotalLength => SyntaxLength + InnerLength;

    /// <inheritdoc />
    public int CopyTo(Span<char> destination)
    {
        var writtenChars = 0;

        // Copy prefix
        Prefix.AsSpan().CopyTo(destination.Slice(writtenChars, Prefix.Length));
        writtenChars += Prefix.Length;

        // Copy inner text
        writtenChars += _innerText.CopyTo(destination[writtenChars..]);;

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

        var innerIndex = index - Prefix.Length;
        return _innerText.TryGetChar(innerIndex, out character);
    }

    /// <summary>
    /// Applies the multi-line quote style to the given inner style.
    /// </summary>
    /// <param name="innerStyle">
    /// The inner style to be wrapped with multi-line quote syntax.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="MultilineQuoteNode{TInner}"/> containing the inner style.
    /// </returns>
    public static MultilineQuoteNode<TInner> Apply(TInner innerStyle) => new(innerStyle);
}
