using StringEnricher.Nodes;

namespace StringEnricher.Discord.Nodes.Markdown.Formatting;

/// <summary>
/// Represents header text in Discord markdown format.
/// Example: "# Header", "## Subheader", "### Smaller header"
/// </summary>
/// <typeparam name="TInner">
/// The type of the inner style that will be wrapped with header syntax.
/// </typeparam>
public readonly struct HeaderNode<TInner> : INode
    where TInner : INode
{
    private readonly TInner _innerText;
    private readonly int _level;

    /// <summary>
    /// Initializes a new instance of the <see cref="HeaderNode{TInner}"/> struct.
    /// </summary>
    /// <param name="inner">
    /// The inner style that will be wrapped with header syntax.
    /// </param>
    /// <param name="level">
    /// The header level (1-3, where 1 is #, 2 is ##, 3 is ###).
    /// </param>
    public HeaderNode(TInner inner, int level = 1)
    {
        if (level < 1 || level > 3)
        {
            throw new ArgumentOutOfRangeException(nameof(level), "Header level must be between 1 and 3.");
        }
        
        _innerText = inner;
        _level = level;
    }

    /// <summary>
    /// Returns the string representation of the header style in Discord markdown format.
    /// Note: This method allocates a new string in the most efficient way possible.
    /// Use this method when you finished all styling operations and need the final string.
    /// </summary>
    /// <returns>The created string representation</returns>
    public override string ToString() => string.Create(TotalLength, this, static (span, style) => style.CopyTo(span));

    /// <summary>
    /// Gets the length of the inner text without the header syntax.
    /// </summary>
    public int InnerLength => _innerText.TotalLength;

    /// <inheritdoc />
    public int SyntaxLength => _level + 1; // '#' characters + space

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

        // Copy prefix (# characters)
        for (var i = 0; i < _level; i++)
        {
            destination[pos++] = '#';
        }
        
        // Add space
        destination[pos++] = ' ';

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

        if (index < _level)
        {
            character = '#';
            return true;
        }

        if (index == _level)
        {
            character = ' ';
            return true;
        }

        var innerIndex = index - SyntaxLength;
        return _innerText.TryGetChar(innerIndex, out character);
    }

    /// <summary>
    /// Applies the header style to the given inner style.
    /// </summary>
    /// <param name="innerStyle">
    /// The inner style to be wrapped with header syntax.
    /// </param>
    /// <param name="level">
    /// The header level (1-3, where 1 is #, 2 is ##, 3 is ###).
    /// </param>
    /// <returns>
    /// A new instance of <see cref="HeaderNode{TInner}"/> containing the inner style.
    /// </returns>
    public static HeaderNode<TInner> Apply(TInner innerStyle, int level = 1) => new(innerStyle, level);
}
