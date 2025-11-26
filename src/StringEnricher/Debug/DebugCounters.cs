using StringEnricher.Buffer;
using StringEnricher.Buffer.Processors;
using StringEnricher.Nodes.Shared;

namespace StringEnricher.Debug;

#if UNIT_TESTS
/// <summary>
/// Contains debug counters for various components of the StringEnricher library.
/// This class is intended for internal use and debugging purposes only - to propagate events to the test environment.
/// In release builds, these counters should have no effect on performance or behavior due to compiler optimizations.
/// </summary>
public class DebugCounters
{
    /// <summary>
    /// Counts the number of times <see cref="StringMaterializer{TValue}.Process"/> was called.
    /// </summary>
    public static int StringMaterializer_Process_Calls = 0;

    /// <summary>
    /// Counts the number of times the cached total length evaluation was used in <see cref="CompositeNode{T, TT}.TryGetChar"/>.
    /// </summary>
    public static int CompositeNode_TryGetChar_CachedTotalLengthEvaluation = 0;

    /// <summary>
    /// Counts the number of times the cached total length evaluation was used in <see cref="IntegerNode.TryGetChar"/>.
    /// </summary>
    public static int UShortNode_TryGetChar_CachedTotalLengthEvaluation = 0;

    /// <summary>
    /// Counts the number of times the cached total length evaluation was used in <see cref="GuidNode.TryGetChar"/>.
    /// </summary>
    public static int ULongNode_TryGetChar_CachedTotalLengthEvaluation = 0;

    /// <summary>
    /// Counts the number of times the cached total length evaluation was used in <see cref="UIntegerNode.TryGetChar"/>.
    /// </summary>
    public static int UIntegerNode_TryGetChar_CachedTotalLengthEvaluation = 0;

    /// <summary>
    /// Counts the number of times the cached total length evaluation was used in <see cref="ShortNode.TryGetChar"/>.
    /// </summary>
    public static int TimeSpanNode_TryGetChar_CachedTotalLengthEvaluation = 0;

    /// <summary>
    /// Counts the number of times the cached total length evaluation was used in <see cref="TimeOnlyNode.TryGetChar"/>.
    /// </summary>
    public static int TimeOnlyNode_TryGetChar_CachedTotalLengthEvaluation = 0;

    /// <summary>
    /// Counts the number of times the cached total length evaluation was used in <see cref="ShortNode.TryGetChar"/>.
    /// </summary>
    public static int ShortNode_TryGetChar_CachedTotalLengthEvaluation = 0;

    /// <summary>
    /// Counts the number of times the cached total length evaluation was used in <see cref="SByteNode.TryGetChar"/>.
    /// </summary>
    public static int SByteNode_TryGetChar_CachedTotalLengthEvaluation = 0;

    /// <summary>
    /// Counts the number of times the cached total length evaluation was used in <see cref="LongNode.TryGetChar"/>.
    /// </summary>
    public static int LongNode_TryGetChar_CachedTotalLengthEvaluation = 0;

    /// <summary>
    /// Counts the number of times the cached total length evaluation was used in <see cref="IntegerNode.TryGetChar"/>.
    /// </summary>
    public static int IntegerNode_TryGetChar_CachedTotalLengthEvaluation = 0;

    /// <summary>
    /// Counts the number of times the cached total length evaluation was used in <see cref="GuidNode.TryGetChar"/>.
    /// </summary>
    public static int GuidNode_TryGetChar_CachedTotalLengthEvaluation = 0;

    /// <summary>
    /// Counts the number of times the cached total length evaluation was used in <see cref="FloatNode.TryGetChar"/>.
    /// </summary>
    public static int FloatNode_TryGetChar_CachedTotalLengthEvaluation = 0;

    /// <summary>
    /// Counts the number of times the cached total length evaluation was used in <see cref="EnumNode{T}.TryGetChar"/>.
    /// </summary>
    public static int EnumNode_TryGetChar_CachedTotalLengthEvaluation = 0;

    /// <summary>
    /// Counts the number of times the cached total length evaluation was used in <see cref="DoubleNode.TryGetChar"/>.
    /// </summary>
    public static int DoubleNode_TryGetChar_CachedTotalLengthEvaluation = 0;

    /// <summary>
    /// Counts the number of times the cached total length evaluation was used in <see cref="DecimalNode.TryGetChar"/>.
    /// </summary>
    public static int DecimalNode_TryGetChar_CachedTotalLengthEvaluation = 0;

    /// <summary>
    /// Counts the number of times the cached total length evaluation was used in <see cref="DateTimeOffsetNode.TryGetChar"/>.
    /// </summary>
    public static int DateTimeOffsetNode_TryGetChar_CachedTotalLengthEvaluation = 0;

