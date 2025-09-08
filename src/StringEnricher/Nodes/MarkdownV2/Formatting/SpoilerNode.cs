using StringEnricher.Nodes.Shared;

namespace StringEnricher.Nodes.MarkdownV2.Formatting;

/// <summary>
/// Provides methods to apply spoiler styling in MarkdownV2 format.
/// Example: "||spoiler text||"
/// </summary>
public static class SpoilerMarkdownV2
{
    /// <summary>
    /// Applies spoiler styling to the given text using plain text style as the inner style.
    /// </summary>
    /// <param name="text">
    /// The text to be styled as a spoiler.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="SpoilerNode{TInner}"/> containing the styled text.
    /// </returns>
    public static SpoilerNode<PlainTextNode> Apply(string text) =>
        SpoilerNode<PlainTextNode>.Apply(text);

    /// <summary>
    /// Applies spoiler styling to the given style.
    /// </summary>
    /// <param name="style">
    /// The style to be wrapped with spoiler syntax.
    /// </param>
    /// <typeparam name="T">
    /// The type of the style that implements <see cref="INode"/>.
    /// </typeparam>
    /// <returns>
    /// A new instance of <see cref="SpoilerNode{TInner}"/> containing the styled text.
    /// </returns>
    public static SpoilerNode<T> Apply<T>(T style) where T : INode =>
        SpoilerNode<T>.Apply(style);

    /// <summary>
    /// Applies spoiler styling to the given integer.
    /// </summary>
    /// <param name="integer">
    /// The integer to be styled as spoiler.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="SpoilerNode{TInner}"/> containing the styled integer.
    /// </returns>
    public static SpoilerNode<IntegerNode> Apply(int integer) =>
        SpoilerNode<IntegerNode>.Apply(integer);
}

/// <summary>
/// Represents spoiler text in MarkdownV2 format.
/// Example: "||spoiler text||"
/// </summary>
/// <typeparam name="TInner">
/// The type of the inner style that will be wrapped with spoiler syntax.
/// </typeparam>
public readonly struct SpoilerNode<TInner> : INode
    where TInner : INode
{
    /// <summary>
    /// The prefix used to denote the start of spoiler text in MarkdownV2.
    /// </summary>
    public const string Prefix = "||";

    /// <summary>
    /// The suffix used to denote the end of spoiler text in MarkdownV2.
    /// </summary>
    public const string Suffix = "||";

    private readonly TInner _innerText;

    /// <summary>
    /// Initializes a new instance of the <see cref="SpoilerNode{TInner}"/> struct.
    /// </summary>
    /// <param name="inner">
    /// The inner style that will be wrapped with spoiler syntax.
    /// </param>
    public SpoilerNode(TInner inner)
    {
        _innerText = inner;
    }

    /// <summary>
    /// Returns the string representation of the spoiler style in MarkdownV2 format.
    /// Note: This method allocates a new string in the most efficient way possible.
    /// Use this method when you finished all styling operations and need the final string.
    /// </summary>
    /// <returns>The created string representation</returns>
    public override string ToString() => string.Create(TotalLength, this, static (span, style) => style.CopyTo(span));

    /// <summary>
    /// Gets the length of the inner text excluding the spoiler syntax.
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
    /// Creates a new instance of <see cref="SpoilerNode{TInner}"/> with the specified inner style.
    /// </summary>
    /// <param name="innerStyle">
    /// The inner style to be wrapped with spoiler syntax.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="SpoilerNode{TInner}"/> containing the specified inner style.
    /// </returns>
    public static SpoilerNode<TInner> Apply(TInner innerStyle) => new(innerStyle);
}