using StringEnricher.Configuration;

namespace StringEnricher.Tests.Configuration;

public partial class StringEnricherSettingsTests
{
    public class StringBuilderTests
    {
        [Fact]
        public void StringBuilder_InitializesWithDefaultValues()
        {
            // Arrange & Act
            StringEnricherSettings.Extensions.ResetStringBuilderSettings();
            var settings = StringEnricherSettings.Extensions.StringBuilder;

            // Assert
            Assert.Equal(512, settings.MaxStackAllocLength);
            Assert.Equal(1_000_000, settings.MaxPooledArrayLength);
        }

        [Fact]
        public void StringBuilder_CanBeModified()
        {
            // Arrange
            StringEnricherSettings.EnableDebugLogs = false;
            StringEnricherSettings.Extensions.ResetStringBuilderSettings();

            // Act
            StringEnricherSettings.Extensions.StringBuilder.MaxStackAllocLength = 1024;
            StringEnricherSettings.Extensions.StringBuilder.MaxPooledArrayLength = 2_000_000;

            // Assert
            Assert.Equal(1024, StringEnricherSettings.Extensions.StringBuilder.MaxStackAllocLength);
            Assert.Equal(2_000_000, StringEnricherSettings.Extensions.StringBuilder.MaxPooledArrayLength);

            // Cleanup
            StringEnricherSettings.Extensions.ResetStringBuilderSettings();
        }

        [Fact]
        public void ResetStringBuilderSettings_RestoresDefaultValues()
        {
            // Arrange
            StringEnricherSettings.EnableDebugLogs = false;
            StringEnricherSettings.Extensions.StringBuilder.MaxStackAllocLength = 2048;
            StringEnricherSettings.Extensions.StringBuilder.MaxPooledArrayLength = 5_000_000;

            // Act
            StringEnricherSettings.Extensions.ResetStringBuilderSettings();

            // Assert
            Assert.Equal(512, StringEnricherSettings.Extensions.StringBuilder.MaxStackAllocLength);
            Assert.Equal(1_000_000, StringEnricherSettings.Extensions.StringBuilder.MaxPooledArrayLength);
        }

        [Fact]
        public void ResetStringBuilderSettings_CanBeCalledMultipleTimes()
        {
            // Arrange & Act
            StringEnricherSettings.Extensions.ResetStringBuilderSettings();
            StringEnricherSettings.Extensions.ResetStringBuilderSettings();
            var settings = StringEnricherSettings.Extensions.StringBuilder;

            // Assert
            Assert.Equal(512, settings.MaxStackAllocLength);
            Assert.Equal(1_000_000, settings.MaxPooledArrayLength);
        }

        [Fact]
        public void StringBuilder_ModificationPersistsAcrossMultipleAccesses()
        {
            // Arrange
            StringEnricherSettings.EnableDebugLogs = false;
            StringEnricherSettings.Extensions.ResetStringBuilderSettings();
            StringEnricherSettings.Extensions.StringBuilder.MaxStackAllocLength = 256;

            // Act
            var firstAccess = StringEnricherSettings.Extensions.StringBuilder.MaxStackAllocLength;
            var secondAccess = StringEnricherSettings.Extensions.StringBuilder.MaxStackAllocLength;

            // Assert
            Assert.Equal(256, firstAccess);
            Assert.Equal(256, secondAccess);

            // Cleanup
            StringEnricherSettings.Extensions.ResetStringBuilderSettings();
        }
    }
}