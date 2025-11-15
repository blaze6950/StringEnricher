namespace StringEnricher.Configuration;

/// <summary>
/// Configuration for buffer sizes used in various operations.
/// </summary>
public struct BufferSizes
{
    private readonly string _name;

    /// <summary>
    /// Initializes a new instance of the <see cref="BufferSizes"/> struct.
    /// NOTE: Initializes with default values: InitialBufferLength = 1, MaxBufferLength = 1,000,000.
    /// </summary>
    /// <param name="name">
    /// The name of the configuration instance, used for logging purposes.
    /// </param>
    internal BufferSizes(string name)
    {
        _name = name;
        _initialBufferLength = 1;
        _maxBufferLength = 1_000_000;
        _growthFactor = 2;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="BufferSizes"/> struct with specified initial and maximum buffer lengths.
    /// </summary>
    /// <param name="name">
    /// The name of the configuration instance, used for logging purposes.
    /// </param>
    /// <param name="initialBufferLength">
    /// The initial length of the buffer to be used for stack allocation.
    /// This value must be greater than zero and less than or equal to <paramref name="maxBufferLength"/>.
    /// Setting this value too high may lead to increased stack usage and potential stack overflow in deep recursion scenarios.
    /// </param>
    /// <param name="maxBufferLength">
    /// The maximum length of the buffer to be used for heap allocation when the initial buffer is insufficient.
    /// This value must be greater than zero and greater than or equal to <paramref name="initialBufferLength"/>.
    /// Setting this value too high may lead to increased memory usage.
    /// </param>
    internal BufferSizes(string name, int initialBufferLength, int maxBufferLength) : this(name)
    {
        _initialBufferLength = initialBufferLength;
        _maxBufferLength = maxBufferLength;
        ValidateInitialBufferLengthNewValue(_initialBufferLength);
        ValidateMaxBufferLengthNewValue(_maxBufferLength);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="BufferSizes"/> struct with specified initial and maximum buffer lengths, and growth factor.
    /// </summary>
    /// <param name="name">
    /// The name of the configuration instance, used for logging purposes.
    /// </param>
    /// <param name="initialBufferLength">
    /// The initial length of the buffer to be used for stack allocation.
    /// This value must be greater than zero and less than or equal to <paramref name="maxBufferLength"/>.
    /// Setting this value too high may lead to increased stack usage and potential stack overflow in deep recursion scenarios.
    /// </param>
    /// <param name="maxBufferLength">
    /// The maximum length of the buffer to be used for heap allocation when the initial buffer is insufficient.
    /// This value must be greater than zero and greater than or equal to <paramref name="initialBufferLength"/>.
    /// Setting this value too high may lead to increased memory usage. 
    /// </param>
    /// <param name="growthFactor">
    /// The multiplier to use when resizing the buffer.
    /// When the current buffer is insufficient, it will be resized by multiplying its size by this factor.
    /// Must be greater than 1.0. Default is 2.0 (doubling the buffer size each time).
    /// </param>
    internal BufferSizes(string name, int initialBufferLength, int maxBufferLength, float growthFactor)
        : this(name, initialBufferLength, maxBufferLength)
    {
        _growthFactor = growthFactor;
        ValidateGrowthFactorNewValue(_growthFactor);
    }

    #region GrowthFactor

    internal float GrowthFactor
    {
        readonly get => _growthFactor;
        set
        {
            StringEnricherSettings.EnsureNotSealed();

            ValidateGrowthFactorNewValue(value);

            _growthFactor = value;

            DebugLog($"[{_name}].{nameof(GrowthFactor)} set to {value}.\n{Environment.StackTrace}");
        }
    }

    private void ValidateGrowthFactorNewValue(float value)
    {
        if (value <= 1.0f)
        {
            throw new ArgumentOutOfRangeException(
                nameof(value),
                value,
                $"{nameof(GrowthFactor)} must be greater than 1.0.");
        }

        if ((int)(_initialBufferLength * value) == _initialBufferLength)
        {
            throw new ArgumentOutOfRangeException(
                nameof(value),
                value,
                $"{nameof(GrowthFactor)} is too small to cause any increase in buffer size from the current {nameof(InitialBufferLength)} ({_initialBufferLength}). " +
                $"Consider setting it to at least {(float)(_initialBufferLength + 1) / _initialBufferLength}.");
        }

        if (value > 10.0f)
        {
            // warn about very high values
            DebugLog(
                $"WARNING: {nameof(GrowthFactor)} is set to a very high value ({value}). " +
                $"This may lead to excessive memory allocations and performance issues. Consider setting it to no more than 10.0.");
        }
    }

    private float _growthFactor;

    #endregion

    #region InitialBufferLength

    /// <summary>
    /// Gets or sets the initial length of the buffer to be used for stack allocation.
    /// This value must be greater than zero and less than or equal to <see cref="MaxBufferLength"/>.
    /// Setting this value too high may lead to increased stack usage and potential stack overflow in deep recursion scenarios.
    /// </summary>
    internal int InitialBufferLength
    {
        get => _initialBufferLength;
        set
        {
            StringEnricherSettings.EnsureNotSealed();

            ValidateInitialBufferLengthNewValue(value);

            _initialBufferLength = value;

            DebugLog($"[{_name}].{nameof(InitialBufferLength)} set to {value}.\n{Environment.StackTrace}");
        }
    }

    private int _initialBufferLength;

    private void ValidateInitialBufferLengthNewValue(int value)
    {
        // strict validation to prevent misconfiguration

        if (value <= 0)
        {
            // must be positive
            throw new ArgumentOutOfRangeException(
                nameof(value),
                value,
                $"{nameof(InitialBufferLength)} must be greater than zero.");
        }

        if (value > _maxBufferLength)
        {
            // must not exceed MaxPooledArrayLength
            throw new ArgumentOutOfRangeException(
                nameof(value),
                value,
                $"{nameof(InitialBufferLength)} cannot be greater than {nameof(MaxBufferLength)} ({_maxBufferLength}).");
        }
    }

    #endregion

    #region MaxBufferLength

    /// <summary>
    /// Gets or sets the maximum length of the buffer to be used for heap allocation when the initial buffer is insufficient.
    /// This value must be greater than zero and greater than or equal to <see cref="InitialBufferLength"/>.
    /// Setting this value too high may lead to increased memory usage.
    /// </summary>
    internal int MaxBufferLength
    {
        get => _maxBufferLength;
        set
        {
            StringEnricherSettings.EnsureNotSealed();

            ValidateMaxBufferLengthNewValue(value);

            _maxBufferLength = value;

            DebugLog($"[{_name}].{nameof(MaxBufferLength)} set to {value}.\n{Environment.StackTrace}");
        }
    }

    private int _maxBufferLength;

    private void ValidateMaxBufferLengthNewValue(int value)
    {
        // strict validation to prevent misconfiguration

        if (value <= 0)
        {
            // must be positive
            throw new ArgumentOutOfRangeException(
                nameof(value),
                value,
                $"{nameof(MaxBufferLength)} must be greater than zero.");
        }

        if (value < _initialBufferLength)
        {
            // must not be less than MaxStackAllocLength
            throw new ArgumentOutOfRangeException(
                nameof(value),
                value,
                $"{nameof(MaxBufferLength)} cannot be less than {nameof(InitialBufferLength)} ({_initialBufferLength}).");
        }
    }

    #endregion

    /// <summary>
    /// Implicitly converts a <see cref="BufferSizes"/> to its internal representation <see cref="BufferSizesInternal"/>.
    /// </summary>
    /// <param name="bufferSizes">
    /// The <see cref="BufferSizes"/> instance to convert.
    /// </param>
    /// <returns>
    /// The internal representation <see cref="BufferSizesInternal"/> of the provided <see cref="BufferSizes"/>.
    /// </returns>
    public static implicit operator BufferSizesInternal(BufferSizes bufferSizes) => new(
        bufferSizes.GrowthFactor,
        bufferSizes.InitialBufferLength,
        bufferSizes.MaxBufferLength
    );

    /// <summary>
    /// Logs a debug message if debug logging is enabled.
    /// </summary>
    /// <param name="msg">
    /// The message to log.
    /// </param>
    private void DebugLog(string msg)
    {
        if (StringEnricherSettings.EnableDebugLogs)
        {
            Console.WriteLine($"[{_name}] {msg}\n{Environment.StackTrace}");
        }
    }
}

/// <summary>
/// Internal DTO representation of BufferSizes for use within the library.
/// This struct is immutable and is used to pass buffer size configurations internally without validation overhead.
/// </summary>
public record struct BufferSizesInternal(
    float GrowthFactor,
    int InitialBufferLength,
    int MaxBufferLength
);