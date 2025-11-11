using StringEnricher.Configuration;

namespace StringEnricher.Tests.Configuration;

public partial class StringEnricherSettingsTests
{
    public class SByteNodeTests
    {
        [Fact]
        public void SByteNode_InitializesWithDefaultValues()
        {
            // Arrange & Act
            StringEnricherSettings.Nodes.Shared.ResetSByteNodeSettings();
            var settings = StringEnricherSettings.Nodes.Shared.SByteNode;

            // Assert
            Assert.Equal("StringEnricherSettings.Nodes.Shared.SByteNode", settings.Name);
            Assert.Equal(4, settings.InitialBufferSize);
            Assert.Equal(32, settings.MaxBufferSize);
            Assert.Equal(2, settings.GrowthFactor);
            Assert.Equal(32, settings.MaxStackAllocLength);
            Assert.Equal(32, settings.MaxPooledArrayLength);
        }

        [Fact]
        public void SByteNode_CanBeModified()
        {
            // Arrange
            StringEnricherSettings.EnableDebugLogs = false;
            StringEnricherSettings.Nodes.Shared.ResetSByteNodeSettings();

            // Act
            StringEnricherSettings.Nodes.Shared.SByteNode.InitialBufferSize = 8;
            StringEnricherSettings.Nodes.Shared.SByteNode.MaxBufferSize = 64;
            StringEnricherSettings.Nodes.Shared.SByteNode.GrowthFactor = 3.0f;
            StringEnricherSettings.Nodes.Shared.SByteNode.MaxPooledArrayLength = 16384;
            StringEnricherSettings.Nodes.Shared.SByteNode.MaxStackAllocLength = 128;

            // Assert
            Assert.Equal(8, StringEnricherSettings.Nodes.Shared.SByteNode.InitialBufferSize);
            Assert.Equal(64, StringEnricherSettings.Nodes.Shared.SByteNode.MaxBufferSize);
            Assert.Equal(3.0f, StringEnricherSettings.Nodes.Shared.SByteNode.GrowthFactor);
            Assert.Equal(128, StringEnricherSettings.Nodes.Shared.SByteNode.MaxStackAllocLength);
            Assert.Equal(16384, StringEnricherSettings.Nodes.Shared.SByteNode.MaxPooledArrayLength);

            // Cleanup
            StringEnricherSettings.Nodes.Shared.ResetSByteNodeSettings();
        }

        [Fact]
        public void ResetSByteNodeSettings_RestoresDefaultValues()
        {
            // Arrange
            StringEnricherSettings.EnableDebugLogs = false;
            StringEnricherSettings.Nodes.Shared.SByteNode.MaxBufferSize = 500;
            StringEnricherSettings.Nodes.Shared.SByteNode.InitialBufferSize = 100;
            StringEnricherSettings.Nodes.Shared.SByteNode.GrowthFactor = 5.0f;
            StringEnricherSettings.Nodes.Shared.SByteNode.MaxPooledArrayLength = 50000;
            StringEnricherSettings.Nodes.Shared.SByteNode.MaxStackAllocLength = 1024;

            // Act
            StringEnricherSettings.Nodes.Shared.ResetSByteNodeSettings();

            // Assert
            Assert.Equal(4, StringEnricherSettings.Nodes.Shared.SByteNode.InitialBufferSize);
            Assert.Equal(32, StringEnricherSettings.Nodes.Shared.SByteNode.MaxBufferSize);
            Assert.Equal(2, StringEnricherSettings.Nodes.Shared.SByteNode.GrowthFactor);
            Assert.Equal(32, StringEnricherSettings.Nodes.Shared.SByteNode.MaxStackAllocLength);
            Assert.Equal(32, StringEnricherSettings.Nodes.Shared.SByteNode.MaxPooledArrayLength);
        }

        [Fact]
        public void ResetSByteNodeSettings_CanBeCalledMultipleTimes()
        {
            // Arrange & Act
            StringEnricherSettings.Nodes.Shared.ResetSByteNodeSettings();
            StringEnricherSettings.Nodes.Shared.ResetSByteNodeSettings();
            var settings = StringEnricherSettings.Nodes.Shared.SByteNode;

            // Assert
            Assert.Equal(4, settings.InitialBufferSize);
            Assert.Equal(32, settings.MaxBufferSize);
            Assert.Equal(2, settings.GrowthFactor);
            Assert.Equal(32, settings.MaxStackAllocLength);
            Assert.Equal(32, settings.MaxPooledArrayLength);
        }

        [Fact]
        public void SByteNode_ModificationPersistsAcrossMultipleAccesses()
        {
            // Arrange
            StringEnricherSettings.EnableDebugLogs = false;
            StringEnricherSettings.Nodes.Shared.ResetSByteNodeSettings();
            StringEnricherSettings.Nodes.Shared.SByteNode.InitialBufferSize = 6;

            // Act
            var firstAccess = StringEnricherSettings.Nodes.Shared.SByteNode.InitialBufferSize;
            var secondAccess = StringEnricherSettings.Nodes.Shared.SByteNode.InitialBufferSize;

            // Assert
            Assert.Equal(6, firstAccess);
            Assert.Equal(6, secondAccess);

            // Cleanup
            StringEnricherSettings.Nodes.Shared.ResetSByteNodeSettings();
        }
    }
}
