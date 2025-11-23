using System.Diagnostics;
using StringEnricher.Configuration;
using StringEnricher.Debug;
using StringEnricher.Extensions;

namespace StringEnricher.Nodes.Shared;

/// <summary>
/// A style that represents an enum.
/// </summary>
[DebuggerDisplay("{typeof(EnumNode).Name,nq} EnumType={typeof(TEnum).Name,nq} Value={_enum} Format={_format}")]
public struct EnumNode<TEnum> : INode where TEnum : struct, Enum
{
    private readonly TEnum _enum;
    private readonly string? _format;

    /// <summary>
    /// Initializes a new instance of the <see cref="EnumNode{TEnum}"/> struct.
    /// </summary>
    /// <param name="enum">
    /// The enum value to represent.
    /// </param>
    /// <param name="format">
    /// The format string (optional).
    /// </param>
    public EnumNode(TEnum @enum, string? format = null)
    {
        _enum = @enum;
        _format = format;
    }

    /// <inheritdoc />
    public int SyntaxLength => 0;

    /// <inheritdoc />
    /// Lazy evaluation of total length is needed to avoid unnecessary complex calculations
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public int TotalLength => _totalLength ??=
        _enum.GetSpanFormattableLength(StringEnricherSettings.Nodes.Shared.EnumNode, _format);

    private int? _totalLength;

    /// <inheritdoc />
    public override string ToString() => string.Create(TotalLength, this, static (span, node) => node.CopyTo(span));

    /// <inheritdoc />
    public string ToString(string? format, IFormatProvider? provider)
    {
        format = string.IsNullOrEmpty(format) ? _format : format;

        var length = _enum.GetSpanFormattableLength(
            nodeSettings: StringEnricherSettings.Nodes.Shared.EnumNode,
            format: format
        );

        return string.Create(
            length: length,
            state: ValueTuple.Create(_enum, format),
            action: static (span, state) =>
            {
                if (!Enum.TryFormat(state.Item1, span, out _, state.Item2))
                {
                    throw new InvalidOperationException("Formatting failed unexpectedly.");
                }
            }
        );
    }

    /// <inheritdoc />
    public bool TryFormat(
        Span<char> destination,
        out int charsWritten,
        ReadOnlySpan<char> format = default,
        IFormatProvider? provider = null
    ) => Enum.TryFormat(
        value: _enum,
        destination: destination,
        charsWritten: out charsWritten,
        format: format.IsEmpty ? _format : format
    );

    /// <inheritdoc />
    public int CopyTo(Span<char> destination) => Enum.TryFormat(_enum, destination, out var textLength, _format)
        ? textLength
        : throw new ArgumentException("The destination span is too small to hold the enum value.",
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
#if UNIT_TESTS
            DebugCounters.EnumNode_TryGetChar_CachedTotalLengthEvaluation++;
#endif
            character = '\0';
            return false;
        }

        var result = _enum.GetCharAtIndex(
            index: index,
            nodeSettings: StringEnricherSettings.Nodes.Shared.EnumNode,
            format: _format,
            initialBufferLengthHint: _totalLength
        );

        character = result.Char;

        _totalLength ??= result.CharsWritten;

        return result.Success;
    }
}