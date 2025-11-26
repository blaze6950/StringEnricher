using StringEnricher.Configuration;

namespace StringEnricher.Tests.Configuration;

public partial class StringEnricherSettingsTests
{
    public class HybridMessageBuilderTests
    {
        [Fact]
        public void HybridMessageBuilder_InitializesWithDefaultValues()
        {
            // Arrange & Act
            StringEnricherSettings.Builders.ResetHybridMessageBuilderSettings();
            var settings = StringEnricherSettings.Builders.HybridMessageBuilder;

            // Assert
            Assert.Equal(2048, settings.MaxStackAllocLength);
            Assert.Equal(1_000_000, settings.MaxPooledArrayLength);
        }

        [Fact]
        public void HybridMessageBuilder_CanBeModified()
        {
            // Arrange
            StringEnricherSettings.EnableDebugLogs = false;
            StringEnricherSettings.Builders.ResetHybridMessageBuilderSettings();

            // Act
            StringEnricherSettings.Builders.HybridMessageBuilder.MaxStackAllocLength = 1024;
            StringEnricherSettings.Builders.HybridMessageBuilder.MaxPooledArrayLength = 2_000_000;

            // Assert
            Assert.Equal(1024, StringEnricherSettings.Builders.HybridMessageBuilder.MaxStackAllocLength);
            Assert.Equal(2_000_000, StringEnricherSettings.Builders.HybridMessageBuilder.MaxPooledArrayLength);

            // Cleanup
            StringEnricherSettings.Builders.ResetHybridMessageBuilderSettings();
        }

        [Fact]
        public void ResetHybridMessageBuilderSettings_RestoresDefaultValues()
        {
            // Arrange
            StringEnricherSettings.EnableDebugLogs = false;
            StringEnricherSettings.Builders.HybridMessageBuilder.MaxStackAllocLength = 2048;
            StringEnricherSettings.Builders.HybridMessageBuilder.MaxPooledArrayLength = 5_000_000;

            // Act
            StringEnricherSettings.Builders.ResetHybridMessageBuilderSettings();

            // Assert
            Assert.Equal(2048, StringEnricherSettings.Builders.HybridMessageBuilder.MaxStackAllocLength);
            Assert.Equal(1_000_000, StringEnricherSettings.Builders.HybridMessageBuilder.MaxPooledArrayLength);
        }

        [Fact]
        public void ResetHybridMessageBuilderSettings_CanBeCalledMultipleTimes()
        {
            // Arrange & Act
            StringEnricherSettings.Builders.ResetHybridMessageBuilderSettings();
            StringEnricherSettings.Builders.ResetHybridMessageBuilderSettings();
            var settings = StringEnricherSettings.Builders.HybridMessageBuilder;

            // Assert
            Assert.Equal(2048, settings.MaxStackAllocLength);
            Assert.Equal(1_000_000, settings.MaxPooledArrayLength);
        }

        [Fact]
        public void HybridMessageBuilder_ModificationPersistsAcrossMultipleAccesses()
        {
            // Arrange
            StringEnricherSettings.EnableDebugLogs = false;
            StringEnricherSettings.Builders.ResetHybridMessageBuilderSettings();
            StringEnricherSettings.Builders.HybridMessageBuilder.MaxStackAllocLength = 256;

            // Act
            var firstAccess = StringEnricherSettings.Builders.HybridMessageBuilder.MaxStackAllocLength;
            var secondAccess = StringEnricherSettings.Builders.HybridMessageBuilder.MaxStackAllocLength;

            // Assert
            Assert.Equal(256, firstAccess);
            Assert.Equal(256, secondAccess);

            // Cleanup
            StringEnricherSettings.Builders.ResetHybridMessageBuilderSettings();
        }

        [Fact]
        public void HybridMessageBuilder_Defaults_AllValuesAreExpected()
        {
            // Arrange
            StringEnricherSettings.Builders.ResetHybridMessageBuilderSettings();

            // Act
            var settings = StringEnricherSettings.Builders.HybridMessageBuilder;

            // Assert - all defaults from GetDefaultHybridMessageBuilderSettings
            Assert.Equal(4, settings.InitialBufferSize);
            Assert.Equal(1_000_000, settings.MaxBufferSize);
            Assert.Equal(2f, settings.GrowthFactor);
            Assert.Equal(2048, settings.MaxStackAllocLength);
            Assert.Equal(1_000_000, settings.MaxPooledArrayLength);

            // StringEnricherSettings.Name is private; assert against the known composed literal
            Assert.Equal("StringEnricherSettings.Builders.HybridMessageBuilder", settings.Name);
        }
    }
}
