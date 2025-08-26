namespace StringEnricher.StringStyles.MarkdownV2;

// todo avoid issue with big text to quote - may cause stack overflow
// try to implement an indexer for every style to avoid stack allocation of big spans
// so instead of allocating a destination array we can get every character by index
// and copy it to destination span char by char
public static class BlockquoteMarkdownV2
{
    public static BlockquoteStyle<PlainTextStyle> Apply(string text) =>
        BlockquoteStyle<PlainTextStyle>.Apply(text);

    public static BlockquoteStyle<T> Apply<T>(T style) where T : IStyle =>
        BlockquoteStyle<T>.Apply(style);
}

public readonly struct BlockquoteStyle<TInner> : IStyle
    where TInner : IStyle
{
    public const string LinePrefix = ">";
    public const char LineSeparator = '\n';

    private readonly TInner _innerText;

    public BlockquoteStyle(TInner inner)
    {
        _innerText = inner;
        SyntaxLength = CalculateSyntaxLength();
    }

    public override string ToString() => string.Create(TotalLength, this, static (span, style) => style.CopyTo(span));

    public int InnerLength => _innerText.TotalLength;

    public int SyntaxLength { get; }

    private int CalculateSyntaxLength()
    {
        var newLinesInTextCount = 0;
        Span<char> span = stackalloc char[_innerText.TotalLength];
        _innerText.CopyTo(span);
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

    public int TotalLength => SyntaxLength + InnerLength;

    public int CopyTo(Span<char> destination)
    {
        var totalLength = TotalLength;
        if (destination.Length < totalLength)
        {
            throw new ArgumentException("The destination span is too small to hold the formatted text.");
        }

        Span<char> innerTextSpan = stackalloc char[_innerText.TotalLength];
        _innerText.CopyTo(innerTextSpan);

        var pos = 0;

        LinePrefix.AsSpan().CopyTo(destination.Slice(pos, LinePrefix.Length));
        pos += LinePrefix.Length;

        for (var i = 0; i < innerTextSpan.Length; i++)
        {
            var character = innerTextSpan[i];
            if (character != LineSeparator)
            {
                destination[pos] = character;
                pos++;
                continue;
            }

            destination[pos] = character;
            pos++;

            LinePrefix.AsSpan().CopyTo(destination.Slice(pos, LinePrefix.Length));
            pos += LinePrefix.Length;
        }

        return totalLength;
    }

    public static BlockquoteStyle<TInner> Apply(TInner innerStyle) => new(innerStyle);
}