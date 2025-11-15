using System.Diagnostics;
using StringEnricher.Configuration;
using StringEnricher.Extensions;

namespace StringEnricher.Nodes.Shared;

/// <summary>
/// A style that represents a dateOnly.
/// </summary>
[DebuggerDisplay("{typeof(DateOnlyNode).Name,nq} Value={_dateOnly} Format={_format} Provider={_provider}")]
public struct DateOnlyNode : INode
{
    private readonly DateOnly _dateOnly;
    private readonly string? _format;
    private readonly IFormatProvider? _provider;

    /// <summary>
    /// Initializes a new instance of the <see cref="DateOnlyNode"/> struct.
    /// </summary>
    /// <param name="dateOnly">
    /// The dateOnly value.
    /// </param>
    /// <param name="format">
    /// The format to use when converting the dateOnly to a string.
    /// </param>
    /// <param name="provider">
    /// The format provider to use when converting the dateOnly to a string.
    /// </param>
    public DateOnlyNode(DateOnly dateOnly, string? format = null, IFormatProvider? provider = null)
    {
        _dateOnly = dateOnly;
        _format = format;
        _provider = provider;
    }

    /// <inheritdoc />
    public int SyntaxLength => 0;

    /// <inheritdoc />
    /// Lazy evaluation of total length is needed to avoid unnecessary complex calculations
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public int TotalLength => _totalLength ??=
        _dateOnly.GetSpanFormattableLength(StringEnricherSettings.Nodes.Shared.DateOnlyNode, _format, _provider);

    private int? _totalLength;

    /// <inheritdoc />
    public override string ToString() => string.Create(TotalLength, this, static (span, node) => node.CopyTo(span));

    /// <inheritdoc />
    public int CopyTo(Span<char> destination) =>
        _dateOnly.TryFormat(destination, out var textLength, _format, _provider)
            ? textLength
            : throw new ArgumentException("The destination span is too small to hold the DateOnly value.",
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

        var result = _dateOnly.GetCharAtIndex(
            index: index,
            nodeSettings: StringEnricherSettings.Nodes.Shared.DateOnlyNode,
            format: _format,
            provider: _provider,
            initialBufferLengthHint: _totalLength
        );

        character = result.Char;

        _totalLength ??= result.CharsWritten;

        return result.Success;
    }

    /// <summary>
    /// Implicitly converts a dateOnly to a <see cref="DateOnlyNode"/>.
    /// </summary>
    /// <param name="dateOnly">Source dateOnly</param>
    /// <returns><see cref="DateOnlyNode"/></returns>
    public static implicit operator DateOnlyNode(DateOnly dateOnly) => new(dateOnly);
}