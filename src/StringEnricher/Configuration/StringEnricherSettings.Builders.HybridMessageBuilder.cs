namespace StringEnricher.Configuration;

public static partial class StringEnricherSettings
{
    /// <summary>
    /// Configuration settings for various builders used in StringEnricher.
    /// </summary>
    public static partial class Builders
    {
        private const string Name = $"{StringEnricherSettings.Name}.{nameof(Builders)}";

        /// <summary>
        /// Gets the default settings for the HybridMessageBuilder.
        /// </summary>
        private static NodeSettings GetDefaultHybridMessageBuilderSettings() => new(
            name: $"{Name}.{nameof(HybridMessageBuilder)}",
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
        public static NodeSettings HybridMessageBuilder = GetDefaultHybridMessageBuilderSettings();

        /// <summary>
        /// Resets the <see cref="HybridMessageBuilder"/> settings to their default values.
        /// </summary>
        public static void ResetHybridMessageBuilderSettings() => HybridMessageBuilder = GetDefaultHybridMessageBuilderSettings();
    }
}