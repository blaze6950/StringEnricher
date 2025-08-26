namespace StringEnricher.StringStyles.MarkdownV2;

public static class CodeBlockMarkdownV2
{
    public static CodeBlockStyle<PlainTextStyle> Apply(string codeBlock) =>
        CodeBlockStyle<PlainTextStyle>.Apply(codeBlock);

    public static CodeBlockStyle<T> Apply<T>(T codeBlock) where T : IStyle =>
        CodeBlockStyle<T>.Apply(codeBlock);
}

public readonly struct CodeBlockStyle<TInner> : IStyle
    where TInner : IStyle
{
    public const string Prefix = "```\n";
    public const string Suffix = "\n```";

    private readonly TInner _innerCodeBlock;

    public CodeBlockStyle(TInner innerCodeBlock)
    {
        _innerCodeBlock = innerCodeBlock;
    }

    public override string ToString() => string.Create(TotalLength, this, static (span, style) => style.CopyTo(span));

    public int InnerLength => _innerCodeBlock.TotalLength;
    public int SyntaxLength => Prefix.Length + Suffix.Length;
    public int TotalLength => SyntaxLength + InnerLength;

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

        _innerCodeBlock.CopyTo(destination.Slice(pos, InnerLength));
        pos += InnerLength;

        Suffix.AsSpan().CopyTo(destination.Slice(pos, Suffix.Length));

        return totalLength;
    }

    public static CodeBlockStyle<TInner> Apply(TInner innerCodeBlock) => new(innerCodeBlock);
}