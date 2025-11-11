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
            private static NodeSettings GetDefaultIntegerNodeSettings() => new(
                name: $"{Name}.{nameof(IntegerNode)}",
                initialBufferLength: 16, maxBufferLength: 64, growthFactor: 2,
                maxStackAllocLength: 64, maxPooledArrayLength: 64
            );

            /// <summary>
            /// Configuration settings for <see cref="IntegerNode"/>.
            /// </summary>
            public static NodeSettings IntegerNode = GetDefaultIntegerNodeSettings();

            /// <summary>
            /// Resets the <see cref="IntegerNode"/> settings to their default values.
            /// </summary>
            public static void ResetIntegerNodeSettings() => IntegerNode = GetDefaultIntegerNodeSettings();
        }
    }
}