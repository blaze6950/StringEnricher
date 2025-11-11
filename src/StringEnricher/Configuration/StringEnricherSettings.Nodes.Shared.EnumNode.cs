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
            private static NodeSettings GetDefaultEnumNodeSettings() => new(
                name: $"{Name}.{nameof(EnumNode)}",
                initialBufferLength: 16, maxBufferLength: 512, growthFactor: 2,
                maxStackAllocLength: 512, maxPooledArrayLength: 512
            );

            /// <summary>
            /// Configuration settings for <see cref="EnumNode"/>.
            /// </summary>
            public static NodeSettings EnumNode = GetDefaultEnumNodeSettings();

            /// <summary>
            /// Resets the <see cref="EnumNode"/> settings to their default values.
            /// </summary>
            public static void ResetEnumNodeSettings() => EnumNode = GetDefaultEnumNodeSettings();
        }
    }
}