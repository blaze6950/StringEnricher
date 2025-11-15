using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;

namespace StringEnricher.Buffer;

/// <summary>
/// Represents the result of a buffer allocation operation.
/// </summary>
/// <typeparam name="T">
/// The type of the value associated with the result.
/// </typeparam>
public readonly struct BufferAllocationResult<T>
{
    /// <summary>
    /// Indicates whether the buffer allocation was successful.
    /// True means the buffer was large enough; false means it was not.
    /// NOTE: This does not indicate whether the operation that used the buffer was successful.
    /// </summary>
    public readonly bool IsSuccess;

    /// <summary>
    /// The value associated with the operation result.
    /// </summary>
    public readonly T? Value;

    private BufferAllocationResult(bool isSuccess, T? value)
    {
        IsSuccess = isSuccess;
        Value = value;
    }

    /// <summary>
    /// Creates a successful result with the specified value.
    /// Successful operations indicate that the buffer allocation was successful.
    /// BUT the operation that used the buffer may still fail by operation specific reasons.
    /// This OK result only indicates that the buffer was successfully allocated and there is no need to try allocate a larger buffer.
    /// </summary>
    /// <param name="value">
    /// The value associated with the successful buffer allocation.
    /// </param>
    /// <returns>
    /// A <see cref="BufferAllocationResult{T}"/> instance representing a successful buffer allocation.
    /// </returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static BufferAllocationResult<T> BufferIsEnough(T value) => new(true, value);

    /// <summary>
    /// Creates a failed result with the specified value.
    /// Failed operations indicate that the provided buffer was not large enough to hold the formatted value.
    /// Do not use this result to indicate that the operation itself failed for other reasons.
    /// Use it only when the buffer size is insufficient and a larger buffer is needed.
    /// </summary>
    /// <param name="value">
    /// The value associated with the failed buffer allocation.
    /// </param>
    /// <returns>
    /// A <see cref="BufferAllocationResult{T}"/> instance representing a failed buffer allocation.
    /// </returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static BufferAllocationResult<T> BufferIsNotEnough(T? value = default) => new(false, value);
}