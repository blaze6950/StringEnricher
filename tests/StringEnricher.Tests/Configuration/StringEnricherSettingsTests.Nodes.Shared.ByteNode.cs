using StringEnricher.Configuration;

namespace StringEnricher.Tests.Configuration;

public partial class StringEnricherSettingsTests
{
    public class ByteNodeTests
    {
        [Fact]
        public void ByteNode_InitializesWithDefaultValues()
        {
            // Arrange & Act
            StringEnricherSettings.Nodes.Shared.ResetByteNodeSettings();
            var settings = StringEnricherSettings.Nodes.Shared.ByteNode;

            // Assert
            Assert.Equal("StringEnricherSettings.Nodes.Shared.ByteNode", settings.Name);
            Assert.Equal(4, settings.InitialBufferSize);
            Assert.Equal(64, settings.MaxBufferSize);
            Assert.Equal(2, settings.GrowthFactor);
            Assert.Equal(64, settings.MaxStackAllocLength);
            Assert.Equal(64, settings.MaxPooledArrayLength);
        }

        [Fact]
        public void ByteNode_CanBeModified()
        {
            // Arrange
            StringEnricherSettings.EnableDebugLogs = false;
            StringEnricherSettings.Nodes.Shared.ResetByteNodeSettings();

            // Act
            StringEnricherSettings.Nodes.Shared.ByteNode.InitialBufferSize = 8;
            StringEnricherSettings.Nodes.Shared.ByteNode.MaxBufferSize = 128;
            StringEnricherSettings.Nodes.Shared.ByteNode.GrowthFactor = 3.0f;
            StringEnricherSettings.Nodes.Shared.ByteNode.MaxPooledArrayLength = 16384;
            StringEnricherSettings.Nodes.Shared.ByteNode.MaxStackAllocLength = 512;

            // Assert
            Assert.Equal(8, StringEnricherSettings.Nodes.Shared.ByteNode.InitialBufferSize);
            Assert.Equal(128, StringEnricherSettings.Nodes.Shared.ByteNode.MaxBufferSize);
            Assert.Equal(3.0f, StringEnricherSettings.Nodes.Shared.ByteNode.GrowthFactor);
            Assert.Equal(512, StringEnricherSettings.Nodes.Shared.ByteNode.MaxStackAllocLength);
            Assert.Equal(16384, StringEnricherSettings.Nodes.Shared.ByteNode.MaxPooledArrayLength);

            // Cleanup
            StringEnricherSettings.Nodes.Shared.ResetByteNodeSettings();
        }

        [Fact]
        public void ResetByteNodeSettings_RestoresDefaultValues()
        {
            // Arrange
            StringEnricherSettings.EnableDebugLogs = false;
            StringEnricherSettings.Nodes.Shared.ByteNode.MaxBufferSize = 500;
            StringEnricherSettings.Nodes.Shared.ByteNode.InitialBufferSize = 100;
            StringEnricherSettings.Nodes.Shared.ByteNode.GrowthFactor = 5.0f;
            StringEnricherSettings.Nodes.Shared.ByteNode.MaxPooledArrayLength = 50000;
            StringEnricherSettings.Nodes.Shared.ByteNode.MaxStackAllocLength = 1024;

            // Act
            StringEnricherSettings.Nodes.Shared.ResetByteNodeSettings();

            // Assert
            Assert.Equal(4, StringEnricherSettings.Nodes.Shared.ByteNode.InitialBufferSize);
            Assert.Equal(64, StringEnricherSettings.Nodes.Shared.ByteNode.MaxBufferSize);
            Assert.Equal(2, StringEnricherSettings.Nodes.Shared.ByteNode.GrowthFactor);
            Assert.Equal(64, StringEnricherSettings.Nodes.Shared.ByteNode.MaxStackAllocLength);
            Assert.Equal(64, StringEnricherSettings.Nodes.Shared.ByteNode.MaxPooledArrayLength);
        }

        [Fact]
        public void ResetByteNodeSettings_CanBeCalledMultipleTimes()
        {
            // Arrange & Act
            StringEnricherSettings.Nodes.Shared.ResetByteNodeSettings();
            StringEnricherSettings.Nodes.Shared.ResetByteNodeSettings();
            var settings = StringEnricherSettings.Nodes.Shared.ByteNode;

            // Assert
            Assert.Equal(4, settings.InitialBufferSize);
            Assert.Equal(64, settings.MaxBufferSize);
            Assert.Equal(2, settings.GrowthFactor);
            Assert.Equal(64, settings.MaxStackAllocLength);
            Assert.Equal(64, settings.MaxPooledArrayLength);
        }

        [Fact]
        public void ByteNode_ModificationPersistsAcrossMultipleAccesses()
        {
            // Arrange
            StringEnricherSettings.EnableDebugLogs = false;
            StringEnricherSettings.Nodes.Shared.ResetByteNodeSettings();
            StringEnricherSettings.Nodes.Shared.ByteNode.InitialBufferSize = 16;

            // Act
            var firstAccess = StringEnricherSettings.Nodes.Shared.ByteNode.InitialBufferSize;
            var secondAccess = StringEnricherSettings.Nodes.Shared.ByteNode.InitialBufferSize;

            // Assert
            Assert.Equal(16, firstAccess);
            Assert.Equal(16, secondAccess);

            // Cleanup
            StringEnricherSettings.Nodes.Shared.ResetByteNodeSettings();
        }
    }
}