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
            private static NodeSettings GetDefaultFloatNodeSettings() => new(
                name: $"{Name}.{nameof(FloatNode)}",
                initialBufferLength: 32, maxBufferLength: 128, growthFactor: 2,
                maxStackAllocLength: 128, maxPooledArrayLength: 128
            );

            /// <summary>
            /// Configuration settings for <see cref="FloatNode"/>.
            /// </summary>
            public static NodeSettings FloatNode = GetDefaultFloatNodeSettings();

            /// <summary>
            /// Resets the <see cref="FloatNode"/> settings to their default values.
            /// </summary>
            public static void ResetFloatNodeSettings() => FloatNode = GetDefaultFloatNodeSettings();
        }
    }
}