namespace StringEnricher.Configuration;

/// <summary>
/// Configuration settings for StringEnricher.
/// <br/><br/>
/// WARNING! Change these settings only if you fully understand the implications.
/// Improper changes may lead to performance degradation or increased memory usage. Or even bugs.
/// It's recommended to keep the default values unless you have specific requirements and have thoroughly tested the changes.
/// Once sealed, the settings cannot be modified further.
/// <br/><br/>
/// Recommendation: Configure these settings at the start of your application, before using any StringEnricher functionality.
/// Then call <see cref="Seal()"/> after initial configuration to prevent accidental changes at runtime.
/// </summary>
public static class StringEnricherSettings
{
    #region Sealed State

    /// <summary>
    /// Seals the settings to prevent further modifications.
    /// Once sealed, any attempt to change settings will throw an InvalidOperationException.
    /// </summary>
    public static void Seal() => _isSealed = true;

    private static bool _isSealed = false;

    /// <summary>
    /// Ensures that the settings are not sealed before allowing modifications.
    /// </summary>
    /// <exception cref="InvalidOperationException">
    /// Thrown if the settings are sealed.
    /// </exception>
    private static void EnsureNotSealed()
    {
        if (_isSealed)
        {
            throw new InvalidOperationException("StringEnricherSettings is sealed and cannot be modified.");
        }
    }

    #endregion

    /// <summary>
    /// Enables or disables debug logging for StringEnricher.
    /// When enabled, detailed debug information will be logged to help diagnose issues.
    /// Default is true (enabled).
    /// </summary>
    public static bool EnableDebugLogs { get; set; } = true;

    /// <summary>
    /// Configuration settings for various extensions used in StringEnricher.
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Configuration settings for StringBuilder-related optimizations.
        /// These settings help balance performance and memory usage when appending StringEnricher nodes to StringBuilder.
        /// - MaxStackAllocLength: The maximum length of a node that can be allocated on the stack (default: 512).
        /// - MaxPooledArrayLength: The maximum length of a node that can use array pooling (default: 1_000_000).
        /// Nodes larger than this will use direct heap allocation.
        /// Adjust these values based on your application's performance and memory usage characteristics.
        /// </summary>
        public static class StringBuilder
        {
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
            public static int MaxStackAllocLength
            {
                get => _maxStackAllocLength;
                set
                {
                    EnsureNotSealed();

                    ValidateMaxStackAllocLengthNewValue(value);

                    _maxStackAllocLength = value;

                    if (EnableDebugLogs)
                    {
                        Console.WriteLine(
                            $"[{nameof(StringEnricherSettings)}.{nameof(Extensions)}.{nameof(StringBuilder)}].{nameof(MaxStackAllocLength)} set to {value}." +
                            $"\n{Environment.StackTrace}");
                    }
                }
            }

            private static int _maxStackAllocLength = 512;

            /// <summary>
            /// Validates the new value for MaxStackAllocLength.
            /// </summary>
            /// <param name="value">New MaxStackAllocLength value.</param>
            private static void ValidateMaxStackAllocLengthNewValue(int value)
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
                        $"{nameof(MaxStackAllocLength)} cannot be greater than {nameof(MaxPooledArrayLength)} ({_maxPooledArrayLength}).");
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
                if (EnableDebugLogs && value < softLowerLimit)
                {
                    // warn about very low values
                    Console.WriteLine(
                        $"WARNING: [{nameof(StringEnricherSettings)}.{nameof(Extensions)}.{nameof(StringBuilder)}].{nameof(MaxStackAllocLength)} is set to a very low value ({value}). " +
                        $"This may lead to increased heap allocations and reduced performance. Consider setting it to at least {softLowerLimit}.");
                }

                const int softUpperLimit = 1024; // Arbitrary soft upper limit to prevent excessive stack allocations.
                if (EnableDebugLogs && value > softUpperLimit)
                {
                    // warn about high values
                    Console.WriteLine(
                        $"WARNING: [{nameof(StringEnricherSettings)}.{nameof(Extensions)}.{nameof(StringBuilder)}].{nameof(MaxStackAllocLength)} is set to a high value ({value}). " +
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
            public static int MaxPooledArrayLength
            {
                get => _maxPooledArrayLength;
                set
                {
                    EnsureNotSealed();

                    ValidateMaxPooledArrayLengthNewValue(value);

                    _maxPooledArrayLength = value;

                    if (EnableDebugLogs)
                    {
                        Console.WriteLine(
                            $"[{nameof(StringEnricherSettings)}.{nameof(Extensions)}.{nameof(StringBuilder)}].{nameof(MaxPooledArrayLength)} set to {value}." +
                            $"\n{Environment.StackTrace}");
                    }
                }
            }

            private static int _maxPooledArrayLength = 1_000_000;

            private static void ValidateMaxPooledArrayLengthNewValue(int value)
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
                if (EnableDebugLogs && value < softLowerLimit)
                {
                    // warn about very low values
                    Console.WriteLine(
                        $"WARNING: [{nameof(StringEnricherSettings)}.{nameof(Extensions)}.{nameof(StringBuilder)}].{nameof(MaxPooledArrayLength)} is set to a very low value ({value}). " +
                        $"This may lead to increased heap allocations and reduced performance. Consider setting it to at least {softLowerLimit}.");
                }

                const int
                    softUpperLimit = 5_242_880; // Arbitrary soft upper limit to prevent excessive pooled arrays (5 MB).
                if (EnableDebugLogs && value > softUpperLimit)
                {
                    // warn about high values
                    Console.WriteLine(
                        $"WARNING: [{nameof(StringEnricherSettings)}.{nameof(Extensions)}.{nameof(StringBuilder)}].{nameof(MaxPooledArrayLength)} is set to a high value ({value}). " +
                        $"This may lead to increased memory usage and pressure on the garbage collector. Consider setting it to no more than {softUpperLimit}.");
                }
            }

            #endregion
        }
    }
}