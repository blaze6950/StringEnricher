namespace StringEnricher.Configuration;

/// <summary>
/// Configuration for buffer sizes used in various operations.
/// </summary>
internal struct BufferSizes
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
        GrowthFactor = 2;
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
        InitialBufferLength = initialBufferLength;
        MaxBufferLength = maxBufferLength;
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
        GrowthFactor = growthFactor;
    }

    #region GrowthFactor

    internal float GrowthFactor { get; set; }

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

            if (StringEnricherSettings.EnableDebugLogs)
            {
                Console.WriteLine($"[{_name}].{nameof(InitialBufferLength)} set to {value}.\n{Environment.StackTrace}");
            }
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

        const int hardLimit = 2048; // Arbitrary hard limit to prevent excessive stack usage.
        if (value > hardLimit)
        {
            // strongly discourage values above this limit
            throw new ArgumentOutOfRangeException(
                nameof(value),
                value,
                $"{nameof(InitialBufferLength)} cannot be greater than {hardLimit}.");
        }

        if (value > _maxBufferLength)
        {
            // must not exceed MaxPooledArrayLength
            throw new ArgumentOutOfRangeException(
                nameof(value),
                value,
                $"{nameof(InitialBufferLength)} cannot be greater than {nameof(MaxBufferLength)} ({_maxBufferLength}).");
        }

        // soft warnings to help with sensible configuration

        const int softUpperLimit = 1024; // Arbitrary soft upper limit to prevent excessive stack allocations.
        if (StringEnricherSettings.EnableDebugLogs && value > softUpperLimit)
        {
            // warn about high values
            Console.WriteLine(
                $"WARNING: [{_name}].{nameof(InitialBufferLength)} is set to a high value ({value}). " +
                $"This may lead to increased stack usage and potential stack overflow in deep recursion scenarios. Consider setting it to no more than {softUpperLimit}.");
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

            if (StringEnricherSettings.EnableDebugLogs)
            {
                Console.WriteLine($"[{_name}].{nameof(MaxBufferLength)} set to {value}.\n{Environment.StackTrace}");
            }
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

        // soft warnings to help with sensible configuration

        const int softUpperLimit = 1024; // Arbitrary soft upper limit to prevent excessive memory usage due to large buffer sizes.
        if (StringEnricherSettings.EnableDebugLogs && value > softUpperLimit)
        {
            // warn about high values
            Console.WriteLine(
                $"WARNING: [{_name}].{nameof(MaxBufferLength)} is set to a high value ({value}). " +
                $"This may lead to increased memory usage and potential performance issues. Consider setting it to no more than {softUpperLimit}.");
        }
    }

    #endregion
}