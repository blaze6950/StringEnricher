namespace StringEnricher.Nodes.MarkdownV2;

/// <summary>
/// Provides methods to apply bold styling in MarkdownV2 format.
/// Example: "*bold text*"
/// </summary>
public static class BoldMarkdownV2
{
    /// <summary>
    /// Applies bold style to the given text.
    /// </summary>
    /// <param name="text">
    /// The text to be wrapped with bold syntax.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="BoldNode{TInner}"/> wrapping the provided text.
    /// </returns>
    public static BoldNode<PlainTextNode> Apply(string text) =>
        BoldNode<PlainTextNode>.Apply(text);

    /// <summary>
    /// Applies bold style to the given style.
    /// </summary>
    /// <param name="style">
    /// The inner style to be wrapped with bold syntax.
    /// </param>
    /// <typeparam name="T">
    /// The type of the inner style that implements <see cref="INode"/>.
    /// </typeparam>
    /// <returns>
    /// A new instance of <see cref="BoldNode{TInner}"/> wrapping the provided inner style.
    /// </returns>
    public static BoldNode<T> Apply<T>(T style) where T : INode =>
        BoldNode<T>.Apply(style);
}

/// <summary>
/// Represents bold text in MarkdownV2 format.
/// Example: "*bold text*"
/// </summary>
/// <typeparam name="TInner">
/// The type of the inner style that will be wrapped with bold syntax.
/// </typeparam>
public readonly struct BoldNode<TInner> : INode
    where TInner : INode
{
    /// <summary>
    /// The prefix and suffix used for bold styling in MarkdownV2.
    /// </summary>
    public const string Prefix = "*";

    /// <summary>
    /// The suffix used for bold styling in MarkdownV2.
    /// </summary>
    public const string Suffix = "*";

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
    /// Returns the string representation of the bold style in MarkdownV2 format.
    /// Note: This method allocates a new string in the most efficient way possible.
    /// Use this method when you finished all styling operations and need the final string.
    /// </summary>
    /// <returns>The created string representation</returns>
    public override string ToString() => string.Create(TotalLength, this, static (span, style) => style.CopyTo(span));

    /// <summary>
    /// Gets the length of the inner text.
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