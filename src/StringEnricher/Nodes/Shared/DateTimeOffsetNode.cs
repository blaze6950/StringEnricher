using System.Diagnostics;
using StringEnricher.Configuration;
using StringEnricher.Extensions;

namespace StringEnricher.Nodes.Shared;

/// <summary>
/// A style that represents a dateTimeOffset.
/// </summary>
[DebuggerDisplay("{typeof(DateTimeOffsetNode).Name,nq} Value={_dateTimeOffset} Format={_format} Provider={_provider}")]
public struct DateTimeOffsetNode : INode
{
    private readonly DateTimeOffset _dateTimeOffset;
    private readonly string? _format;
    private readonly IFormatProvider? _provider;

    /// <summary>
    /// Initializes a new instance of the <see cref="DateTimeOffsetNode"/> struct.
    /// </summary>
    /// <param name="dateTimeOffset">
    /// The dateTimeOffset value.
    /// </param>
    /// <param name="format">
    /// The format to use when converting the dateTimeOffset to a string.
    /// </param>
    /// <param name="provider">
    /// The format provider to use when converting the dateTimeOffset to a string.
    /// </param>
    public DateTimeOffsetNode(DateTimeOffset dateTimeOffset, string? format = null, IFormatProvider? provider = null)
    {
        _dateTimeOffset = dateTimeOffset;
        _format = format;
        _provider = provider;
    }

    /// <inheritdoc />
    public int SyntaxLength => 0;

    /// <inheritdoc />
    /// Lazy evaluation of total length is needed to avoid unnecessary complex calculations
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public int TotalLength => _totalLength ??= _dateTimeOffset.GetSpanFormattableLength(
        StringEnricherSettings.Nodes.Shared.DateTimeOffsetNode,
        _format,
        _provider
    );

    private int? _totalLength;

    /// <inheritdoc />
    public override string ToString() => string.Create(TotalLength, this, static (span, node) => node.CopyTo(span));

    /// <inheritdoc />
    public int CopyTo(Span<char> destination) =>
        _dateTimeOffset.TryFormat(destination, out var textLength, _format, _provider)
            ? textLength
            : throw new ArgumentException("The destination span is too small to hold the DateTimeOffset value.",
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

        var result = _dateTimeOffset.GetCharAtIndex(
            index: index,
            nodeSettings: StringEnricherSettings.Nodes.Shared.DateTimeOffsetNode,
            format: _format,
            provider: _provider,
            initialBufferLengthHint: _totalLength
        );

        character = result.Char;

        _totalLength ??= result.CharsWritten;

        return result.Success;
    }

    /// <summary>
    /// Implicitly converts a dateTimeOffset to a <see cref="DateTimeOffsetNode"/>.
    /// </summary>
    /// <param name="dateTimeOffset">Source dateTimeOffset</param>
    /// <returns><see cref="DateTimeOffsetNode"/></returns>
    public static implicit operator DateTimeOffsetNode(DateTimeOffset dateTimeOffset) => new(dateTimeOffset);
}