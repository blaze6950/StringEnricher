using StringEnricher.Nodes.Shared;

namespace StringEnricher.Configuration;

/// <summary>
/// Configuration settings for a node.
/// </summary>
public struct NodeSettings
{
    /// <summary>
    /// The name of the node.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// The buffer sizes to use for the node.
    /// </summary>
    private BufferSizes _bufferSizes;

    /// <summary>
    /// The buffer allocation thresholds to use for the node.
    /// </summary>
    private BufferAllocationThresholds _bufferAllocationThresholds;

    /// <summary>
    /// Initializes a new instance of the <see cref="NodeSettings"/> struct.
    /// </summary>
    public NodeSettings(
        string name,
        BufferSizes bufferSizes,
        BufferAllocationThresholds bufferAllocationThresholds
    )
    {
        Name = name;
        _bufferSizes = bufferSizes;
        _bufferAllocationThresholds = bufferAllocationThresholds;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="NodeSettings"/> struct.
    /// </summary>
    /// <param name="name">The name of the node.</param>
    /// <param name="initialBufferLength">
    /// The initial length of the buffer to be used for memory allocation.
    /// This value must be greater than zero and less than or equal to <paramref name="maxBufferLength"/>.
    /// </param>
    /// <param name="maxBufferLength">
    /// The maximum length of the buffer to be used for memory allocation when the initial buffer is insufficient.
    /// This value must be greater than zero and greater than or equal to <paramref name="initialBufferLength"/>.
    /// </param>
    /// <param name="growthFactor">
    /// The growth factor to use when increasing buffer sizes.
    /// This factor determines how quickly the buffer size increases when the initial buffer is insufficient.
    /// Must be greater than 1.0.
    /// </param>
    /// <param name="maxStackAllocLength">
    /// The maximum length of the buffer to be allocated on the stack.
    /// If a buffer larger than this is needed, memory won't be allocated on the stack.
    /// If a buffer smaller than or equal to this is needed, it will be allocated on the stack.
    ///
    /// Note: Dual-threshold memory model: two adjustable boundaries define how objects are allocated — to the stack,
    /// array pool, or heap. Shifting the thresholds dynamically redistributes memory ranges between these regions
    /// for optimal performance and balance.
    /// 
    /// |-----------|----------------------|--------------------------->
    /// ^           ^                      ^
    /// 0   MaxStackAllocLength    MaxPooledArrayLength
    /// 
    /// [0, MaxStackAllocLength)                    -> Stack allocation  
    /// [MaxStackAllocLength, MaxPooledArrayLength) -> Array Pool
    /// [MaxPooledArrayLength, ∞)                   -> Heap allocation
    /// </param>
    /// <param name="maxPooledArrayLength">
    /// The maximum length of the buffer to be allocated from the array pool.
    /// If a buffer larger than this is needed, a new array will be allocated instead of renting from the pool.
    /// If a buffer smaller than or equal to this is needed, it will be rented from the pool.
    ///
    /// Note: Dual-threshold memory model: two adjustable boundaries define how objects are allocated — to the stack,
    /// array pool, or heap. Shifting the thresholds dynamically redistributes memory ranges between these regions
    /// for optimal performance and balance.
    /// 
    /// |-----------|----------------------|--------------------------->
    /// ^           ^                      ^
    /// 0   MaxStackAllocLength    MaxPooledArrayLength
    /// 
    /// [0, MaxStackAllocLength)                    -> Stack allocation  
    /// [MaxStackAllocLength, MaxPooledArrayLength) -> Array Pool
    /// [MaxPooledArrayLength, ∞)                   -> Heap allocation
    /// </param>
    public NodeSettings(
        string name,
        int initialBufferLength,
        int maxBufferLength,
        float growthFactor,
        int maxStackAllocLength,
        int maxPooledArrayLength
    ) : this(
        name,
        new BufferSizes(name, initialBufferLength, maxBufferLength, growthFactor),
        new BufferAllocationThresholds(name, maxStackAllocLength, maxPooledArrayLength)
    )
    {
    }

    /// <summary>
    /// The multiplier to use when resizing the buffer for formatting a <see cref="DateOnlyNode"/>.
    /// When the current buffer is insufficient, it will be resized by multiplying its size by this factor.
    /// Default is 2.0 (doubling the buffer size).
    /// Note: A higher multiplier reduces the number of resizing operations but may lead to increased memory usage.
    /// A lower multiplier minimizes memory usage but may increase the number of resizing operations, impacting performance.
    /// Recommended range is between 1.5 and 3.0.
    /// </summary>
    public float GrowthFactor
    {
        get => _bufferSizes.GrowthFactor;
        set => _bufferSizes.GrowthFactor = value;
    }

    /// <summary>
    /// The initial buffer length to use when formatting a <see cref="DateOnlyNode"/>.
    /// This buffer is used to attempt formatting the node before falling back to larger buffers.
    /// </summary>
    public int InitialBufferSize
    {
        get => _bufferSizes.InitialBufferLength;
        set => _bufferSizes.InitialBufferLength = value;
    }

    /// <summary>
    /// The maximum buffer length to use when formatting a <see cref="DateOnlyNode"/>.
    /// If formatting requires a buffer larger than this, an exception will be thrown.
    /// </summary>
    public int MaxBufferSize
    {
        get => _bufferSizes.MaxBufferLength;
        set => _bufferSizes.MaxBufferLength = value;
    }

    /// <summary>
    /// The maximum length of a buffer that can be allocated on the stack when formatting a <see cref="DateOnlyNode"/>.
    /// Buffers larger than this will be allocated on the heap.
    /// </summary>
    public int MaxStackAllocLength
    {
        get => _bufferAllocationThresholds.MaxStackAllocLength;
        set => _bufferAllocationThresholds.MaxStackAllocLength = value;
    }

    /// <summary>
    /// The maximum length of a buffer that can be rented from the array pool when formatting a <see cref="DateOnlyNode"/>.
    /// Buffers larger than this will be allocated on the heap.
    /// </summary>
    public int MaxPooledArrayLength
    {
        get => _bufferAllocationThresholds.MaxPooledArrayLength;
        set => _bufferAllocationThresholds.MaxPooledArrayLength = value;
    }
}