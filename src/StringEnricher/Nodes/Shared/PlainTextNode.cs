using System.Diagnostics;

namespace StringEnricher.Nodes.Shared;

/// <summary>
/// A style that represents plain text without any special formatting.
/// </summary>
[DebuggerDisplay("{typeof(PlainTextNode).Name,nq} Value={_text}")]
public readonly struct PlainTextNode : INode
{
    private readonly string _text;

    /// <summary>
    /// Initializes a new instance of the <see cref="PlainTextNode"/> struct.
    /// </summary>
    /// <param name="text"></param>
    public PlainTextNode(string text)
    {
        ArgumentNullException.ThrowIfNull(text);
        _text = text;
    }

    /// <inheritdoc />
    public int SyntaxLength => 0;

    /// <inheritdoc />
    public int TotalLength => _text.Length;

    /// <inheritdoc />
    public override string ToString() => string.Create(TotalLength, this, static (span, node) => node.CopyTo(span));

    /// <inheritdoc />
    /// Format and provider are ignored since bool has no custom formatting options
    public string ToString(string? format, IFormatProvider? formatProvider) => ToString();

    /// <inheritdoc />
    /// Format and provider are ignored since bool has no custom formatting options
    public bool TryFormat(
        Span<char> destination,
        out int charsWritten,
        ReadOnlySpan<char> format = default,
        IFormatProvider? provider = null
    )
    {
        charsWritten = 0;
        try
        {
            charsWritten = CopyTo(destination);
        }
        catch (Exception)
        {
            return false;
        }
        return true;
    }

    /// <inheritdoc />
    public int CopyTo(Span<char> destination)
    {
        if (_text.Length > destination.Length)
        {
            throw new ArgumentException("The destination span is too small to hold the text value.",
                nameof(destination));
        }

        _text.AsSpan().CopyTo(destination);

        return _text.Length;
    }

    /// <inheritdoc />
    public bool TryGetChar(int index, out char character)
    {
        if (index < 0 || index >= _text.Length)
        {
            character = '\0';
            return false;
        }

        character = _text[index];
        return true;
    }

    /// <summary>
    /// Implicitly converts a string to a <see cref="PlainTextNode"/>.
    /// </summary>
    /// <param name="text">Source string</param>
    /// <returns><see cref="PlainTextNode"/></returns>
    public static implicit operator PlainTextNode(string text) => new(text);
}