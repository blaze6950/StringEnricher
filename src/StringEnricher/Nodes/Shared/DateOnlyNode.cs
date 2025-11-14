using StringEnricher.Buffer;
using StringEnricher.Buffer.Processors;
using StringEnricher.Buffer.Processors.LengthCalculation;
using StringEnricher.Configuration;

namespace StringEnricher.Nodes.Shared;

/// <summary>
/// A style that represents a dateOnly.
/// </summary>
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
    public int TotalLength => _totalLength ??= GetDateOnlyLength(_dateOnly, _format, _provider);

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
        if (index < 0 || index >= TotalLength)
        {
            character = '\0';
            return false;
        }

        Span<char> buffer = stackalloc char[TotalLength];
        _dateOnly.TryFormat(buffer, out _, _format, _provider);
        character = buffer[index];
        return true;
    }

    /// <summary>
    /// Implicitly converts a dateOnly to a <see cref="DateOnlyNode"/>.
    /// </summary>
    /// <param name="dateOnly">Source dateOnly</param>
    /// <returns><see cref="DateOnlyNode"/></returns>
    public static implicit operator DateOnlyNode(DateOnly dateOnly) => new(dateOnly);

    /// <summary>
    /// Gets the length of the string representation of a dateOnly.
    /// </summary>
    /// <param name="value">
    /// The dateOnly value.
    /// </param>
    /// <param name="format">
    /// The format to use when converting the dateOnly to a string.
    /// </param>
    /// <param name="provider">
    /// The format provider to use when converting the dateOnly to a string.
    /// </param>
    /// <returns>
    /// The length of the string representation of the dateOnly.
    /// </returns>
    private static int GetDateOnlyLength(DateOnly value, string? format, IFormatProvider? provider)
    {
        // prepare state - the value and everything needed for formatting into an allocated buffer
        var state = new FormattingState<DateOnly>(value, format, provider);

        // tries to allocate a buffer and use the processor to get the length of the formatted value
        var length = BufferUtils.AllocateBuffer<DateOnlyLengthProcessor, FormattingState<DateOnly>, int>(
            func: new DateOnlyLengthProcessor(),
            state: in state,
            nodeSettings: StringEnricherSettings.Nodes.Shared.DateOnlyNode
        );

        return length;
    }
}