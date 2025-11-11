using StringEnricher.Configuration;

namespace StringEnricher.Tests.Configuration;

public partial class StringEnricherSettingsTests
{
    public class ShortNodeTests
    {
        [Fact]
        public void ShortNode_InitializesWithDefaultValues()
        {
            // Arrange & Act
            StringEnricherSettings.Nodes.Shared.ResetShortNodeSettings();
            var settings = StringEnricherSettings.Nodes.Shared.ShortNode;

            // Assert
            Assert.Equal("StringEnricherSettings.Nodes.Shared.ShortNode", settings.Name);
            Assert.Equal(8, settings.InitialBufferSize);
            Assert.Equal(64, settings.MaxBufferSize);
            Assert.Equal(2, settings.GrowthFactor);
            Assert.Equal(64, settings.MaxStackAllocLength);
            Assert.Equal(64, settings.MaxPooledArrayLength);
        }

        [Fact]
        public void ShortNode_CanBeModified()
        {
            // Arrange
            StringEnricherSettings.EnableDebugLogs = false;
            StringEnricherSettings.Nodes.Shared.ResetShortNodeSettings();

            // Act
            StringEnricherSettings.Nodes.Shared.ShortNode.InitialBufferSize = 16;
            StringEnricherSettings.Nodes.Shared.ShortNode.MaxBufferSize = 128;
            StringEnricherSettings.Nodes.Shared.ShortNode.GrowthFactor = 3.0f;
            StringEnricherSettings.Nodes.Shared.ShortNode.MaxPooledArrayLength = 16384;
            StringEnricherSettings.Nodes.Shared.ShortNode.MaxStackAllocLength = 256;

            // Assert
            Assert.Equal(16, StringEnricherSettings.Nodes.Shared.ShortNode.InitialBufferSize);
            Assert.Equal(128, StringEnricherSettings.Nodes.Shared.ShortNode.MaxBufferSize);
            Assert.Equal(3.0f, StringEnricherSettings.Nodes.Shared.ShortNode.GrowthFactor);
            Assert.Equal(256, StringEnricherSettings.Nodes.Shared.ShortNode.MaxStackAllocLength);
            Assert.Equal(16384, StringEnricherSettings.Nodes.Shared.ShortNode.MaxPooledArrayLength);

            // Cleanup
            StringEnricherSettings.Nodes.Shared.ResetShortNodeSettings();
        }

        [Fact]
        public void ResetShortNodeSettings_RestoresDefaultValues()
        {
            // Arrange
            StringEnricherSettings.EnableDebugLogs = false;
            StringEnricherSettings.Nodes.Shared.ShortNode.MaxBufferSize = 500;
            StringEnricherSettings.Nodes.Shared.ShortNode.InitialBufferSize = 100;
            StringEnricherSettings.Nodes.Shared.ShortNode.GrowthFactor = 5.0f;
            StringEnricherSettings.Nodes.Shared.ShortNode.MaxPooledArrayLength = 50000;
            StringEnricherSettings.Nodes.Shared.ShortNode.MaxStackAllocLength = 1024;

            // Act
            StringEnricherSettings.Nodes.Shared.ResetShortNodeSettings();

            // Assert
            Assert.Equal(8, StringEnricherSettings.Nodes.Shared.ShortNode.InitialBufferSize);
            Assert.Equal(64, StringEnricherSettings.Nodes.Shared.ShortNode.MaxBufferSize);
            Assert.Equal(2, StringEnricherSettings.Nodes.Shared.ShortNode.GrowthFactor);
            Assert.Equal(64, StringEnricherSettings.Nodes.Shared.ShortNode.MaxStackAllocLength);
            Assert.Equal(64, StringEnricherSettings.Nodes.Shared.ShortNode.MaxPooledArrayLength);
        }

        [Fact]
        public void ResetShortNodeSettings_CanBeCalledMultipleTimes()
        {
            // Arrange & Act
            StringEnricherSettings.Nodes.Shared.ResetShortNodeSettings();
            StringEnricherSettings.Nodes.Shared.ResetShortNodeSettings();
            var settings = StringEnricherSettings.Nodes.Shared.ShortNode;

            // Assert
            Assert.Equal(8, settings.InitialBufferSize);
            Assert.Equal(64, settings.MaxBufferSize);
            Assert.Equal(2, settings.GrowthFactor);
            Assert.Equal(64, settings.MaxStackAllocLength);
            Assert.Equal(64, settings.MaxPooledArrayLength);
        }

        [Fact]
        public void ShortNode_ModificationPersistsAcrossMultipleAccesses()
        {
            // Arrange
            StringEnricherSettings.EnableDebugLogs = false;
            StringEnricherSettings.Nodes.Shared.ResetShortNodeSettings();
            StringEnricherSettings.Nodes.Shared.ShortNode.InitialBufferSize = 12;

            // Act
            var firstAccess = StringEnricherSettings.Nodes.Shared.ShortNode.InitialBufferSize;
            var secondAccess = StringEnricherSettings.Nodes.Shared.ShortNode.InitialBufferSize;

            // Assert
            Assert.Equal(12, firstAccess);
            Assert.Equal(12, secondAccess);

            // Cleanup
            StringEnricherSettings.Nodes.Shared.ResetShortNodeSettings();
        }
    }
}
