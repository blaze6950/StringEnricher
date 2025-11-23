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
        /// The allocation settings are optimized for small byte arrays - the allocation is done on stack.
        /// The allocation using array pool and heap disabled by default.
        /// </summary>
        private static NodeSettings GetDefaultStringBuilderSettings() => new(
            name: $"{Name}.{nameof(StringBuilder)}",
            initialBufferLength: 4, maxBufferLength: 1_000_000, growthFactor: 2,
            maxStackAllocLength: 2048, maxPooledArrayLength: 1_000_000
        );

        /// <summary>
        /// Configuration settings for StringBuilder-related optimizations.
        /// These settings help balance performance and memory usage when appending StringEnricher nodes to StringBuilder.
        /// - MaxStackAllocLength: The maximum length of a node that can be allocated on the stack (default: 2048).
        /// - MaxPooledArrayLength: The maximum length of a node that can use array pooling (default: 1_000_000).
        /// Nodes larger than this will use direct heap allocation.
        /// Adjust these values based on your application's performance and memory usage characteristics.
        /// </summary>
        public static NodeSettings StringBuilder = GetDefaultStringBuilderSettings();

        /// <summary>
        /// Resets the <see cref="StringBuilder"/> settings to their default values.
        /// </summary>
        public static void ResetStringBuilderSettings() => StringBuilder = GetDefaultStringBuilderSettings();
    }
}