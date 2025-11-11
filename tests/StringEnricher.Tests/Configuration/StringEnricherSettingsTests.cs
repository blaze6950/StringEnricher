using StringEnricher.Configuration;

namespace StringEnricher.Tests.Configuration;

/// <summary>
/// Collection definition to disable parallelization for tests that modify static settings.
/// This ensures that tests do not interfere with each other by modifying shared state.
/// </summary>
[CollectionDefinition("NonParallelTests", DisableParallelization = true)]
public class NonParallelTestsCollection;

/// <summary>
/// Unit tests for StringEnricherSettings class.
/// </summary>
[Collection("NonParallelTests")]
public partial class StringEnricherSettingsTests
{
    /// <summary>
    /// Resets the settings to their default state before each test.
    /// This ensures test isolation by unsealing settings and restoring defaults.
    /// </summary>
    private static void ResetSettings()
    {
        // Use reflection to reset the sealed state for testing
        var sealedField = typeof(StringEnricherSettings).GetField("_isSealed",
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
        sealedField?.SetValue(null, false);

        // Reset to default values
        StringEnricherSettings.EnableDebugLogs = true;
    }

    [Collection("NonParallelTests")]
    public class SealingTests
    {
        public SealingTests()
        {
            ResetSettings();
        }

        [Fact]
        public void Seal_ShouldPreventFurtherModifications()
        {
            // Arrange & Act
            StringEnricherSettings.Seal();

            // Assert
            var exception = Record.Exception(() =>
                StringEnricherSettings.EnableDebugLogs = false);
            Assert.Null(exception);

            Assert.Throws<InvalidOperationException>(() =>
                StringEnricherSettings.Extensions.StringBuilder.MaxStackAllocLength = 256);

            Assert.Throws<InvalidOperationException>(() =>
                StringEnricherSettings.Extensions.StringBuilder.MaxPooledArrayLength = 500_000);
        }

        [Fact]
        public void Seal_ExceptionMessage_ShouldBeDescriptive()
        {
            // Arrange
            StringEnricherSettings.Seal();

            // Act & Assert
            var exception = Assert.Throws<InvalidOperationException>(() =>
                StringEnricherSettings.Extensions.StringBuilder.MaxStackAllocLength = 256);

            Assert.Equal("StringEnricherSettings is sealed and cannot be modified.", exception.Message);
        }

        [Fact]
        public void Settings_WhenNotSealed_ShouldAllowModifications()
        {
            // Arrange
            // Settings are not sealed by default

            // Act & Assert - Should not throw
            StringEnricherSettings.EnableDebugLogs = false;
            StringEnricherSettings.Extensions.StringBuilder.MaxStackAllocLength = 256;
            StringEnricherSettings.Extensions.StringBuilder.MaxPooledArrayLength = 500_000;

            Assert.False(StringEnricherSettings.EnableDebugLogs);
            Assert.Equal(256, StringEnricherSettings.Extensions.StringBuilder.MaxStackAllocLength);
            Assert.Equal(500_000, StringEnricherSettings.Extensions.StringBuilder.MaxPooledArrayLength);
        }
    }

    [Collection("NonParallelTests")]
    public class EnableDebugLogsTests
    {
        public EnableDebugLogsTests()
        {
            ResetSettings();
        }

        [Fact]
        public void EnableDebugLogs_DefaultValue_ShouldBeTrue()
        {
            // Assert
            Assert.True(StringEnricherSettings.EnableDebugLogs);
        }

        [Fact]
        public void EnableDebugLogs_CanBeSetToFalse()
        {
            // Act
            StringEnricherSettings.EnableDebugLogs = false;

            // Assert
            Assert.False(StringEnricherSettings.EnableDebugLogs);
        }

        [Fact]
        public void EnableDebugLogs_CanBeSetToTrue()
        {
            // Arrange
            StringEnricherSettings.EnableDebugLogs = false;

            // Act
            StringEnricherSettings.EnableDebugLogs = true;

            // Assert
            Assert.True(StringEnricherSettings.EnableDebugLogs);
        }
    }
}