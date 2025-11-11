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
            private static NodeSettings GetDefaultDateTimeOffsetNodeSettings() => new(
                name: $"{Name}.{nameof(DateTimeOffsetNode)}",
                initialBufferLength: 32, maxBufferLength: 512, growthFactor: 2,
                maxStackAllocLength: 512, maxPooledArrayLength: 512
            );

            /// <summary>
            /// Configuration settings for <see cref="DateTimeOffsetNode"/>.
            /// </summary>
            public static NodeSettings DateTimeOffsetNode = GetDefaultDateTimeOffsetNodeSettings();

            /// <summary>
            /// Resets the <see cref="DateTimeOffsetNode"/> settings to their default values.
            /// </summary>
            public static void ResetDateTimeOffsetNodeSettings() =>
                DateTimeOffsetNode = GetDefaultDateTimeOffsetNodeSettings();
        }
    }
}