using StringEnricher.Nodes.Shared;

namespace StringEnricher.Nodes.Html.Formatting;

/// <summary>
/// Provides methods to apply blockquote styling in HTML format.
/// Example: "<blockquote>quoted text</blockquote>"
/// </summary>
public static class BlockquoteHtml
{
    /// <summary>
    /// Applies blockquote style to the given text.
    /// </summary>
    /// <param name="text">
    /// The text to be wrapped with blockquote HTML tags.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="BlockquoteNode{TInner}"/> wrapping the provided text.
    /// </returns>
    public static BlockquoteNode<PlainTextNode> Apply(string text) =>
        BlockquoteNode<PlainTextNode>.Apply(text);

    /// <summary>
    /// Applies blockquote style to the given style.
    /// </summary>
    /// <param name="style">
    /// The inner style to be wrapped with blockquote HTML tags.
    /// </param>
    /// <typeparam name="T">
    /// The type of the inner style that implements <see cref="INode"/>.
    /// </typeparam>
    /// <returns>
    /// A new instance of <see cref="BlockquoteNode{TInner}"/> wrapping the provided inner style.
    /// </returns>
    public static BlockquoteNode<T> Apply<T>(T style) where T : INode =>
        BlockquoteNode<T>.Apply(style);

    /// <summary>
    /// Applies blockquote style to the given integer.
    /// </summary>
    /// <param name="integer">
    /// The integer to be styled as blockquote.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="BlockquoteNode{TInner}"/> containing the styled integer.
    /// </returns>
    public static BlockquoteNode<IntegerNode> Apply(int integer) =>
        BlockquoteNode<IntegerNode>.Apply(integer);
}

/// <summary>
/// Represents blockquote text in HTML format.
/// Example: "<blockquote>quoted text</blockquote>"
/// </summary>
public readonly struct BlockquoteNode<TInner> : INode
    where TInner : INode
{
    /// <summary>
    /// The opening blockquote tag.
    /// </summary>
    public const string Prefix = "<blockquote>";
    /// <summary>
    /// The closing blockquote tag.
    /// </summary>
    public const string Suffix = "</blockquote>";

    private readonly TInner _innerText;

    /// <summary>
    /// Initializes a new instance of the <see cref="BlockquoteNode{TInner}"/> struct.
    /// </summary>
    /// <param name="inner">The inner style to be wrapped with blockquote HTML tags.</param>
    public BlockquoteNode(TInner inner)
    {
        _innerText = inner;
    }

    /// <inheritdoc/>
    public override string ToString() => string.Create(TotalLength, this, static (span, style) => style.CopyTo(span));

    /// <summary>
    /// Gets the length of the inner text.
    /// </summary>
    public int InnerLength => _innerText.TotalLength;
    /// <summary>
    /// Gets the total length of the HTML blockquote syntax.
    /// </summary>
    public int SyntaxLength => Prefix.Length + Suffix.Length;
    /// <summary>
    /// Gets the total length of the formatted text.
    /// </summary>
    public int TotalLength => SyntaxLength + InnerLength;

    /// <summary>
    /// Copies the formatted blockquote text to the provided span.
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

        _innerText.CopyTo(destination.Slice(pos, InnerLength));
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
            return _innerText.TryGetChar(index, out character);
        }

        index -= InnerLength;

        character = Suffix[index];
        return true;
    }

    public static BlockquoteNode<TInner> Apply(TInner innerStyle) => new(innerStyle);
}