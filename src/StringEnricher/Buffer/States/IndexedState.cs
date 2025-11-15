using System.Runtime.CompilerServices;

namespace StringEnricher.Buffer.States;

/// <summary>
/// Represents the state for formatting a value of type T.
/// </summary>
/// <typeparam name="T">
/// The type of the value to be formatted.
/// </typeparam>
public readonly struct IndexedState<T> : IState<T>
{
    /// <summary>
    /// The value to be formatted.
    /// </summary>
    public T Value { get; }

    /// <summary>
    /// The index of the value.
    /// </summary>
    public readonly int Index;

    /// <summary>
    /// Initializes a new instance of the <see cref="IndexedState{T}"/> struct.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IndexedState(T value, int index)
    {
        Value = value;
        Index = index;
    }
}