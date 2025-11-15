using System.Diagnostics;
using StringEnricher.Configuration;
using StringEnricher.Extensions;

namespace StringEnricher.Nodes.Shared;

/// <summary>
/// A style that represents a decimal.
/// </summary>
[DebuggerDisplay("{typeof(DecimalNode).Name,nq} Value={_decimal} Format={_format} Provider={_provider}")]
public struct DecimalNode : INode
{
    private readonly decimal _decimal;
    private readonly string? _format;
    private readonly IFormatProvider? _provider;

    /// <summary>
    /// Initializes a new instance of the <see cref="DecimalNode"/> struct.
    /// </summary>
    /// <param name="decimal">
    /// The decimal value to represent.
    /// </param>
    /// <param name="format">
    /// The format string to use when converting the decimal to a string.
    /// </param>
    /// <param name="provider">
    /// The format provider to use when converting the decimal to a string.
    /// </param>
    public DecimalNode(decimal @decimal, string? format = null, IFormatProvider? provider = null)
    {
        _decimal = @decimal;
        _format = format;
        _provider = provider;
    }

    /// <inheritdoc />
    public int SyntaxLength => 0;

    /// <inheritdoc />
    /// Lazy evaluation of total length is needed to avoid unnecessary complex calculations
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public int TotalLength => _totalLength ??=
        _decimal.GetSpanFormattableLength(StringEnricherSettings.Nodes.Shared.DecimalNode, _format, _provider);

    private int? _totalLength;

    /// <inheritdoc />
    public override string ToString() => string.Create(TotalLength, this, static (span, node) => node.CopyTo(span));

    /// <inheritdoc />
    public int CopyTo(Span<char> destination) => _decimal.TryFormat(destination, out var textLength, _format, _provider)
        ? textLength
        : throw new ArgumentException("The destination span is too small to hold the decimal value.",
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

        var result = _decimal.GetCharAtIndex(
            index: index,
            nodeSettings: StringEnricherSettings.Nodes.Shared.DecimalNode,
            format: _format,
            provider: _provider,
            initialBufferLengthHint: _totalLength
        );

        character = result.Char;

        _totalLength ??= result.CharsWritten;

        return result.Success;
    }

    /// <summary>
    /// Implicitly converts a decimal to a <see cref="DecimalNode"/>.
    /// </summary>
    /// <param name="decimal">Source decimal</param>
    /// <returns><see cref="DecimalNode"/></returns>
    public static implicit operator DecimalNode(decimal @decimal) => new(@decimal);
}