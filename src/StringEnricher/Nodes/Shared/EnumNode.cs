using System.Diagnostics;
using StringEnricher.Configuration;
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
    public int CopyTo(Span<char> destination) => Enum.TryFormat(_enum, destination, out var textLength, _format)
        ? textLength
        : throw new ArgumentException("The destination span is too small to hold the enum value.",
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
        Enum.TryFormat(_enum, buffer, out _, _format);
        character = buffer[index];

        return true;
    }
}