namespace StringEnricher.Nodes.Html;

/// <summary>
/// Provides methods to apply code block styling in HTML format.
/// Example: "<pre>code block</pre>"
/// </summary>
public static class CodeBlockHtml
{
    /// <summary>
    /// Applies code block style to the given text.
    /// </summary>
    /// <param name="codeBlock">
    /// The text to be wrapped with code block HTML tags.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="CodeBlockNode{TInner}"/> wrapping the provided text.
    /// </returns>
    public static CodeBlockNode<PlainTextNode> Apply(string codeBlock) =>
        CodeBlockNode<PlainTextNode>.Apply(codeBlock);

    /// <summary>
    /// Applies code block style to the given style.
    /// </summary>
    /// <param name="codeBlock">
    /// The inner style to be wrapped with code block HTML tags.
    /// </param>
    /// <typeparam name="T">
    /// The type of the inner style that implements <see cref="INode"/>.
    /// </typeparam>
    /// <returns>
    /// A new instance of <see cref="CodeBlockNode{TInner}"/> wrapping the provided inner style.
    /// </returns>
    public static CodeBlockNode<T> Apply<T>(T codeBlock) where T : INode =>
        CodeBlockNode<T>.Apply(codeBlock);
}

/// <summary>
/// Represents code block text in HTML format.
/// Example: "<pre>code block</pre>"
/// </summary>
public readonly struct CodeBlockNode<TInner> : INode
    where TInner : INode
{
    /// <summary>
    /// The opening code block tag.
    /// </summary>
    public const string Prefix = "<pre>";
    /// <summary>
    /// The closing code block tag.
    /// </summary>
    public const string Suffix = "</pre>";

    private readonly TInner _innerCodeBlock;

    /// <summary>
    /// Initializes a new instance of the <see cref="CodeBlockNode{TInner}"/> struct.
    /// </summary>
    /// <param name="innerCodeBlock">The inner style to be wrapped with code block HTML tags.</param>
    public CodeBlockNode(TInner innerCodeBlock)
    {
        _innerCodeBlock = innerCodeBlock;
    }

    /// <inheritdoc/>
    public override string ToString() => string.Create(TotalLength, this, static (span, style) => style.CopyTo(span));

    /// <summary>
    /// Gets the length of the inner code block.
    /// </summary>
    public int InnerLength => _innerCodeBlock.TotalLength;
    /// <summary>
    /// Gets the total length of the HTML code block syntax.
    /// </summary>
    public int SyntaxLength => Prefix.Length + Suffix.Length;
    /// <summary>
    /// Gets the total length of the formatted text.
    /// </summary>
    public int TotalLength => SyntaxLength + InnerLength;

    /// <summary>
    /// Copies the formatted code block text to the provided span.
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

        _innerCodeBlock.CopyTo(destination.Slice(pos, InnerLength));
        pos += InnerLength;

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

        if (index < InnerLength)
        {
            return _innerCodeBlock.TryGetChar(index, out character);
        }

        index -= InnerLength;

        character = Suffix[index];
        return true;
    }

    public static CodeBlockNode<TInner> Apply(TInner innerCodeBlock) => new(innerCodeBlock);
}