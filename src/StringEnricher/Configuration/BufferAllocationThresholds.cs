namespace StringEnricher.Configuration;

/// <summary>
/// Settings related to buffer sizes for string operations.
/// Use this struct to configure and represent buffer size settings for different extensions.
/// </summary>
internal struct BufferAllocationThresholds
{
    private readonly string _name;

    /// <summary>
    /// Initializes a new instance of the <see cref="BufferAllocationThresholds"/> struct.
    /// Each instance should be uniquely named to identify the extension it configures.
    /// </summary>
    /// <param name="name">
    /// The name of the extension these settings apply to.
    /// </param>
    private BufferAllocationThresholds(string name)
    {
        _name = name;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="BufferAllocationThresholds"/> struct with specified settings.
    /// Each instance should be uniquely named to identify the extension it configures.
    /// </summary>
    /// <param name="name">
    /// The name of the extension these settings apply to.
    /// </param>
    /// <param name="maxStackAllocLength">
    /// The maximum length of a node that can be allocated on the stack.
    /// Nodes with a length less than or equal to this value will use stack allocation for optimal performance.
    /// Default is 512 characters.
    /// Note: Increasing this value may improve performance for larger nodes but also increases stack usage,
    /// which can lead to stack overflow in deep recursion scenarios. Adjust with caution.
    /// Recommended range is between 128 and 1024 characters.
    /// Values above 2048 are strongly discouraged due to potential stack overflow risks.
    /// Consider your application's typical node sizes and stack usage patterns when configuring this setting.
    /// </param>
    /// <param name="maxPooledArrayLength">
    /// The maximum length of a node that can use array pooling.
    /// Nodes with a length greater than MaxStackAllocLength and less than or equal to this value
    /// will use array pooling to reduce memory allocations and pressure on the garbage collector.
    /// Nodes larger than this will use direct heap allocation.
    /// Default is 1,000,000 characters.
    /// Note: Increasing this value may reduce heap allocations for larger nodes but also increases memory usage
    /// and pressure on the garbage collector. Adjust with caution.
    /// Recommended range is between 100,000 and 5,000,000 characters.
    /// Values above 10,000,000 are strongly discouraged due to potential excessive memory usage.
    /// Consider your application's typical node sizes and memory usage patterns when configuring this setting.
    /// </param>
    public BufferAllocationThresholds(string name, int maxStackAllocLength, int maxPooledArrayLength) : this(name)
    {
        MaxStackAllocLength = maxStackAllocLength;
        MaxPooledArrayLength = maxPooledArrayLength;
    }

    #region MaxStackAllocLength

    /// <summary>
    /// The maximum length of a node that can be allocated on the stack.
    /// Nodes with a length less than or equal to this value will use stack allocation for optimal performance.
    /// Default is 512 characters.
    /// Note: Increasing this value may improve performance for larger nodes but also increases stack usage,
    /// which can lead to stack overflow in deep recursion scenarios. Adjust with caution.
    /// Recommended range is between 128 and 1024 characters.
    /// Values above 2048 are strongly discouraged due to potential stack overflow risks.
    /// Consider your application's typical node sizes and stack usage patterns when configuring this setting.
    /// </summary>
    public int MaxStackAllocLength
    {
        get => _maxStackAllocLength;
        set
        {
            StringEnricherSettings.EnsureNotSealed();

            ValidateMaxStackAllocLengthNewValue(value);

            _maxStackAllocLength = value;

            if (StringEnricherSettings.EnableDebugLogs)
            {
                Console.WriteLine($"[{_name}].{nameof(MaxStackAllocLength)} set to {value}.\n{Environment.StackTrace}");
            }
        }
    }

    private int _maxStackAllocLength = 512;

    /// <summary>
    /// Validates the new value for MaxStackAllocLength.
    /// </summary>
    /// <param name="value">New MaxStackAllocLength value.</param>
    private void ValidateMaxStackAllocLengthNewValue(int value)
    {
        // strict validation to prevent misconfiguration

        if (value <= 0)
        {
            // must be positive
            throw new ArgumentOutOfRangeException(
                nameof(value),
                value,
                $"{nameof(MaxStackAllocLength)} must be greater than zero.");
        }

        const int hardLimit = 2048; // Arbitrary hard limit to prevent excessive stack usage.
        if (value > hardLimit)
        {
            // strongly discourage values above this limit
            throw new ArgumentOutOfRangeException(
                nameof(value),
                value,
                $"{nameof(MaxStackAllocLength)} cannot be greater than {hardLimit}.");
        }

        if (value > _maxPooledArrayLength)
        {
            // must not exceed MaxPooledArrayLength
            throw new ArgumentOutOfRangeException(
                nameof(value),
                value,
                $"{nameof(MaxStackAllocLength)} cannot be greater than {nameof(MaxPooledArrayLength)} ({_maxPooledArrayLength}).");
        }

        // soft warnings to help with sensible configuration

        const int softLowerLimit = 32; // Arbitrary soft lower limit to prevent too small stack allocations.
        if (StringEnricherSettings.EnableDebugLogs && value < softLowerLimit)
        {
            // warn about very low values
            Console.WriteLine(
                $"WARNING: [{_name}].{nameof(MaxStackAllocLength)} is set to a very low value ({value}). " +
                $"This may lead to increased heap allocations and reduced performance. Consider setting it to at least {softLowerLimit}.");
        }

        const int softUpperLimit = 1024; // Arbitrary soft upper limit to prevent excessive stack allocations.
        if (StringEnricherSettings.EnableDebugLogs && value > softUpperLimit)
        {
            // warn about high values
            Console.WriteLine(
                $"WARNING: [{_name}].{nameof(MaxStackAllocLength)} is set to a high value ({value}). " +
                $"This may lead to increased stack usage and potential stack overflow in deep recursion scenarios. Consider setting it to no more than {softUpperLimit}.");
        }
    }

    #endregion

    #region MaxPooledArrayLength

    /// <summary>
    /// The maximum length of a node that can use array pooling.
    /// Nodes with a length greater than MaxStackAllocLength and less than or equal to this value
    /// will use array pooling to reduce memory allocations and pressure on the garbage collector.
    /// Nodes larger than this will use direct heap allocation.
    /// Default is 1,000,000 characters.
    /// Note: Increasing this value may reduce heap allocations for larger nodes but also increases memory usage
    /// and pressure on the garbage collector. Adjust with caution.
    /// Recommended range is between 100,000 and 5,000,000 characters.
    /// Values above 10,000,000 are strongly discouraged due to potential excessive memory usage.
    /// Consider your application's typical node sizes and memory usage patterns when configuring this setting.
    /// </summary>
    public int MaxPooledArrayLength
    {
        get => _maxPooledArrayLength;
        set
        {
            StringEnricherSettings.EnsureNotSealed();

            ValidateMaxPooledArrayLengthNewValue(value);

            _maxPooledArrayLength = value;

            if (StringEnricherSettings.EnableDebugLogs)
            {
                Console.WriteLine(
                    $"[{_name}].{nameof(MaxPooledArrayLength)} set to {value}.\n{Environment.StackTrace}");
            }
        }
    }

    private int _maxPooledArrayLength = 1_000_000;

    private void ValidateMaxPooledArrayLengthNewValue(int value)
    {
        // strict validation to prevent misconfiguration

        if (value <= 0)
        {
            // must be positive
            throw new ArgumentOutOfRangeException(
                nameof(value),
                value,
                $"{nameof(MaxPooledArrayLength)} must be greater than zero.");
        }

        if (value < _maxStackAllocLength)
        {
            // must not be less than MaxStackAllocLength
            throw new ArgumentOutOfRangeException(
                nameof(value),
                value,
                $"{nameof(MaxPooledArrayLength)} cannot be less than {nameof(MaxStackAllocLength)} ({_maxStackAllocLength}).");
        }

        const int hardLimit = 10_485_760; // Arbitrary hard limit to prevent excessive memory usage (10 MB).
        if (value > hardLimit)
        {
            // strongly discourage values above this limit
            throw new ArgumentOutOfRangeException(
                nameof(value),
                value,
                $"{nameof(MaxPooledArrayLength)} cannot be greater than {hardLimit}.");
        }

        // soft warnings to help with sensible configuration

        const int
            softLowerLimit = 500_000; // Arbitrary soft lower limit to prevent too small pooled arrays (500 KB).
        if (StringEnricherSettings.EnableDebugLogs && value < softLowerLimit)
        {
            // warn about very low values
            Console.WriteLine(
                $"WARNING: [{_name}].{nameof(MaxPooledArrayLength)} is set to a very low value ({value}). " +
                $"This may lead to increased heap allocations and reduced performance. Consider setting it to at least {softLowerLimit}.");
        }

        const int
            softUpperLimit = 5_242_880; // Arbitrary soft upper limit to prevent excessive pooled arrays (5 MB).
        if (StringEnricherSettings.EnableDebugLogs && value > softUpperLimit)
        {
            // warn about high values
            Console.WriteLine(
                $"WARNING: [{_name}].{nameof(MaxPooledArrayLength)} is set to a high value ({value}). " +
                $"This may lead to increased memory usage and pressure on the garbage collector. Consider setting it to no more than {softUpperLimit}.");
        }
    }

    #endregion
}