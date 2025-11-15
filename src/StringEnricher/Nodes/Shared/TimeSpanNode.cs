using System.Diagnostics;
using StringEnricher.Configuration;
using StringEnricher.Extensions;

namespace StringEnricher.Nodes.Shared;

/// <summary>
/// A style that represents a timeSpan.
/// </summary>
[DebuggerDisplay("{typeof(TimeSpanNode).Name,nq} Value={_timeSpan} Format={_format} Provider={_provider}")]
public struct TimeSpanNode : INode
{
    private readonly TimeSpan _timeSpan;
    private readonly string? _format;
    private readonly IFormatProvider? _provider;

    /// <summary>
    /// Initializes a new instance of the <see cref="TimeSpanNode"/> struct.
    /// </summary>
    /// <param name="timeSpan">
    /// The timeSpan value.
    /// </param>
    /// <param name="format">
    /// The format to use. If null, the default format is used.
    /// </param>
    /// <param name="provider">
    /// The format provider to use. If null, the current culture is used.
    /// </param>
    public TimeSpanNode(TimeSpan timeSpan, string? format = null, IFormatProvider? provider = null)
    {
        _timeSpan = timeSpan;
        _format = format;
        _provider = provider;
    }

    /// <inheritdoc />
    public int SyntaxLength => 0;

    /// <inheritdoc />
    /// Lazy evaluation of total length is needed to avoid unnecessary complex calculations
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public int TotalLength => _totalLength ??=
        _timeSpan.GetSpanFormattableLength(StringEnricherSettings.Nodes.Shared.TimeSpanNode, _format, _provider);

    private int? _totalLength;

    /// <inheritdoc />
    public override string ToString() => string.Create(TotalLength, this, static (span, node) => node.CopyTo(span));

    /// <inheritdoc />
    public int CopyTo(Span<char> destination) =>
        _timeSpan.TryFormat(destination, out var textLength, _format, _provider)
            ? textLength
            : throw new ArgumentException("The destination span is too small to hold the TimeSpan value.",
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

        var result = _timeSpan.GetCharAtIndex(
            index: index,
            nodeSettings: StringEnricherSettings.Nodes.Shared.TimeSpanNode,
            format: _format,
            provider: _provider,
            initialBufferLengthHint: _totalLength
        );

        character = result.Char;

        _totalLength ??= result.CharsWritten;

        return result.Success;
    }

    /// <summary>
    /// Implicitly converts a timeSpan to a <see cref="TimeSpanNode"/>.
    /// </summary>
    /// <param name="timeSpan">Source timeSpan</param>
    /// <returns><see cref="TimeSpanNode"/></returns>
    public static implicit operator TimeSpanNode(TimeSpan timeSpan) => new(timeSpan);
}