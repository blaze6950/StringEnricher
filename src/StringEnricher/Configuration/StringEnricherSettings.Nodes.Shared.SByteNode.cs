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
            private static NodeSettings GetDefaultSByteNodeSettings() => new(
                name: $"{Name}.{nameof(SByteNode)}",
                initialBufferLength: 4, maxBufferLength: 32, growthFactor: 2,
                maxStackAllocLength: 32, maxPooledArrayLength: 32
            );

            /// <summary>
            /// Configuration settings for <see cref="SByteNode"/>.
            /// </summary>
            public static NodeSettings SByteNode = GetDefaultSByteNodeSettings();

            /// <summary>
            /// Resets the <see cref="SByteNode"/> settings to their default values.
            /// </summary>
            public static void ResetSByteNodeSettings() => SByteNode = GetDefaultSByteNodeSettings();
        }
    }
}