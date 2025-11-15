using System.Diagnostics;
using StringEnricher.Configuration;
using StringEnricher.Extensions;

namespace StringEnricher.Nodes.Shared;

/// <summary>
/// A style that represents an ushort.
/// </summary>
[DebuggerDisplay("{typeof(UShortNode).Name,nq} Value={_ushort} Format={_format} Provider={_provider}")]
public struct UShortNode : INode
{
    private readonly ushort _ushort;
    private readonly string? _format;
    private readonly IFormatProvider? _provider;

    /// <summary>
    /// Initializes a new instance of the <see cref="UShortNode"/> struct.
    /// </summary>
    /// <param name="ushort">
    /// The ushort value to be represented by this node.
    /// </param>
    /// <param name="format">
    /// A standard or custom numeric format string that defines the format of the ushort.
    /// </param>
    /// <param name="provider">
    /// An object that supplies culture-specific formatting information.
    /// </param>
    public UShortNode(ushort @ushort, string? format = null, IFormatProvider? provider = null)
    {
        _ushort = @ushort;
        _format = format;
        _provider = provider;
    }

    /// <inheritdoc />
    public int SyntaxLength => 0;

    /// <inheritdoc />
    /// Lazy evaluation of total length is needed to avoid unnecessary complex calculations
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public int TotalLength => _totalLength ??=
        _ushort.GetSpanFormattableLength(StringEnricherSettings.Nodes.Shared.UShortNode, _format, _provider);

    private int? _totalLength;

    /// <inheritdoc />
    public override string ToString() => string.Create(TotalLength, this, static (span, node) => node.CopyTo(span));

    /// <inheritdoc />
    public int CopyTo(Span<char> destination) => _ushort.TryFormat(destination, out var textLength, _format, _provider)
        ? textLength
        : throw new ArgumentException("The destination span is too small to hold the ushort value.",
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
        _ushort.TryFormat(buffer, out _, _format, _provider);
        character = buffer[index];
        return true;
    }

    /// <summary>
    /// Implicitly converts a ushort to a <see cref="UShortNode"/>.
    /// </summary>
    /// <param name="ushort">Source ushort</param>
    /// <returns><see cref="UShortNode"/></returns>
    public static implicit operator UShortNode(ushort @ushort) => new(@ushort);
}