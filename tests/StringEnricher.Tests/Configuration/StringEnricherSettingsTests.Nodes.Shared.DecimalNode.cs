using StringEnricher.Configuration;

namespace StringEnricher.Tests.Configuration;

public partial class StringEnricherSettingsTests
{
    public class DecimalNodeTests
    {
        [Fact]
        public void DecimalNode_InitializesWithDefaultValues()
        {
            // Arrange & Act
            StringEnricherSettings.Nodes.Shared.ResetDecimalNodeSettings();
            var settings = StringEnricherSettings.Nodes.Shared.DecimalNode;

            // Assert
            Assert.Equal("StringEnricherSettings.Nodes.Shared.DecimalNode", settings.Name);
            Assert.Equal(32, settings.InitialBufferSize);
            Assert.Equal(128, settings.MaxBufferSize);
            Assert.Equal(2, settings.GrowthFactor);
            Assert.Equal(128, settings.MaxStackAllocLength);
            Assert.Equal(128, settings.MaxPooledArrayLength);
        }

        [Fact]
        public void DecimalNode_CanBeModified()
        {
            // Arrange
            StringEnricherSettings.EnableDebugLogs = false;
            StringEnricherSettings.Nodes.Shared.ResetDecimalNodeSettings();

            // Act
            StringEnricherSettings.Nodes.Shared.DecimalNode.InitialBufferSize = 64;
            StringEnricherSettings.Nodes.Shared.DecimalNode.MaxBufferSize = 256;
            StringEnricherSettings.Nodes.Shared.DecimalNode.GrowthFactor = 3.0f;
            StringEnricherSettings.Nodes.Shared.DecimalNode.MaxPooledArrayLength = 32768;
            StringEnricherSettings.Nodes.Shared.DecimalNode.MaxStackAllocLength = 512;

            // Assert
            Assert.Equal(64, StringEnricherSettings.Nodes.Shared.DecimalNode.InitialBufferSize);
            Assert.Equal(256, StringEnricherSettings.Nodes.Shared.DecimalNode.MaxBufferSize);
            Assert.Equal(3.0f, StringEnricherSettings.Nodes.Shared.DecimalNode.GrowthFactor);
            Assert.Equal(512, StringEnricherSettings.Nodes.Shared.DecimalNode.MaxStackAllocLength);
            Assert.Equal(32768, StringEnricherSettings.Nodes.Shared.DecimalNode.MaxPooledArrayLength);

            // Cleanup
            StringEnricherSettings.Nodes.Shared.ResetDecimalNodeSettings();
        }

        [Fact]
        public void ResetDecimalNodeSettings_RestoresDefaultValues()
        {
            // Arrange
            StringEnricherSettings.EnableDebugLogs = false;
            StringEnricherSettings.Nodes.Shared.DecimalNode.MaxBufferSize = 500;
            StringEnricherSettings.Nodes.Shared.DecimalNode.InitialBufferSize = 100;
            StringEnricherSettings.Nodes.Shared.DecimalNode.GrowthFactor = 5.0f;
            StringEnricherSettings.Nodes.Shared.DecimalNode.MaxPooledArrayLength = 50000;
            StringEnricherSettings.Nodes.Shared.DecimalNode.MaxStackAllocLength = 1024;

            // Act
            StringEnricherSettings.Nodes.Shared.ResetDecimalNodeSettings();

            // Assert
            Assert.Equal(32, StringEnricherSettings.Nodes.Shared.DecimalNode.InitialBufferSize);
            Assert.Equal(128, StringEnricherSettings.Nodes.Shared.DecimalNode.MaxBufferSize);
            Assert.Equal(2, StringEnricherSettings.Nodes.Shared.DecimalNode.GrowthFactor);
            Assert.Equal(128, StringEnricherSettings.Nodes.Shared.DecimalNode.MaxStackAllocLength);
            Assert.Equal(128, StringEnricherSettings.Nodes.Shared.DecimalNode.MaxPooledArrayLength);
        }

        [Fact]
        public void ResetDecimalNodeSettings_CanBeCalledMultipleTimes()
        {
            // Arrange & Act
            StringEnricherSettings.Nodes.Shared.ResetDecimalNodeSettings();
            StringEnricherSettings.Nodes.Shared.ResetDecimalNodeSettings();
            var settings = StringEnricherSettings.Nodes.Shared.DecimalNode;

            // Assert
            Assert.Equal(32, settings.InitialBufferSize);
            Assert.Equal(128, settings.MaxBufferSize);
            Assert.Equal(2, settings.GrowthFactor);
            Assert.Equal(128, settings.MaxStackAllocLength);
            Assert.Equal(128, settings.MaxPooledArrayLength);
        }

        [Fact]
        public void DecimalNode_ModificationPersistsAcrossMultipleAccesses()
        {
            // Arrange
            StringEnricherSettings.EnableDebugLogs = false;
            StringEnricherSettings.Nodes.Shared.ResetDecimalNodeSettings();
            StringEnricherSettings.Nodes.Shared.DecimalNode.InitialBufferSize = 48;

            // Act
            var firstAccess = StringEnricherSettings.Nodes.Shared.DecimalNode.InitialBufferSize;
            var secondAccess = StringEnricherSettings.Nodes.Shared.DecimalNode.InitialBufferSize;

            // Assert
            Assert.Equal(48, firstAccess);
            Assert.Equal(48, secondAccess);

            // Cleanup
            StringEnricherSettings.Nodes.Shared.ResetDecimalNodeSettings();
        }
    }
}
