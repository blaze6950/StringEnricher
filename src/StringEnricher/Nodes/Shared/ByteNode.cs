using System.Diagnostics;
using StringEnricher.Configuration;
using StringEnricher.Extensions;

namespace StringEnricher.Nodes.Shared;

/// <summary>
/// A style that represents a byte.
/// </summary>
[DebuggerDisplay("{typeof(ByteNode).Name,nq} Value={_byte} Format={_format} Provider={_provider}")]
public struct ByteNode : INode
{
    private readonly byte _byte;
    private readonly string? _format;
    private readonly IFormatProvider? _provider;

    /// <summary>
    /// Initializes a new instance of the <see cref="ByteNode"/> struct.
    /// </summary>
    /// <param name="byte">
    /// The byte value to represent.
    /// </param>
    /// <param name="format">
    /// The format string to use when converting the byte to a string.
    /// </param>
    /// <param name="provider">
    /// The format provider to use when converting the byte to a string.
    /// </param>
    public ByteNode(byte @byte, string? format = null, IFormatProvider? provider = null)
    {
        _byte = @byte;
        _format = format;
        _provider = provider;
    }

    /// <inheritdoc />
    public int SyntaxLength => 0;

    /// <inheritdoc />
    /// Lazy evaluation of total length is needed to avoid unnecessary complex calculations
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public int TotalLength => _totalLength ??=
        _byte.GetSpanFormattableLength(StringEnricherSettings.Nodes.Shared.ByteNode, _format, _provider);

    private int? _totalLength;

    /// <inheritdoc />
    public override string ToString() => string.Create(TotalLength, this, static (span, node) => node.CopyTo(span));

    /// <inheritdoc />
    public int CopyTo(Span<char> destination) => _byte.TryFormat(destination, out var textLength, _format, _provider)
        ? textLength
        : throw new ArgumentException("The destination span is too small to hold the byte value.",
            nameof(destination));

    /// <inheritdoc />
    public bool TryGetChar(int index, out char character)
    {
        if (index < 0)
        {
            character = '\0';
            return false;
        }

        // if we already have the total length cached, use it to quickly determine if the index is valid
        if (_totalLength.HasValue && index >= _totalLength.Value)
        {
            character = '\0';
            return false;
        }

        var result = _byte.GetCharAtIndex(
            index: index,
            nodeSettings: StringEnricherSettings.Nodes.Shared.ByteNode,
            format: _format,
            provider: _provider,
            initialBufferLengthHint: _totalLength
        );

        character = result.Char;

        _totalLength ??= result.CharsWritten;

        return result.Success;
    }

    /// <summary>
    /// Implicitly converts a byte to a <see cref="ByteNode"/>.
    /// </summary>
    /// <param name="byte">Source byte</param>
    /// <returns><see cref="ByteNode"/></returns>
    public static implicit operator ByteNode(byte @byte) => new(@byte);
}