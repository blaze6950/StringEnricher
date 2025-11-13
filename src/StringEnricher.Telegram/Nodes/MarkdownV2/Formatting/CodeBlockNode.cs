using StringEnricher.Nodes;

namespace StringEnricher.Telegram.Nodes.MarkdownV2.Formatting;

/// <summary>
/// Represents code block text in MarkdownV2 format.
/// Example: "```\ncode block\n```"
/// </summary>
/// <typeparam name="TInner">
/// The type of the inner style that will be wrapped with code block syntax.
/// </typeparam>
public readonly struct CodeBlockNode<TInner> : INode
    where TInner : INode
{
    /// <summary>
    /// The prefix for code block in MarkdownV2 format.
    /// </summary>
    public const string Prefix = "```\n";

    /// <summary>
    /// The suffix for code block in MarkdownV2 format.
    /// </summary>
    public const string Suffix = "\n```";

    private readonly TInner _innerCodeBlock;

    /// <summary>
    /// Initializes a new instance of the <see cref="CodeBlockNode{TInner}"/> struct.
    /// </summary>
    /// <param name="innerCodeBlock">
    /// The inner style to be wrapped with code block syntax.
    /// </param>
    public CodeBlockNode(TInner innerCodeBlock)
    {
        _innerCodeBlock = innerCodeBlock;
    }

    /// <summary>
    /// Returns the string representation of the code block style in MarkdownV2 format.
    /// Note: This method allocates a new string in the most efficient way possible.
    /// Use this method when you finished all styling operations and need the final string.
    /// </summary>
    /// <returns>The created string representation</returns>
    public override string ToString() => string.Create(TotalLength, this, static (span, style) => style.CopyTo(span));

    /// <summary>
    /// Gets the length of the inner code block content.
    /// </summary>
    public int InnerLength => _innerCodeBlock.TotalLength;

    /// <inheritdoc />
    public int SyntaxLength => Prefix.Length + Suffix.Length;

    /// <inheritdoc />
    public int TotalLength => SyntaxLength + InnerLength;

    /// <inheritdoc />
    public int CopyTo(Span<char> destination)
    {
        var writtenChars = 0;

        // Copy prefix
        Prefix.AsSpan().CopyTo(destination.Slice(writtenChars, Prefix.Length));
        writtenChars += Prefix.Length;

        // Copy inner code block
        writtenChars += _innerCodeBlock.CopyTo(destination[writtenChars..]);

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

        index -= Prefix.Length;

        if (index < InnerLength)
        {
            return _innerCodeBlock.TryGetChar(index, out character);
        }

        index -= InnerLength;

        character = Suffix[index];
        return true;
    }

    /// <summary>
    /// Applies the code block style to the given inner code block.
    /// </summary>
    /// <param name="innerCodeBlock">
    /// The inner style to be wrapped with code block syntax.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="CodeBlockNode{TInner}"/> wrapping the provided inner style.
    /// </returns>
    public static CodeBlockNode<TInner> Apply(TInner innerCodeBlock) => new(innerCodeBlock);
}