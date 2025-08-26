namespace StringEnricher.StringStyles.Html;

public static class SpecificCodeBlockHtml
{
    public static SpecificCodeBlockStyle<PlainTextStyle> Apply(string codeBlock, string language) =>
        SpecificCodeBlockStyle<PlainTextStyle>.Apply(codeBlock, language);

    public static SpecificCodeBlockStyle<T> Apply<T>(T codeBlock, T language) where T : IStyle =>
        SpecificCodeBlockStyle<T>.Apply(codeBlock, language);
}

public readonly struct SpecificCodeBlockStyle<TInner> : IStyle
    where TInner : IStyle
{
    public const string Prefix = "<pre><code class=\"language-";
    public const string Separator = "\">";
    public const string Suffix = "</code></pre>";

    private readonly TInner _innerCodeBlock;
    private readonly TInner _language;

    public SpecificCodeBlockStyle(TInner codeBlock, TInner language)
    {
        _innerCodeBlock = codeBlock;
        _language = language;
    }

    public override string ToString() => string.Create(TotalLength, this, static (span, style) => style.CopyTo(span));

    public int InnerLength => _innerCodeBlock.TotalLength + _language.TotalLength;
    public int SyntaxLength => Prefix.Length + Separator.Length + Suffix.Length;
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

        _language.CopyTo(destination.Slice(pos, _language.TotalLength));
        pos += _language.TotalLength;

        Separator.AsSpan().CopyTo(destination.Slice(pos, Separator.Length));
        pos += Separator.Length;

        _innerCodeBlock.CopyTo(destination.Slice(pos, _innerCodeBlock.TotalLength));
        pos += _innerCodeBlock.TotalLength;

        Suffix.AsSpan().CopyTo(destination.Slice(pos, Suffix.Length));

        return totalLength;
    }

    public static SpecificCodeBlockStyle<TInner> Apply(TInner codeBlock, TInner language) => new(codeBlock, language);
}