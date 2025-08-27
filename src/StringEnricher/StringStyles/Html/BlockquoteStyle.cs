namespace StringEnricher.StringStyles.Html;

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
    /// A new instance of <see cref="BlockquoteStyle{PlainTextStyle}"/> wrapping the provided text.
    /// </returns>
    public static BlockquoteStyle<PlainTextStyle> Apply(string text) =>
        BlockquoteStyle<PlainTextStyle>.Apply(text);

    /// <summary>
    /// Applies blockquote style to the given style.
    /// </summary>
    /// <param name="style">
    /// The inner style to be wrapped with blockquote HTML tags.
    /// </param>
    /// <typeparam name="T">
    /// The type of the inner style that implements <see cref="IStyle"/>.
    /// </typeparam>
    /// <returns>
    /// A new instance of <see cref="BlockquoteStyle{T}"/> wrapping the provided inner style.
    /// </returns>
    public static BlockquoteStyle<T> Apply<T>(T style) where T : IStyle =>
        BlockquoteStyle<T>.Apply(style);
}

/// <summary>
/// Represents blockquote text in HTML format.
/// Example: "<blockquote>quoted text</blockquote>"
/// </summary>
public readonly struct BlockquoteStyle<TInner> : IStyle
    where TInner : IStyle
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
    /// Initializes a new instance of the <see cref="BlockquoteStyle{TInner}"/> struct.
    /// </summary>
    /// <param name="inner">The inner style to be wrapped with blockquote HTML tags.</param>
    public BlockquoteStyle(TInner inner)
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

    public static BlockquoteStyle<TInner> Apply(TInner innerStyle) => new(innerStyle);
}