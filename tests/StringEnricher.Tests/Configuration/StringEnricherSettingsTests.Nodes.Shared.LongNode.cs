using StringEnricher.Configuration;

namespace StringEnricher.Tests.Configuration;

public partial class StringEnricherSettingsTests
{
    public class LongNodeTests
    {
        [Fact]
        public void LongNode_InitializesWithDefaultValues()
        {
            // Arrange & Act
            StringEnricherSettings.Nodes.Shared.ResetLongNodeSettings();
            var settings = StringEnricherSettings.Nodes.Shared.LongNode;

            // Assert
            Assert.Equal("StringEnricherSettings.Nodes.Shared.LongNode", settings.Name);
            Assert.Equal(32, settings.InitialBufferSize);
            Assert.Equal(128, settings.MaxBufferSize);
            Assert.Equal(2, settings.GrowthFactor);
            Assert.Equal(128, settings.MaxStackAllocLength);
            Assert.Equal(128, settings.MaxPooledArrayLength);
        }

        [Fact]
        public void LongNode_CanBeModified()
        {
            // Arrange
            StringEnricherSettings.EnableDebugLogs = false;
            StringEnricherSettings.Nodes.Shared.ResetLongNodeSettings();

            // Act
            StringEnricherSettings.Nodes.Shared.LongNode.InitialBufferSize = 64;
            StringEnricherSettings.Nodes.Shared.LongNode.MaxBufferSize = 256;
            StringEnricherSettings.Nodes.Shared.LongNode.GrowthFactor = 3.0f;
            StringEnricherSettings.Nodes.Shared.LongNode.MaxPooledArrayLength = 16384;
            StringEnricherSettings.Nodes.Shared.LongNode.MaxStackAllocLength = 512;

            // Assert
            Assert.Equal(64, StringEnricherSettings.Nodes.Shared.LongNode.InitialBufferSize);
            Assert.Equal(256, StringEnricherSettings.Nodes.Shared.LongNode.MaxBufferSize);
            Assert.Equal(3.0f, StringEnricherSettings.Nodes.Shared.LongNode.GrowthFactor);
            Assert.Equal(512, StringEnricherSettings.Nodes.Shared.LongNode.MaxStackAllocLength);
            Assert.Equal(16384, StringEnricherSettings.Nodes.Shared.LongNode.MaxPooledArrayLength);

            // Cleanup
            StringEnricherSettings.Nodes.Shared.ResetLongNodeSettings();
        }

        [Fact]
        public void ResetLongNodeSettings_RestoresDefaultValues()
        {
            // Arrange
            StringEnricherSettings.EnableDebugLogs = false;
            StringEnricherSettings.Nodes.Shared.LongNode.MaxBufferSize = 500;
            StringEnricherSettings.Nodes.Shared.LongNode.InitialBufferSize = 100;
            StringEnricherSettings.Nodes.Shared.LongNode.GrowthFactor = 5.0f;
            StringEnricherSettings.Nodes.Shared.LongNode.MaxPooledArrayLength = 50000;
            StringEnricherSettings.Nodes.Shared.LongNode.MaxStackAllocLength = 1024;

            // Act
            StringEnricherSettings.Nodes.Shared.ResetLongNodeSettings();

            // Assert
            Assert.Equal(32, StringEnricherSettings.Nodes.Shared.LongNode.InitialBufferSize);
            Assert.Equal(128, StringEnricherSettings.Nodes.Shared.LongNode.MaxBufferSize);
            Assert.Equal(2, StringEnricherSettings.Nodes.Shared.LongNode.GrowthFactor);
            Assert.Equal(128, StringEnricherSettings.Nodes.Shared.LongNode.MaxStackAllocLength);
            Assert.Equal(128, StringEnricherSettings.Nodes.Shared.LongNode.MaxPooledArrayLength);
        }

        [Fact]
        public void ResetLongNodeSettings_CanBeCalledMultipleTimes()
        {
            // Arrange & Act
            StringEnricherSettings.Nodes.Shared.ResetLongNodeSettings();
            StringEnricherSettings.Nodes.Shared.ResetLongNodeSettings();
            var settings = StringEnricherSettings.Nodes.Shared.LongNode;

            // Assert
            Assert.Equal(32, settings.InitialBufferSize);
            Assert.Equal(128, settings.MaxBufferSize);
            Assert.Equal(2, settings.GrowthFactor);
            Assert.Equal(128, settings.MaxStackAllocLength);
            Assert.Equal(128, settings.MaxPooledArrayLength);
        }

        [Fact]
        public void LongNode_ModificationPersistsAcrossMultipleAccesses()
        {
            // Arrange
            StringEnricherSettings.EnableDebugLogs = false;
            StringEnricherSettings.Nodes.Shared.ResetLongNodeSettings();
            StringEnricherSettings.Nodes.Shared.LongNode.InitialBufferSize = 80;

            // Act
            var firstAccess = StringEnricherSettings.Nodes.Shared.LongNode.InitialBufferSize;
            var secondAccess = StringEnricherSettings.Nodes.Shared.LongNode.InitialBufferSize;

            // Assert
            Assert.Equal(80, firstAccess);
            Assert.Equal(80, secondAccess);

            // Cleanup
            StringEnricherSettings.Nodes.Shared.ResetLongNodeSettings();
        }
    }
}
