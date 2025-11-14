using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;

namespace StringEnricher.Buffer;

/// <summary>
/// Represents the result of an operation, indicating success or failure along with a value.
/// </summary>
/// <typeparam name="T">
/// The type of the value associated with the result.
/// </typeparam>
public readonly struct Result<T>
{
    /// <summary>
    /// Indicates whether the operation was successful.
    /// </summary>
    public readonly bool Success;

    /// <summary>
    /// The value associated with the result.
    /// </summary>
    public readonly T? Value;

    private Result(bool success, T? value)
    {
        Success = success;
        Value = value;
    }

    /// <summary>
    /// Creates a successful result with the specified value.
    /// </summary>
    /// <param name="value">
    /// The value associated with the successful result.
    /// </param>
    /// <returns>
    /// A <see cref="Result{T}"/> instance representing a successful operation.
    /// </returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Result<T> Ok(T value) => new(true, value);

    /// <summary>
    /// Creates a failed result with an optional value.
    /// </summary>
    /// <param name="value">
    /// The value associated with the failed result.
    /// </param>
    /// <returns>
    /// A <see cref="Result{T}"/> instance representing a failed operation.
    /// </returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Result<T> Fail(T? value = default) => new(false, value);
}