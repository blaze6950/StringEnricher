using System.Diagnostics;
using StringEnricher.Nodes;

namespace StringEnricher.Discord.Nodes.Markdown.Formatting;

/// <summary>
/// Represents bold text in Discord markdown format.
/// Example: "**bold text**"
/// </summary>
/// <typeparam name="TInner">
/// The type of the inner style that will be wrapped with bold syntax.
/// </typeparam>
[DebuggerDisplay("{typeof(BoldNode).Name,nq} Prefix={Prefix} InnerType={typeof(TInner).Name,nq} Suffix={Suffix}")]
public readonly struct BoldNode<TInner> : INode
    where TInner : INode
{
    /// <summary>
    /// The prefix and suffix used for bold styling in Discord markdown.
    /// </summary>
    public const string Prefix = "**";

    /// <summary>
    /// The suffix used for bold styling in Discord markdown.
    /// </summary>
    public const string Suffix = "**";

    private readonly TInner _innerText;

    /// <summary>
    /// Initializes a new instance of the <see cref="BoldNode{TInner}"/> struct.
    /// </summary>
    /// <param name="inner">The inner style that will be wrapped with bold syntax</param>
    public BoldNode(TInner inner)
    {
        _innerText = inner;
    }

    /// <summary>
    /// Returns the string representation of the bold style in Discord markdown format.
    /// Note: This method allocates a new string in the most efficient way possible.
    /// Use this method when you finished all styling operations and need the final string.
    /// </summary>
    /// <returns>The created string representation</returns>
    public override string ToString() => string.Create(TotalLength, this, static (span, style) => style.CopyTo(span));

    /// <summary>
    /// Gets the length of the inner text.
    /// </summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public int InnerLength => _innerText.TotalLength;

    /// <inheritdoc />
    public int SyntaxLength => Prefix.Length + Suffix.Length;

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
        writtenChars += _innerText.CopyTo(destination[writtenChars..]);

        // Copy suffix
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

        var innerIndex = index - Prefix.Length;
        if (innerIndex < InnerLength)
        {
            return _innerText.TryGetChar(innerIndex, out character);
        }

        var suffixIndex = innerIndex - InnerLength;
        character = Suffix[suffixIndex];

        return true;
    }

    /// <summary>
    /// Applies bold style to the given inner style.
    /// </summary>
    /// <param name="innerStyle">
    /// The inner style to be wrapped with bold syntax.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="BoldNode{TInner}"/> wrapping the provided inner style.
    /// </returns>
    public static BoldNode<TInner> Apply(TInner innerStyle) => new(innerStyle);
}