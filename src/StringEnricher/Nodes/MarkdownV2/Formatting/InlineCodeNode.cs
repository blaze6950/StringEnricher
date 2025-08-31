namespace StringEnricher.Nodes.MarkdownV2.Formatting;

/// <summary>
/// Provides methods to create inline code styles in MarkdownV2 format.
/// Example: "`inline code`"
/// </summary>
public static class InlineCodeMarkdownV2
{
    /// <summary>
    /// Applies inline code style to the given text
    /// </summary>
    /// <param name="text">
    /// The text to be styled as inline code.
    /// </param>
    /// <returns>
    /// An instance of <see cref="InlineCodeNode{TInner}"/> containing the provided text styled as inline code.
    /// </returns>
    public static InlineCodeNode<PlainTextNode> Apply(string text) =>
        InlineCodeNode<PlainTextNode>.Apply(text);

    /// <summary>
    /// Applies inline code style to the given style
    /// </summary>
    /// <param name="style">
    /// The style to be styled as inline code.
    /// </param>
    /// <typeparam name="T">
    /// The type of the style that implements <see cref="INode"/>.
    /// </typeparam>
    /// <returns>
    /// An instance of <see cref="InlineCodeNode{TInner}"/> containing the provided style wrapped with inline code syntax.
    /// </returns>
    public static InlineCodeNode<T> Apply<T>(T style) where T : INode =>
        InlineCodeNode<T>.Apply(style);
}

/// <summary>
/// Represents inline code text in MarkdownV2 format.
/// Example: "`inline code`"
/// </summary>
/// <typeparam name="TInner">
/// The type of the inner style that will be wrapped with inline code syntax.
/// </typeparam>
public readonly struct InlineCodeNode<TInner> : INode
    where TInner : INode
{
    /// <summary>
    /// The prefix used to denote the start of inline code in MarkdownV2.
    /// </summary>
    public const string Prefix = "`";

    /// <summary>
    /// The suffix used to denote the end of inline code in MarkdownV2.
    /// </summary>
    public const string Suffix = "`";

    private readonly TInner _innerText;

    /// <summary>
    /// Initializes a new instance of the <see cref="InlineCodeNode{TInner}"/> struct.
    /// </summary>
    /// <param name="inner">
    /// The inner style to be wrapped with inline code syntax.
    /// </param>
    public InlineCodeNode(TInner inner)
    {
        _innerText = inner;
    }

    /// <summary>
    /// Returns the string representation of the inline code style in MarkdownV2 format.
    /// Note: This method allocates a new string in the most efficient way possible.
    /// Use this method when you finished all styling operations and need the final string.
    /// </summary>
    /// <returns>The created string representation</returns>
    public override string ToString() => string.Create(TotalLength, this, static (span, style) => style.CopyTo(span));

    /// <summary>
    /// Gets the length of the inner text (excluding syntax).
    /// </summary>
    public int InnerLength => _innerText.TotalLength;

    /// <inheritdoc />
    public int SyntaxLength => Prefix.Length + Suffix.Length;

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
        pos += InnerLength;

        // Copy suffix
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

        if (index < Prefix.Length + InnerLength)
        {
            return _innerText.TryGetChar(index - Prefix.Length, out character);
        }

        // Suffix part
        character = Suffix[index - Prefix.Length - InnerLength];
        return true;
    }

    /// <summary>
    /// Creates a new instance of <see cref="InlineCodeNode{TInner}"/> with the specified inner style.
    /// </summary>
    /// <param name="innerStyle">
    /// The inner style to be wrapped with inline code syntax.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="InlineCodeNode{TInner}"/> containing the provided inner style.
    /// </returns>
    public static InlineCodeNode<TInner> Apply(TInner innerStyle) => new(innerStyle);
}