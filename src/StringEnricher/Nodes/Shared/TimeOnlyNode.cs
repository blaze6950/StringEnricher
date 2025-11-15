using System.Diagnostics;
using StringEnricher.Configuration;
using StringEnricher.Extensions;

namespace StringEnricher.Nodes.Shared;

/// <summary>
/// A style that represents a timeOnly.
/// </summary>
[DebuggerDisplay("{typeof(TimeOnlyNode).Name,nq} Value={_timeOnly} Format={_format} Provider={_provider}")]
public struct TimeOnlyNode : INode
{
    private readonly TimeOnly _timeOnly;
    private readonly string? _format;
    private readonly IFormatProvider? _provider;

    /// <summary>
    /// Initializes a new instance of the <see cref="TimeOnlyNode"/> struct.
    /// </summary>
    /// <param name="timeOnly">
    /// The timeOnly value.
    /// </param>
    /// <param name="format">
    /// The format string.
    /// </param>
    /// <param name="provider">
    /// The format provider.
    /// </param>
    public TimeOnlyNode(TimeOnly timeOnly, string? format = null, IFormatProvider? provider = null)
    {
        _timeOnly = timeOnly;
        _format = format;
        _provider = provider;
    }

    /// <inheritdoc />
    public int SyntaxLength => 0;

    /// <inheritdoc />
    /// Lazy evaluation of total length is needed to avoid unnecessary complex calculations
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public int TotalLength => _totalLength ??=
        _timeOnly.GetSpanFormattableLength(StringEnricherSettings.Nodes.Shared.TimeOnlyNode, _format, _provider);

    private int? _totalLength;

    /// <inheritdoc />
    public override string ToString() => string.Create(TotalLength, this, static (span, node) => node.CopyTo(span));

    /// <inheritdoc />
    public int CopyTo(Span<char> destination) =>
        _timeOnly.TryFormat(destination, out var textLength, _format, _provider)
            ? textLength
            : throw new ArgumentException("The destination span is too small to hold the TimeOnly value.",
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

        var result = _timeOnly.GetCharAtIndex(
            index: index,
            nodeSettings: StringEnricherSettings.Nodes.Shared.TimeOnlyNode,
            format: _format,
            provider: _provider,
            initialBufferLengthHint: _totalLength
        );

        character = result.Char;

        _totalLength ??= result.CharsWritten;

        return result.Success;
    }

    /// <summary>
    /// Implicitly converts a timeOnly to a <see cref="TimeOnlyNode"/>.
    /// </summary>
    /// <param name="timeOnly">Source timeOnly</param>
    /// <returns><see cref="TimeOnlyNode"/></returns>
    public static implicit operator TimeOnlyNode(TimeOnly timeOnly) => new(timeOnly);
}