    /// <summary>
    /// Counts the number of times the cached total length evaluation was used in <see cref="DateTimeNode.TryGetChar"/>.
    /// </summary>
    public static int DateTimeNode_TryGetChar_CachedTotalLengthEvaluation = 0;

    /// <summary>
    /// Counts the number of times the cached total length evaluation was used in <see cref="DateOnlyNode.TryGetChar"/>.
    /// </summary>
    public static int DateOnlyNode_TryGetChar_CachedTotalLengthEvaluation = 0;

    /// <summary>
    /// Counts the number of times the cached total length evaluation was used in <see cref="ByteNode.TryGetChar"/>.
    /// </summary>
    public static int ByteNode_TryGetChar_CachedTotalLengthEvaluation = 0;

    /// <summary>
    /// Counts the number of heap allocations made in <see cref="BufferUtils.TryAllocateAndProcess{TState, TResult}"/>
    /// </summary>
    public static int BufferUtils_TryAllocateAndProcess_HeapAllocCount = 0;

    /// <summary>
    /// Counts the number of array pool allocations made in <see cref="BufferUtils.TryAllocateAndProcess{TState, TResult}"/>
    /// </summary>
    public static int BufferUtils_TryAllocateAndProcess_ArrayPoolAllocCount = 0;

    /// <summary>
    /// Counts the number of stack allocations made in <see cref="BufferUtils.TryAllocateAndProcess{TState, TResult}"/>
    /// </summary>
    public static int BufferUtils_TryAllocateAndProcess_StackAllocCount = 0;

    /// <summary>
    /// Counts the number of heap allocations made in <see cref="BufferUtils.StreamBuffer"/>
    /// </summary>
    public static int BufferUtils_StreamBuffer_HeapAllocCount = 0;

    /// <summary>
    /// Counts the number of array pool allocations made in <see cref="BufferUtils.StreamBuffer"/>
    /// </summary>
    public static int BufferUtils_StreamBuffer_ArrayPoolAllocCount = 0;

    /// <summary>
    /// Counts the number of stack allocations made in <see cref="BufferUtils.StreamBuffer"/>
    /// </summary>
    public static int BufferUtils_StreamBuffer_StackAllocCount = 0;

    /// <summary>
    /// Resets all debug counters to their initial state.
    /// </summary>
    public static void ResetAllCounters()
    {
        BufferUtils_StreamBuffer_StackAllocCount = 0;
        BufferUtils_StreamBuffer_ArrayPoolAllocCount = 0;
        BufferUtils_StreamBuffer_HeapAllocCount = 0;

        BufferUtils_TryAllocateAndProcess_StackAllocCount = 0;
        BufferUtils_TryAllocateAndProcess_ArrayPoolAllocCount = 0;
        BufferUtils_TryAllocateAndProcess_HeapAllocCount = 0;

        ByteNode_TryGetChar_CachedTotalLengthEvaluation = 0;
        DateOnlyNode_TryGetChar_CachedTotalLengthEvaluation = 0;
        DateTimeNode_TryGetChar_CachedTotalLengthEvaluation = 0;
        DateTimeOffsetNode_TryGetChar_CachedTotalLengthEvaluation = 0;
        DecimalNode_TryGetChar_CachedTotalLengthEvaluation = 0;
        DoubleNode_TryGetChar_CachedTotalLengthEvaluation = 0;
        EnumNode_TryGetChar_CachedTotalLengthEvaluation = 0;
        FloatNode_TryGetChar_CachedTotalLengthEvaluation = 0;
        GuidNode_TryGetChar_CachedTotalLengthEvaluation = 0;
        IntegerNode_TryGetChar_CachedTotalLengthEvaluation = 0;
        LongNode_TryGetChar_CachedTotalLengthEvaluation = 0;
        SByteNode_TryGetChar_CachedTotalLengthEvaluation = 0;
        ShortNode_TryGetChar_CachedTotalLengthEvaluation = 0;
        TimeOnlyNode_TryGetChar_CachedTotalLengthEvaluation = 0;
        TimeSpanNode_TryGetChar_CachedTotalLengthEvaluation = 0;
        UIntegerNode_TryGetChar_CachedTotalLengthEvaluation = 0;
        ULongNode_TryGetChar_CachedTotalLengthEvaluation = 0;
        UShortNode_TryGetChar_CachedTotalLengthEvaluation = 0;
        CompositeNode_TryGetChar_CachedTotalLengthEvaluation = 0;

        StringMaterializer_Process_Calls = 0;
    }
}
#endif