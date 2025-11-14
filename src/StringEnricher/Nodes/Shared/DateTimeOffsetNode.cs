using StringEnricher.Buffer;
using StringEnricher.Buffer.Processors.LengthCalculation;
using StringEnricher.Buffer.States;
using StringEnricher.Configuration;

namespace StringEnricher.Nodes.Shared;

/// <summary>
/// A style that represents a dateTimeOffset.
/// </summary>
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
    public int TotalLength => _totalLength ??= GetDateTimeOffsetLength(_dateTimeOffset, _format, _provider);

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
        if (index < 0 || index >= TotalLength)
        {
            character = '\0';
            return false;
        }

        Span<char> buffer = stackalloc char[TotalLength];
        _dateTimeOffset.TryFormat(buffer, out _, _format, _provider);
        character = buffer[index];
        return true;
    }

    /// <summary>
    /// Implicitly converts a dateTimeOffset to a <see cref="DateTimeOffsetNode"/>.
    /// </summary>
    /// <param name="dateTimeOffset">Source dateTimeOffset</param>
    /// <returns><see cref="DateTimeOffsetNode"/></returns>
    public static implicit operator DateTimeOffsetNode(DateTimeOffset dateTimeOffset) => new(dateTimeOffset);

    /// <summary>
    /// Gets the length of the string representation of a dateTimeOffset.
    /// </summary>
    /// <param name="value">
    /// The dateTimeOffset value.
    /// </param>
    /// <param name="format">
    /// The format to use when converting the dateTimeOffset to a string.
    /// </param>
    /// <param name="provider">
    /// The format provider to use when converting the dateTimeOffset to a string.
    /// </param>
    /// <returns>
    /// The length of the string representation of the dateTimeOffset.
    /// </returns>
    private static int GetDateTimeOffsetLength(DateTimeOffset value, string? format = null,
        IFormatProvider? provider = null)
    {
        // prepare state - the value and everything needed for formatting into an allocated buffer
        var state = new FormattingState<DateTimeOffset>(value, format, provider);

        // tries to allocate a buffer and use the processor to get the length of the formatted value
        var length = BufferUtils.AllocateBuffer<DateTimeOffsetLengthProcessor, FormattingState<DateTimeOffset>, int>(
            processor: new DateTimeOffsetLengthProcessor(),
            state: in state,
            nodeSettings: StringEnricherSettings.Nodes.Shared.DateTimeOffsetNode
        );

        return length;
    }
}