using StringEnricher.Nodes.Shared;

namespace StringEnricher.Nodes.Html.Formatting;

/// <summary>
/// Provides methods to apply bold styling in HTML format.
/// Example: "<b>bold text</b>"
/// </summary>
public static class BoldHtml
{
    /// <summary>
    /// Applies bold style to the given text.
    /// </summary>
    /// <param name="text">
    /// The text to be wrapped with bold HTML tags.
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
    /// The inner style to be wrapped with bold HTML tags.
    /// </param>
    /// <typeparam name="T">
    /// The type of the inner style that implements <see cref="INode"/>.
    /// </typeparam>
    /// <returns>
    /// A new instance of <see cref="BoldNode{TInner}"/> wrapping the provided inner style.
    /// </returns>
    public static BoldNode<T> Apply<T>(T style) where T : INode =>
        BoldNode<T>.Apply(style);

    /// <summary>
    /// Applies bold style to the given integer.
    /// </summary>
    /// <param name="integer">
    /// The integer to be styled as bold.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="BoldNode{TInner}"/> containing the styled integer.
    /// </returns>
    public static BoldNode<IntegerNode> Apply(int integer) =>
        BoldNode<IntegerNode>.Apply(integer);
}

/// <summary>
/// Represents bold text in HTML format.
/// Example: "<b>bold text</b>"
/// </summary>
public readonly struct BoldNode<TInner> : INode
    where TInner : INode
{
    /// <summary>
    /// The opening bold tag.
    /// </summary>
    public const string Prefix = "<b>";
    /// <summary>
    /// The closing bold tag.
    /// </summary>
    public const string Suffix = "</b>";

    private readonly TInner _innerText;

    /// <summary>
    /// Initializes a new instance of the <see cref="BoldNode{TInner}"/> struct.
    /// </summary>
    /// <param name="inner">The inner style to be wrapped with bold HTML tags.</param>
    public BoldNode(TInner inner)
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
    /// Gets the total length of the HTML bold syntax.
    /// </summary>
    public int SyntaxLength => Prefix.Length + Suffix.Length;
    /// <summary>
    /// Gets the total length of the formatted text.
    /// </summary>
    public int TotalLength => SyntaxLength + InnerLength;

    /// <summary>
    /// Copies the formatted bold text to the provided span.
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
        var totalLength = TotalLength;
        if (index < 0 || index >= totalLength)
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

    public static BoldNode<TInner> Apply(TInner innerStyle) => new(innerStyle);
}