namespace StringEnricher.StringStyles.Html;

/// <summary>
/// Provides methods to apply italic styling in HTML format.
/// Example: "<i>italic text</i>"
/// </summary>
public static class ItalicHtml
{
    /// <summary>
    /// Applies italic style to the given text.
    /// </summary>
    /// <param name="text">The text to be wrapped with italic HTML tags.</param>
    /// <returns>A new instance of <see cref="ItalicStyle{PlainTextStyle}"/> wrapping the provided text.</returns>
    public static ItalicStyle<PlainTextStyle> Apply(string text) =>
        ItalicStyle<PlainTextStyle>.Apply(text);

    /// <summary>
    /// Applies italic style to the given style.
    /// </summary>
    /// <param name="style">The inner style to be wrapped with italic HTML tags.</param>
    /// <typeparam name="T">The type of the inner style that implements <see cref="IStyle"/>.</typeparam>
    /// <returns>A new instance of <see cref="ItalicStyle{T}"/> wrapping the provided inner style.</returns>
    public static ItalicStyle<T> Apply<T>(T style) where T : IStyle =>
        ItalicStyle<T>.Apply(style);
}

/// <summary>
/// Represents italic text in HTML format.
/// Example: "<i>italic text</i>"
/// </summary>
public readonly struct ItalicStyle<TInner> : IStyle
    where TInner : IStyle
{
    /// <summary>
    /// The opening italic tag.
    /// </summary>
    public const string Prefix = "<i>";
    /// <summary>
    /// The closing italic tag.
    /// </summary>
    public const string Suffix = "</i>";

    private readonly TInner _innerText;

    /// <summary>
    /// Initializes a new instance of the <see cref="ItalicStyle{TInner}"/> struct.
    /// </summary>
    /// <param name="inner">The inner style to be wrapped with italic HTML tags.</param>
    public ItalicStyle(TInner inner)
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
    /// Gets the total length of the HTML italic syntax.
    /// </summary>
    public int SyntaxLength => Prefix.Length + Suffix.Length;
    /// <summary>
    /// Gets the total length of the formatted text.
    /// </summary>
    public int TotalLength => SyntaxLength + InnerLength;

    /// <summary>
    /// Copies the formatted italic text to the provided span.
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

        if (index < Prefix.Length + InnerLength)
        {
            return _innerText.TryGetChar(index - Prefix.Length, out character);
        }

        character = Suffix[index - Prefix.Length - InnerLength];
        return true;
    }

    public static ItalicStyle<TInner> Apply(TInner innerStyle) => new(innerStyle);
}