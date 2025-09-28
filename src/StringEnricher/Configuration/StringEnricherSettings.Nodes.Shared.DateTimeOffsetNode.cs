namespace StringEnricher.Configuration;

public static partial class StringEnricherSettings
{
    public static partial class Nodes
    {
        public static partial class Shared
        {
            /// <summary>
            /// Configuration settings for <see cref="DateTimeOffsetNode"/>.
            /// </summary>
            public static class DateTimeOffsetNode
            {
                private const string Name = $"{Shared.Name}.{nameof(DateTimeOffsetNode)}";

                #region BufferSizes

                private static BufferSizes _bufferSizes = new(Name, 32, 512);

                /// <summary>
                /// The growth factor to use when increasing buffer sizes for formatting a <see cref="DateTimeOffsetNode"/>.
                /// This factor determines how quickly the buffer size increases when the initial buffer is insufficient.
                /// Must be greater than 1.0. Default is 2.0 (doubling the buffer size each time).
                /// </summary>
                public static float GrowthFactor
                {
                    get => _bufferSizes.GrowthFactor;
                    set => _bufferSizes.GrowthFactor = value;
                }

                /// <summary>
                /// The initial buffer length to use when formatting a <see cref="DateTimeOffsetNode"/>.
                /// This buffer is used to attempt formatting the node before falling back to larger buffers.
                /// Default is 32 characters.
                /// </summary>
                public static int InitialBufferSize
                {
                    get => _bufferSizes.InitialBufferLength;
                    set => _bufferSizes.InitialBufferLength = value;
                }

                /// <summary>
                /// The maximum buffer length to use when formatting a <see cref="DateTimeOffsetNode"/>.
                /// If formatting requires a buffer larger than this, an exception will be thrown.
                /// Default is 512 characters.
                /// </summary>
                public static int MaxBufferSize
                {
                    get => _bufferSizes.MaxBufferLength;
                    set => _bufferSizes.MaxBufferLength = value;
                }

                #endregion

                #region BufferAllocationThresholds

                private static BufferAllocationThresholds _bufferAllocationThresholds = new(Name);

                /// <summary>
                /// The maximum length of a buffer that can be allocated on the stack when formatting a <see cref="DateTimeOffsetNode"/>.
                /// Buffers larger than this will be allocated on the heap.
                /// Default is 256 characters.
                /// </summary>
                public static int MaxStackAllocLength
                {
                    get => _bufferAllocationThresholds.MaxStackAllocLength;
                    set => _bufferAllocationThresholds.MaxStackAllocLength = value;
                }

                /// <summary>
                /// The maximum length of a buffer that can be rented from the array pool when formatting a <see cref="DateTimeOffsetNode"/>.
                /// Buffers larger than this will be allocated on the heap.
                /// Default is 8192 characters.
                /// </summary>
                public static int MaxPooledArrayLength
                {
                    get => _bufferAllocationThresholds.MaxPooledArrayLength;
                    set => _bufferAllocationThresholds.MaxPooledArrayLength = value;
                }

                #endregion
            }
        }
    }
}