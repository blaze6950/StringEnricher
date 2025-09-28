namespace StringEnricher.Configuration;

/// <summary>
/// Configuration settings for StringEnricher.
/// <br/><br/>
/// WARNING! Change these settings only if you fully understand the implications.
/// Improper changes may lead to performance degradation or increased memory usage. Or even bugs.
/// It's recommended to keep the default values unless you have specific requirements and have thoroughly tested the changes.
/// Once sealed, the settings cannot be modified further.
/// <br/><br/>
/// Recommendation: Configure these settings at the start of your application, before using any StringEnricher functionality.
/// Then call <see cref="Seal()"/> after initial configuration to prevent accidental changes at runtime.
/// </summary>
public static partial class StringEnricherSettings
{
    private const string Name = nameof(StringEnricherSettings);

    #region Sealed State

    /// <summary>
    /// Seals the settings to prevent further modifications.
    /// Once sealed, any attempt to change settings will throw an InvalidOperationException.
    /// </summary>
    public static void Seal() => _isSealed = true;

    private static bool _isSealed = false;

    /// <summary>
    /// Ensures that the settings are not sealed before allowing modifications.
    /// </summary>
    /// <exception cref="InvalidOperationException">
    /// Thrown if the settings are sealed.
    /// </exception>
    internal static void EnsureNotSealed()
    {
        if (_isSealed)
        {
            throw new InvalidOperationException("StringEnricherSettings is sealed and cannot be modified.");
        }
    }

    #endregion

    /// <summary>
    /// Enables or disables debug logging for StringEnricher.
    /// When enabled, detailed debug information will be logged to help diagnose issues.
    /// Default is true (enabled).
    /// </summary>
    public static bool EnableDebugLogs { get; set; } = true;

    /// <summary>
    /// Configuration settings for various nodes used in StringEnricher.
    /// </summary>
    public static partial class Nodes
    {
        private const string Name = $"{StringEnricherSettings.Name}.{nameof(Nodes)}";

        /// <summary>
        /// Configuration settings for shared nodes used across multiple styles.
        /// These settings help tune performance and memory usage for common node types.
        /// </summary>
        public static partial class Shared
        {
            private const string Name = $"{Nodes.Name}.{nameof(Shared)}";
        }
    }
}