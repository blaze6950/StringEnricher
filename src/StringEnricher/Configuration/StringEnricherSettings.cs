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
    internal static void EnsureNotSealed()
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
            private static BufferSizeSettingEntry _bufferSizeSettingEntry = new(nameof(StringBuilder), 512, 1_000_000);

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
                get => _bufferSizeSettingEntry.MaxStackAllocLength;
                set => _bufferSizeSettingEntry.MaxStackAllocLength = value;
            }

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
                get => _bufferSizeSettingEntry.MaxPooledArrayLength;
                set => _bufferSizeSettingEntry.MaxPooledArrayLength = value;
            }
        }
    }
}