using System.Buffers;
using System.Text;
using StringEnricher.Configuration;
using StringEnricher.Nodes;

namespace StringEnricher.Extensions;

/// <summary>
/// Extension methods for StringBuilder to work with StringEnricher nodes.
/// </summary>
public static class StringBuilderExtensions
{
    /// <summary>
    /// Appends the content of a StringEnricher node to a <see cref="StringBuilder"/> efficiently.
    /// This method uses stack allocation for small nodes, array pooling for medium-sized nodes,
    /// and direct heap allocation for large nodes to minimize memory allocations and improve performance.
    /// Note: There is an option to configure the thresholds in <see cref="StringEnricherSettings"/> to balance performance and memory usage in advanced cases.
    /// </summary>
    /// <param name="sb">
    /// The <see cref="StringBuilder"/> to append to.
    /// </param>
    /// <param name="node">
    /// The StringEnricher node to append.
    /// </param>
    /// <typeparam name="TNode">
    /// The type of the StringEnricher node, constrained to <see cref="INode"/>.
    /// </typeparam>
    /// <returns>
    /// The same <see cref="StringBuilder"/> instance for chaining.
    /// </returns>
    public static StringBuilder AppendNode<TNode>(this StringBuilder sb, TNode node) where TNode : INode
    {
        // Efficiently append the content of a StringEnricher node to a StringBuilder.
        if (node.TotalLength <= StringEnricherSettings.Extensions.StringBuilder.MaxStackAllocLength)
        {
            // stackalloc for small sizes (fastest)
            Span<char> buffer = stackalloc char[node.TotalLength];
            node.CopyTo(buffer);
            sb.Append(buffer);
        }
        else if (node.TotalLength <= StringEnricherSettings.Extensions.StringBuilder.MaxPooledArrayLength)
        {
            // array pool for medium sizes (less pressure on the GC)
            var buffer = ArrayPool<char>.Shared.Rent(node.TotalLength);
            try
            {
                var written = node.CopyTo(buffer);
                sb.Append(buffer, 0, written);
            }
            finally
            {
                ArrayPool<char>.Shared.Return(buffer);
            }
        }
        else
        {
            // fallback: direct heap allocation (rare but safe)
            var buffer = new char[node.TotalLength];
            node.CopyTo(buffer);
            sb.Append(buffer);
        }

        return sb;
    }
}