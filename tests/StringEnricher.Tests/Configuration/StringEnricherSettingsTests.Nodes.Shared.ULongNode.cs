using StringEnricher.Configuration;

namespace StringEnricher.Tests.Configuration;

public partial class StringEnricherSettingsTests
{
    public class ULongNodeTests
    {
        [Fact]
        public void ULongNode_InitializesWithDefaultValues()
        {
            // Arrange & Act
            StringEnricherSettings.Nodes.Shared.ResetULongNodeSettings();
            var settings = StringEnricherSettings.Nodes.Shared.ULongNode;

            // Assert
            Assert.Equal("StringEnricherSettings.Nodes.Shared.ULongNode", settings.Name);
            Assert.Equal(32, settings.InitialBufferSize);
            Assert.Equal(128, settings.MaxBufferSize);
            Assert.Equal(2, settings.GrowthFactor);
            Assert.Equal(128, settings.MaxStackAllocLength);
            Assert.Equal(128, settings.MaxPooledArrayLength);
        }

        [Fact]
        public void ULongNode_CanBeModified()
        {
            // Arrange
            StringEnricherSettings.EnableDebugLogs = false;
            StringEnricherSettings.Nodes.Shared.ResetULongNodeSettings();

            // Act
            StringEnricherSettings.Nodes.Shared.ULongNode.InitialBufferSize = 64;
            StringEnricherSettings.Nodes.Shared.ULongNode.MaxBufferSize = 256;
            StringEnricherSettings.Nodes.Shared.ULongNode.GrowthFactor = 3.0f;
            StringEnricherSettings.Nodes.Shared.ULongNode.MaxPooledArrayLength = 16384;
            StringEnricherSettings.Nodes.Shared.ULongNode.MaxStackAllocLength = 512;

            // Assert
            Assert.Equal(64, StringEnricherSettings.Nodes.Shared.ULongNode.InitialBufferSize);
            Assert.Equal(256, StringEnricherSettings.Nodes.Shared.ULongNode.MaxBufferSize);
            Assert.Equal(3.0f, StringEnricherSettings.Nodes.Shared.ULongNode.GrowthFactor);
            Assert.Equal(512, StringEnricherSettings.Nodes.Shared.ULongNode.MaxStackAllocLength);
            Assert.Equal(16384, StringEnricherSettings.Nodes.Shared.ULongNode.MaxPooledArrayLength);

            // Cleanup
            StringEnricherSettings.Nodes.Shared.ResetULongNodeSettings();
        }

        [Fact]
        public void ResetULongNodeSettings_RestoresDefaultValues()
        {
            // Arrange
            StringEnricherSettings.EnableDebugLogs = false;
            StringEnricherSettings.Nodes.Shared.ULongNode.MaxBufferSize = 500;
            StringEnricherSettings.Nodes.Shared.ULongNode.InitialBufferSize = 100;
            StringEnricherSettings.Nodes.Shared.ULongNode.GrowthFactor = 5.0f;
            StringEnricherSettings.Nodes.Shared.ULongNode.MaxPooledArrayLength = 50000;
            StringEnricherSettings.Nodes.Shared.ULongNode.MaxStackAllocLength = 1024;

            // Act
            StringEnricherSettings.Nodes.Shared.ResetULongNodeSettings();

            // Assert
            Assert.Equal(32, StringEnricherSettings.Nodes.Shared.ULongNode.InitialBufferSize);
            Assert.Equal(128, StringEnricherSettings.Nodes.Shared.ULongNode.MaxBufferSize);
            Assert.Equal(2, StringEnricherSettings.Nodes.Shared.ULongNode.GrowthFactor);
            Assert.Equal(128, StringEnricherSettings.Nodes.Shared.ULongNode.MaxStackAllocLength);
            Assert.Equal(128, StringEnricherSettings.Nodes.Shared.ULongNode.MaxPooledArrayLength);
        }

        [Fact]
        public void ResetULongNodeSettings_CanBeCalledMultipleTimes()
        {
            // Arrange & Act
            StringEnricherSettings.Nodes.Shared.ResetULongNodeSettings();
            StringEnricherSettings.Nodes.Shared.ResetULongNodeSettings();
            var settings = StringEnricherSettings.Nodes.Shared.ULongNode;

            // Assert
            Assert.Equal(32, settings.InitialBufferSize);
            Assert.Equal(128, settings.MaxBufferSize);
            Assert.Equal(2, settings.GrowthFactor);
            Assert.Equal(128, settings.MaxStackAllocLength);
            Assert.Equal(128, settings.MaxPooledArrayLength);
        }

        [Fact]
        public void ULongNode_ModificationPersistsAcrossMultipleAccesses()
        {
            // Arrange
            StringEnricherSettings.EnableDebugLogs = false;
            StringEnricherSettings.Nodes.Shared.ResetULongNodeSettings();
            StringEnricherSettings.Nodes.Shared.ULongNode.InitialBufferSize = 80;

            // Act
            var firstAccess = StringEnricherSettings.Nodes.Shared.ULongNode.InitialBufferSize;
            var secondAccess = StringEnricherSettings.Nodes.Shared.ULongNode.InitialBufferSize;

            // Assert
            Assert.Equal(80, firstAccess);
            Assert.Equal(80, secondAccess);

            // Cleanup
            StringEnricherSettings.Nodes.Shared.ResetULongNodeSettings();
        }
    }
}
