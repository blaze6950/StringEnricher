using System.Diagnostics;
using StringEnricher.Buffer;
using StringEnricher.Buffer.Results;
using StringEnricher.Buffer.States;
using StringEnricher.Configuration;
using StringEnricher.Extensions;

namespace StringEnricher.Nodes.Shared;

/// <summary>
/// A style that represents a dateTime.
/// </summary>
[DebuggerDisplay("{typeof(DateTimeNode).Name,nq} Value={_dateTime} Format={_format} Provider={_provider}")]
public struct DateTimeNode : INode
{
    private readonly DateTime _dateTime;
    private readonly string? _format;
    private readonly IFormatProvider? _provider;

    /// <summary>
    /// Initializes a new instance of the <see cref="DateTimeNode"/> struct.
    /// </summary>
    /// <param name="dateTime">
    /// The dateTime value.
    /// </param>
    /// <param name="format">
    /// The format to use when converting the dateTime to a string.
    /// </param>
    /// <param name="provider">
    /// The format provider to use when converting the dateTime to a string.
    /// </param>
    public DateTimeNode(DateTime dateTime, string? format = null, IFormatProvider? provider = null)
    {
        _dateTime = dateTime;
        _format = format;
        _provider = provider;
    }

    /// <inheritdoc />
    public int SyntaxLength => 0;

    /// <inheritdoc />
    /// Lazy evaluation of total length is needed to avoid unnecessary complex calculations
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public int TotalLength => _totalLength ??=
        _dateTime.GetSpanFormattableLength(StringEnricherSettings.Nodes.Shared.DateTimeNode, _format, _provider);

    private int? _totalLength;

    /// <inheritdoc />
    public override string ToString() => string.Create(TotalLength, this, static (span, node) => node.CopyTo(span));

    /// <inheritdoc />
    public int CopyTo(Span<char> destination) =>
        _dateTime.TryFormat(destination, out var textLength, _format, _provider)
            ? textLength
            : throw new ArgumentException("The destination span is too small to hold the DateTime value.",
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

        var result = _dateTime.GetCharAtIndex(
            index: index,
            nodeSettings: StringEnricherSettings.Nodes.Shared.DateTimeNode,
            format: _format,
            provider: _provider,
            initialBufferLengthHint: _totalLength
        );

        character = result.Char;

        _totalLength ??= result.CharsWritten;

        return result.Success;
    }

    /// <summary>
    /// Implicitly converts a dateTime to a <see cref="DateTimeNode"/>.
    /// </summary>
    /// <param name="dateTime">Source dateTime</param>
    /// <returns><see cref="DateTimeNode"/></returns>
    public static implicit operator DateTimeNode(DateTime dateTime) => new(dateTime);
}