using StringEnricher.Nodes.Shared;

namespace StringEnricher.Nodes.MarkdownV2.Formatting;

/// <summary>
/// Provides methods to apply underline style to text in MarkdownV2 format.
/// Example: "__underline text__"
/// </summary>
public static class UnderlineMarkdownV2
{
    /// <summary>
    /// Applies the underline style to the given text.
    /// </summary>
    /// <param name="text">
    /// The text to be styled with underline syntax.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="UnderlineNode{TInner}"/> that wraps the provided text.
    /// </returns>
    public static UnderlineNode<PlainTextNode> Apply(string text) =>
        UnderlineNode<PlainTextNode>.Apply(text);

    /// <summary>
    /// Applies the underline style to the given style.
    /// </summary>
    /// <param name="style">
    /// The style to be wrapped with underline syntax.
    /// </param>
    /// <typeparam name="T">
    /// The type of the style that implements <see cref="INode"/>.
    /// </typeparam>
    /// <returns>
    /// A new instance of <see cref="UnderlineNode{TInner}"/> that wraps the provided style.
    /// </returns>
    public static UnderlineNode<T> Apply<T>(T style) where T : INode =>
        UnderlineNode<T>.Apply(style);

    /// <summary>
    /// Applies underline style to the given integer.
    /// </summary>
    /// <param name="integer">
    /// The integer to be styled with underline.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="UnderlineNode{TInner}"/> containing the styled integer.
    /// </returns>
    public static UnderlineNode<IntegerNode> Apply(int integer) =>
        UnderlineNode<IntegerNode>.Apply(integer);
}

/// <summary>
/// Represents underline text in MarkdownV2 format.
/// Example: "__underline text__"
/// </summary>
/// <typeparam name="TInner">
/// The type of the inner style that will be wrapped with underline syntax.
/// </typeparam>
public readonly struct UnderlineNode<TInner> : INode
    where TInner : INode
{
    /// <summary>
    /// The prefix and suffix used for underline style in MarkdownV2 format.
    /// </summary>
    public const string Prefix = "__";

    /// <summary>
    /// The prefix and suffix used for underline style in MarkdownV2 format.
    /// </summary>
    public const string Suffix = "__";

    private readonly TInner _innerText;

    /// <summary>
    /// Initializes a new instance of the <see cref="UnderlineNode{TInner}"/> struct.
    /// </summary>
    /// <param name="inner">
    /// The inner style that will be wrapped with underline syntax.
    /// </param>
    public UnderlineNode(TInner inner)
    {
        _innerText = inner;
    }

    /// <summary>
    /// Returns the string representation of the underline style in MarkdownV2 format.
    /// Note: This method allocates a new string in the most efficient way possible.
    /// Use this method when you finished all styling operations and need the final string.
    /// </summary>
    /// <returns>The created string representation</returns>
    public override string ToString() => string.Create(TotalLength, this, static (span, style) => style.CopyTo(span));

    /// <summary>
    /// Gets the length of the inner text excluding the underline syntax.
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
    /// Applies the underline style to the given inner style.
    /// </summary>
    /// <param name="innerStyle">
    /// The inner style to be wrapped with underline syntax.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="UnderlineNode{TInner}"/> that wraps the provided inner style.
    /// </returns>
    public static UnderlineNode<TInner> Apply(TInner innerStyle) => new(innerStyle);
}