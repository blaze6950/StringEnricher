using System.Diagnostics;
using StringEnricher.Configuration;
using StringEnricher.Extensions;

namespace StringEnricher.Nodes.Shared;

/// <summary>
/// A style that represents a sbyte.
/// </summary>
[DebuggerDisplay("{typeof(SByteNode).Name,nq} Value={_sbyte} Format={_format} Provider={_provider}")]
public struct SByteNode : INode
{
    private readonly sbyte _sbyte;
    private readonly string? _format;
    private readonly IFormatProvider? _provider;

    /// <summary>
    /// Initializes a new instance of the <see cref="SByteNode"/> struct.
    /// </summary>
    /// <param name="sbyte">
    /// The sbyte value to represent.
    /// </param>
    /// <param name="format">
    /// The format string to use when converting the sbyte to a string.
    /// </param>
    /// <param name="provider">
    /// The format provider to use when converting the sbyte to a string.
    /// </param>
    public SByteNode(sbyte @sbyte, string? format = null, IFormatProvider? provider = null)
    {
        _sbyte = @sbyte;
        _format = format;
        _provider = provider;
    }

    /// <inheritdoc />
    public int SyntaxLength => 0;

    /// <inheritdoc />
    /// Lazy evaluation of total length is needed to avoid unnecessary complex calculations
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public int TotalLength => _totalLength ??=
        _sbyte.GetSpanFormattableLength(StringEnricherSettings.Nodes.Shared.SByteNode, _format, _provider);

    private int? _totalLength;

    /// <inheritdoc />
    public override string ToString() => string.Create(TotalLength, this, static (span, node) => node.CopyTo(span));

    /// <inheritdoc />
    public int CopyTo(Span<char> destination) => _sbyte.TryFormat(destination, out var textLength, _format, _provider)
        ? textLength
        : throw new ArgumentException("The destination span is too small to hold the sbyte value.",
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

        var result = _sbyte.GetCharAtIndex(
            index: index,
            nodeSettings: StringEnricherSettings.Nodes.Shared.SByteNode,
            format: _format,
            provider: _provider,
            initialBufferLengthHint: _totalLength
        );

        character = result.Char;

        _totalLength ??= result.CharsWritten;

        return result.Success;
    }

    /// <summary>
    /// Implicitly converts a sbyte to a <see cref="SByteNode"/>.
    /// </summary>
    /// <param name="sbyte">Source sbyte</param>
    /// <returns><see cref="SByteNode"/></returns>
    public static implicit operator SByteNode(sbyte @sbyte) => new(@sbyte);
}