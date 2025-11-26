using StringEnricher.Configuration;

namespace StringEnricher.Tests.Configuration;

public partial class StringEnricherSettingsTests
{
    public class DoubleNodeTests
    {
        [Fact]
        public void DoubleNode_InitializesWithDefaultValues()
        {
            // Arrange & Act
            StringEnricherSettings.Nodes.Shared.ResetDoubleNodeSettings();
            var settings = StringEnricherSettings.Nodes.Shared.DoubleNode;

            // Assert
            Assert.Equal("StringEnricherSettings.Nodes.Shared.DoubleNode", settings.Name);
            Assert.Equal(32, settings.InitialBufferSize);
            Assert.Equal(128, settings.MaxBufferSize);
            Assert.Equal(2, settings.GrowthFactor);
            Assert.Equal(128, settings.MaxStackAllocLength);
            Assert.Equal(128, settings.MaxPooledArrayLength);
        }

        [Fact]
        public void DoubleNode_CanBeModified()
        {
            // Arrange
            StringEnricherSettings.EnableDebugLogs = false;
            StringEnricherSettings.Nodes.Shared.ResetDoubleNodeSettings();

            // Act
            StringEnricherSettings.Nodes.Shared.DoubleNode.InitialBufferSize = 64;
            StringEnricherSettings.Nodes.Shared.DoubleNode.MaxBufferSize = 256;
            StringEnricherSettings.Nodes.Shared.DoubleNode.GrowthFactor = 3.0f;
            StringEnricherSettings.Nodes.Shared.DoubleNode.MaxPooledArrayLength = 32768;
            StringEnricherSettings.Nodes.Shared.DoubleNode.MaxStackAllocLength = 512;

            // Assert
            Assert.Equal(64, StringEnricherSettings.Nodes.Shared.DoubleNode.InitialBufferSize);
            Assert.Equal(256, StringEnricherSettings.Nodes.Shared.DoubleNode.MaxBufferSize);
            Assert.Equal(3.0f, StringEnricherSettings.Nodes.Shared.DoubleNode.GrowthFactor);
            Assert.Equal(512, StringEnricherSettings.Nodes.Shared.DoubleNode.MaxStackAllocLength);
            Assert.Equal(32768, StringEnricherSettings.Nodes.Shared.DoubleNode.MaxPooledArrayLength);

            // Cleanup
            StringEnricherSettings.Nodes.Shared.ResetDoubleNodeSettings();
        }

        [Fact]
        public void ResetDoubleNodeSettings_RestoresDefaultValues()
        {
            // Arrange
            StringEnricherSettings.EnableDebugLogs = false;
            StringEnricherSettings.Nodes.Shared.DoubleNode.MaxBufferSize = 500;
            StringEnricherSettings.Nodes.Shared.DoubleNode.InitialBufferSize = 100;
            StringEnricherSettings.Nodes.Shared.DoubleNode.GrowthFactor = 5.0f;
            StringEnricherSettings.Nodes.Shared.DoubleNode.MaxPooledArrayLength = 50000;
            StringEnricherSettings.Nodes.Shared.DoubleNode.MaxStackAllocLength = 1024;

            // Act
            StringEnricherSettings.Nodes.Shared.ResetDoubleNodeSettings();

            // Assert
            Assert.Equal(32, StringEnricherSettings.Nodes.Shared.DoubleNode.InitialBufferSize);
            Assert.Equal(128, StringEnricherSettings.Nodes.Shared.DoubleNode.MaxBufferSize);
            Assert.Equal(2, StringEnricherSettings.Nodes.Shared.DoubleNode.GrowthFactor);
            Assert.Equal(128, StringEnricherSettings.Nodes.Shared.DoubleNode.MaxStackAllocLength);
            Assert.Equal(128, StringEnricherSettings.Nodes.Shared.DoubleNode.MaxPooledArrayLength);
        }

        [Fact]
        public void ResetDoubleNodeSettings_CanBeCalledMultipleTimes()
        {
            // Arrange & Act
            StringEnricherSettings.Nodes.Shared.ResetDoubleNodeSettings();
            StringEnricherSettings.Nodes.Shared.ResetDoubleNodeSettings();
            var settings = StringEnricherSettings.Nodes.Shared.DoubleNode;

            // Assert
            Assert.Equal(32, settings.InitialBufferSize);
            Assert.Equal(128, settings.MaxBufferSize);
            Assert.Equal(2, settings.GrowthFactor);
            Assert.Equal(128, settings.MaxStackAllocLength);
            Assert.Equal(128, settings.MaxPooledArrayLength);
        }

        [Fact]
        public void DoubleNode_ModificationPersistsAcrossMultipleAccesses()
        {
            // Arrange
            StringEnricherSettings.EnableDebugLogs = false;
            StringEnricherSettings.Nodes.Shared.ResetDoubleNodeSettings();
            StringEnricherSettings.Nodes.Shared.DoubleNode.InitialBufferSize = 48;

            // Act
            var firstAccess = StringEnricherSettings.Nodes.Shared.DoubleNode.InitialBufferSize;
            var secondAccess = StringEnricherSettings.Nodes.Shared.DoubleNode.InitialBufferSize;

            // Assert
            Assert.Equal(48, firstAccess);
            Assert.Equal(48, secondAccess);

            // Cleanup
            StringEnricherSettings.Nodes.Shared.ResetDoubleNodeSettings();
        }
    }
}
