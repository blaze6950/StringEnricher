using System.Diagnostics;
using StringEnricher.Configuration;
using StringEnricher.Extensions;

namespace StringEnricher.Nodes.Shared;

/// <summary>
/// A style that represents a long.
/// </summary>
[DebuggerDisplay("{typeof(LongNode).Name,nq} Value={_long} Format={_format} Provider={_provider}")]
public struct LongNode : INode
{
    private readonly long _long;
    private readonly string? _format;
    private readonly IFormatProvider? _provider;

    /// <summary>
    /// Initializes a new instance of the <see cref="LongNode"/> struct.
    /// </summary>
    /// <param name="long">
    /// The long value to represent.
    /// </param>
    /// <param name="format">
    /// The format string to use when converting the long to a string.
    /// </param>
    /// <param name="provider">
    /// The format provider to use when converting the long to a string.
    /// </param>
    public LongNode(long @long, string? format = null, IFormatProvider? provider = null)
    {
        _long = @long;
        _format = format;
        _provider = provider;
    }

    /// <inheritdoc />
    public int SyntaxLength => 0;

    /// <inheritdoc />
    /// Lazy evaluation of total length is needed to avoid unnecessary complex calculations
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public int TotalLength => _totalLength ??=
        _long.GetSpanFormattableLength(StringEnricherSettings.Nodes.Shared.LongNode, _format, _provider);

    private int? _totalLength;

    /// <inheritdoc />
    public override string ToString() => string.Create(TotalLength, this, static (span, node) => node.CopyTo(span));

    /// <inheritdoc />
    public int CopyTo(Span<char> destination) => _long.TryFormat(destination, out var textLength, _format, _provider)
        ? textLength
        : throw new ArgumentException("The destination span is too small to hold the long value.",
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

        var result = _long.GetCharAtIndex(
            index: index,
            nodeSettings: StringEnricherSettings.Nodes.Shared.LongNode,
            format: _format,
            provider: _provider,
            initialBufferLengthHint: _totalLength
        );

        character = result.Char;

        _totalLength ??= result.CharsWritten;

        return result.Success;
    }

    /// <summary>
    /// Implicitly converts a long to a <see cref="LongNode"/>.
    /// </summary>
    /// <param name="long">Source long</param>
    /// <returns><see cref="LongNode"/></returns>
    public static implicit operator LongNode(long @long) => new(@long);
}