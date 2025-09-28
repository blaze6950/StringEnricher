namespace StringEnricher.Configuration;

public static partial class StringEnricherSettings
{
    /// <summary>
    /// Configuration settings for various extensions used in StringEnricher.
    /// </summary>
    public static partial class Extensions
    {
        private const string Name = $"{StringEnricherSettings.Name}.{nameof(Extensions)}";

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
            private const string Name = $"{Extensions.Name}.{nameof(StringBuilder)}";

            private static BufferAllocationThresholds _bufferAllocationThresholds = new(Name, 512, 1_000_000);

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
                get => _bufferAllocationThresholds.MaxStackAllocLength;
                set => _bufferAllocationThresholds.MaxStackAllocLength = value;
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
                get => _bufferAllocationThresholds.MaxPooledArrayLength;
                set => _bufferAllocationThresholds.MaxPooledArrayLength = value;
            }
        }
    }
}