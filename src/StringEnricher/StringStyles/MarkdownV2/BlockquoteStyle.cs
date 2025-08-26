namespace StringEnricher.StringStyles.MarkdownV2;

// todo avoid issue with big text to quote - may cause stack overflow
// try to implement an indexer for every style to avoid stack allocation of big spans
// so instead of allocating a destination array we can get every character by index
// and copy it to destination span char by char

/// <summary>
/// Provides methods to apply blockquote style in MarkdownV2 format.
/// Example: ">blockquote text example\n>new blockquote line"
/// </summary>
public static class BlockquoteMarkdownV2
{
    /// <summary>
    /// Applies blockquote style to the given text.
    /// </summary>
    /// <param name="text">
    /// The text to be styled as a blockquote.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="BlockquoteStyle{PlainTextStyle}"/> containing the styled text.
    /// </returns>
    public static BlockquoteStyle<PlainTextStyle> Apply(string text) =>
        BlockquoteStyle<PlainTextStyle>.Apply(text);

    /// <summary>
    /// Applies blockquote style to the given style.
    /// </summary>
    /// <param name="style">
    /// The style to be wrapped with blockquote syntax.
    /// </param>
    /// <typeparam name="T">
    /// The type of the style that implements <see cref="IStyle"/>.
    /// </typeparam>
    /// <returns>
    /// A new instance of <see cref="BlockquoteStyle{T}"/> containing the styled text.
    /// </returns>
    public static BlockquoteStyle<T> Apply<T>(T style) where T : IStyle =>
        BlockquoteStyle<T>.Apply(style);
}

/// <summary>
/// Represents blockquote text in MarkdownV2 format.
/// Example: ">blockquote text example\n>new blockquote line"
/// </summary>
/// <typeparam name="TInner">
/// The type of the inner style that will be wrapped with blockquote syntax.
/// </typeparam>
public readonly struct BlockquoteStyle<TInner> : IStyle
    where TInner : IStyle
{
    /// <summary>
    /// The prefix used for each line in a blockquote in MarkdownV2.
    /// </summary>
    public const string LinePrefix = ">";
    /// <summary>
    /// The character used to separate lines in the blockquote.
    /// </summary>
    public const char LineSeparator = '\n';

    private readonly TInner _innerText;

    /// <summary>
    /// Initializes a new instance of the <see cref="BlockquoteStyle{TInner}"/> struct.
    /// </summary>
    /// <param name="inner">
    /// The inner style that will be wrapped with blockquote syntax.
    /// </param>
    public BlockquoteStyle(TInner inner)
    {
        _innerText = inner;
        SyntaxLength = CalculateSyntaxLength(inner);
    }

    /// <summary>
    /// Returns the string representation of the blockquote style in MarkdownV2 format.
    /// Note: This method allocates a new string in the most efficient way possible.
    /// Use this method when you finished all styling operations and need the final string.
    /// </summary>
    /// <returns>The created string representation</returns>
    public override string ToString() => string.Create(TotalLength, this, static (span, style) => style.CopyTo(span));

    public int InnerLength => _innerText.TotalLength;

    /// <inheritdoc />
    public int SyntaxLength { get; }

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

        // Copy inner text to a stack-allocated span
        Span<char> innerTextSpan = stackalloc char[_innerText.TotalLength];
        _innerText.CopyTo(innerTextSpan);

        var pos = 0;

        // Copy the line prefix for the first line
        LinePrefix.AsSpan().CopyTo(destination.Slice(pos, LinePrefix.Length));
        pos += LinePrefix.Length;

        // Iterate through each character in the inner text
        for (var i = 0; i < innerTextSpan.Length; i++)
        {
            var character = innerTextSpan[i];
            if (character != LineSeparator)
            {
                // Regular character, just copy it
                destination[pos] = character;
                pos++;
                continue;
            }

            // It's a line separator, so we need to add the line prefix after the new line character
            destination[pos] = character;
            pos++;

            // Add the line prefix for the new line
            LinePrefix.AsSpan().CopyTo(destination.Slice(pos, LinePrefix.Length));
            pos += LinePrefix.Length;
        }

        return totalLength;
    }

    /// <summary>
    /// Applies the blockquote style to the given inner style.
    /// </summary>
    /// <param name="innerStyle">
    /// The inner style to be wrapped with blockquote syntax.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="BlockquoteStyle{TInner}"/> containing the inner style.
    /// </returns>
    public static BlockquoteStyle<TInner> Apply(TInner innerStyle) => new(innerStyle);

    /// <summary>
    /// Calculates the length of the blockquote syntax based on the number of lines in the inner text.
    /// Does this in the most efficient way possible. No heap allocations.
    /// </summary>
    /// <param name="innerText">
    /// The inner text whose lines will be counted to determine the syntax length.
    /// </param>
    /// <returns>
    /// The total length of the blockquote syntax, which is the number of lines multiplied by the length of the line prefix.
    /// </returns>
    private static int CalculateSyntaxLength(TInner innerText)
    {
        var newLinesInTextCount = 0;
        Span<char> span = stackalloc char[innerText.TotalLength];
        innerText.CopyTo(span);
        for (var i = 0; i < span.Length; i++)
        {
            if (span[i] == '\n')
            {
                newLinesInTextCount++;
            }
        }

        var prefixCount = newLinesInTextCount + 1; // one prefix for each line
        return LinePrefix.Length * prefixCount;
    }
}