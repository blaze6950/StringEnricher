namespace StringEnricher.StringStyles;

public interface IStyle
{
    public int SyntaxLength { get; }
    public int TotalLength { get; }
    public int CopyTo(Span<char> destination);
}