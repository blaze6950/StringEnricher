using System.Runtime.CompilerServices;

namespace StringEnricher.Buffer;

/// <summary>
/// Represents the state for formatting a value of type T.
/// </summary>
/// <typeparam name="T">
/// The type of the value to be formatted.
/// </typeparam>
public readonly struct FormattingState<T>
{
    /// <summary>
    /// The value to be formatted.
    /// </summary>
    public readonly T Value;

    /// <summary>
    /// The format string to use for formatting.
    /// </summary>
    public readonly string? Format;

    /// <summary>
    /// The format provider to use for formatting.
    /// </summary>
    public readonly IFormatProvider? Provider;

    /// <summary>
    /// Initializes a new instance of the <see cref="FormattingState{T}"/> struct.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public FormattingState(T value, string? format = null, IFormatProvider? provider = null)
    {
        Value = value;
        Format = format;
        Provider = provider;
    }
}