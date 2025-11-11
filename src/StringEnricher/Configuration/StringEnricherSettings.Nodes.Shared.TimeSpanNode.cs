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
            private static NodeSettings GetDefaultTimeSpanNodeSettings() => new(
                name: $"{Name}.{nameof(TimeSpanNode)}",
                initialBufferLength: 32, maxBufferLength: 128, growthFactor: 2,
                maxStackAllocLength: 128, maxPooledArrayLength: 128
            );

            /// <summary>
            /// Configuration settings for <see cref="TimeSpanNode"/>.
            /// </summary>
            public static NodeSettings TimeSpanNode = GetDefaultTimeSpanNodeSettings();

            /// <summary>
            /// Resets the <see cref="TimeSpanNode"/> settings to their default values.
            /// </summary>
            public static void ResetTimeSpanNodeSettings() => TimeSpanNode = GetDefaultTimeSpanNodeSettings();
        }
    }
}