namespace StringEnricher.Configuration;

public static partial class StringEnricherSettings
{
    public static partial class Nodes
    {
        public static partial class Shared
        {
            /// <summary>
            /// The allocation settings are optimized for small byte arrays - the allocation is done on stack.
            /// The allocation using array pool and heap disabled by default.
            /// </summary>
            private static NodeSettings GetDefaultDateOnlyNodeSettings() => new(
                name: $"{Name}.{nameof(DateOnlyNode)}",
                initialBufferLength: 16, maxBufferLength: 256, growthFactor: 2,
                maxStackAllocLength: 256, maxPooledArrayLength: 256
            );

            /// <summary>
            /// Configuration settings for <see cref="DateOnlyNode"/>.
            /// </summary>
            public static NodeSettings DateOnlyNode = GetDefaultDateOnlyNodeSettings();

            /// <summary>
            /// Resets the <see cref="DateOnlyNode"/> settings to their default values.
            /// </summary>
            public static void ResetDateOnlyNodeSettings() => DateOnlyNode = GetDefaultDateOnlyNodeSettings();
        }
    }
}