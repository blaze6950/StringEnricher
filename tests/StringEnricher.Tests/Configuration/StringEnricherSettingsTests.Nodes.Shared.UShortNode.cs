using StringEnricher.Configuration;

namespace StringEnricher.Tests.Configuration;

public partial class StringEnricherSettingsTests
{
    public class UShortNodeTests
    {
        [Fact]
        public void UShortNode_InitializesWithDefaultValues()
        {
            // Arrange & Act
            StringEnricherSettings.Nodes.Shared.ResetUShortNodeSettings();
            var settings = StringEnricherSettings.Nodes.Shared.UShortNode;

            // Assert
            Assert.Equal("StringEnricherSettings.Nodes.Shared.UShortNode", settings.Name);
            Assert.Equal(8, settings.InitialBufferSize);
            Assert.Equal(64, settings.MaxBufferSize);
            Assert.Equal(2, settings.GrowthFactor);
            Assert.Equal(64, settings.MaxStackAllocLength);
            Assert.Equal(64, settings.MaxPooledArrayLength);
        }

        [Fact]
        public void UShortNode_CanBeModified()
        {
            // Arrange
            StringEnricherSettings.EnableDebugLogs = false;
            StringEnricherSettings.Nodes.Shared.ResetUShortNodeSettings();

            // Act
            StringEnricherSettings.Nodes.Shared.UShortNode.InitialBufferSize = 16;
            StringEnricherSettings.Nodes.Shared.UShortNode.MaxBufferSize = 128;
            StringEnricherSettings.Nodes.Shared.UShortNode.GrowthFactor = 3.0f;
            StringEnricherSettings.Nodes.Shared.UShortNode.MaxPooledArrayLength = 16384;
            StringEnricherSettings.Nodes.Shared.UShortNode.MaxStackAllocLength = 256;

            // Assert
            Assert.Equal(16, StringEnricherSettings.Nodes.Shared.UShortNode.InitialBufferSize);
            Assert.Equal(128, StringEnricherSettings.Nodes.Shared.UShortNode.MaxBufferSize);
            Assert.Equal(3.0f, StringEnricherSettings.Nodes.Shared.UShortNode.GrowthFactor);
            Assert.Equal(256, StringEnricherSettings.Nodes.Shared.UShortNode.MaxStackAllocLength);
            Assert.Equal(16384, StringEnricherSettings.Nodes.Shared.UShortNode.MaxPooledArrayLength);

            // Cleanup
            StringEnricherSettings.Nodes.Shared.ResetUShortNodeSettings();
        }

        [Fact]
        public void ResetUShortNodeSettings_RestoresDefaultValues()
        {
            // Arrange
            StringEnricherSettings.EnableDebugLogs = false;
            StringEnricherSettings.Nodes.Shared.UShortNode.MaxBufferSize = 500;
            StringEnricherSettings.Nodes.Shared.UShortNode.InitialBufferSize = 100;
            StringEnricherSettings.Nodes.Shared.UShortNode.GrowthFactor = 5.0f;
            StringEnricherSettings.Nodes.Shared.UShortNode.MaxPooledArrayLength = 50000;
            StringEnricherSettings.Nodes.Shared.UShortNode.MaxStackAllocLength = 1024;

            // Act
            StringEnricherSettings.Nodes.Shared.ResetUShortNodeSettings();

            // Assert
            Assert.Equal(8, StringEnricherSettings.Nodes.Shared.UShortNode.InitialBufferSize);
            Assert.Equal(64, StringEnricherSettings.Nodes.Shared.UShortNode.MaxBufferSize);
            Assert.Equal(2, StringEnricherSettings.Nodes.Shared.UShortNode.GrowthFactor);
            Assert.Equal(64, StringEnricherSettings.Nodes.Shared.UShortNode.MaxStackAllocLength);
            Assert.Equal(64, StringEnricherSettings.Nodes.Shared.UShortNode.MaxPooledArrayLength);
        }

        [Fact]
        public void ResetUShortNodeSettings_CanBeCalledMultipleTimes()
        {
            // Arrange & Act
            StringEnricherSettings.Nodes.Shared.ResetUShortNodeSettings();
            StringEnricherSettings.Nodes.Shared.ResetUShortNodeSettings();
            var settings = StringEnricherSettings.Nodes.Shared.UShortNode;

            // Assert
            Assert.Equal(8, settings.InitialBufferSize);
            Assert.Equal(64, settings.MaxBufferSize);
            Assert.Equal(2, settings.GrowthFactor);
            Assert.Equal(64, settings.MaxStackAllocLength);
            Assert.Equal(64, settings.MaxPooledArrayLength);
        }

        [Fact]
        public void UShortNode_ModificationPersistsAcrossMultipleAccesses()
        {
            // Arrange
            StringEnricherSettings.EnableDebugLogs = false;
            StringEnricherSettings.Nodes.Shared.ResetUShortNodeSettings();
            StringEnricherSettings.Nodes.Shared.UShortNode.InitialBufferSize = 12;

            // Act
            var firstAccess = StringEnricherSettings.Nodes.Shared.UShortNode.InitialBufferSize;
            var secondAccess = StringEnricherSettings.Nodes.Shared.UShortNode.InitialBufferSize;

            // Assert
            Assert.Equal(12, firstAccess);
            Assert.Equal(12, secondAccess);

            // Cleanup
            StringEnricherSettings.Nodes.Shared.ResetUShortNodeSettings();
        }
    }
}
