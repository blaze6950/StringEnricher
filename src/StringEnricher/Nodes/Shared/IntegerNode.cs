using System.Diagnostics;
using StringEnricher.Configuration;
using StringEnricher.Extensions;

namespace StringEnricher.Nodes.Shared;

/// <summary>
/// A style that represents an integer.
/// </summary>
[DebuggerDisplay("{typeof(IntegerNode).Name,nq} Value={_integer} Format={_format} Provider={_provider}")]
public struct IntegerNode : INode
{
    private readonly int _integer;
    private readonly string? _format;
    private readonly IFormatProvider? _provider;

    /// <summary>
    /// Initializes a new instance of the <see cref="IntegerNode"/> struct.
    /// </summary>
    /// <param name="integer">
    /// The integer value to be represented by this node.
    /// </param>
    /// <param name="format">
    /// An optional format string that defines how the integer should be formatted.
    /// </param>
    /// <param name="provider">
    /// An optional format provider that supplies culture-specific formatting information.
    /// </param>
    public IntegerNode(int integer, string? format = null, IFormatProvider? provider = null)
    {
        _integer = integer;
        _format = format;
        _provider = provider;
    }

    /// <inheritdoc />
    public int SyntaxLength => 0;

    /// <inheritdoc />
    /// Lazy evaluation of total length is needed to avoid unnecessary complex calculations
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public int TotalLength => _totalLength ??=
        _integer.GetSpanFormattableLength(StringEnricherSettings.Nodes.Shared.IntegerNode, _format, _provider);

    private int? _totalLength;

    /// <inheritdoc />
    public override string ToString() => string.Create(TotalLength, this, static (span, node) => node.CopyTo(span));

    /// <inheritdoc />
    public int CopyTo(Span<char> destination) => _integer.TryFormat(destination, out var textLength, _format, _provider)
        ? textLength
        : throw new ArgumentException("The destination span is too small to hold the integer value.",
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

        var result = _integer.GetCharAtIndex(
            index: index,
            nodeSettings: StringEnricherSettings.Nodes.Shared.IntegerNode,
            format: _format,
            provider: _provider,
            initialBufferLengthHint: _totalLength
        );

        character = result.Char;

        _totalLength ??= result.CharsWritten;

        return result.Success;
    }

    /// <summary>
    /// Implicitly converts an integer to a <see cref="IntegerNode"/>.
    /// </summary>
    /// <param name="integer">Source integer</param>
    /// <returns><see cref="IntegerNode"/></returns>
    public static implicit operator IntegerNode(int integer) => new(integer);
}