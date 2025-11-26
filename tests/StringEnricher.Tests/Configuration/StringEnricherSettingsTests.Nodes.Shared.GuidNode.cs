using StringEnricher.Configuration;

namespace StringEnricher.Tests.Configuration;

public partial class StringEnricherSettingsTests
{
    public class GuidNodeTests
    {
        [Fact]
        public void GuidNode_InitializesWithDefaultValues()
        {
            // Arrange & Act
            StringEnricherSettings.Nodes.Shared.ResetGuidNodeSettings();
            var settings = StringEnricherSettings.Nodes.Shared.GuidNode;

            // Assert
            Assert.Equal("StringEnricherSettings.Nodes.Shared.GuidNode", settings.Name);
            Assert.Equal(64, settings.InitialBufferSize);
            Assert.Equal(128, settings.MaxBufferSize);
            Assert.Equal(2, settings.GrowthFactor);
            Assert.Equal(128, settings.MaxStackAllocLength);
            Assert.Equal(128, settings.MaxPooledArrayLength);
        }

        [Fact]
        public void GuidNode_CanBeModified()
        {
            // Arrange
            StringEnricherSettings.EnableDebugLogs = false;
            StringEnricherSettings.Nodes.Shared.ResetGuidNodeSettings();

            // Act
            StringEnricherSettings.Nodes.Shared.GuidNode.InitialBufferSize = 128;
            StringEnricherSettings.Nodes.Shared.GuidNode.MaxBufferSize = 256;
            StringEnricherSettings.Nodes.Shared.GuidNode.GrowthFactor = 3.0f;
            StringEnricherSettings.Nodes.Shared.GuidNode.MaxPooledArrayLength = 32768;
            StringEnricherSettings.Nodes.Shared.GuidNode.MaxStackAllocLength = 512;

            // Assert
            Assert.Equal(128, StringEnricherSettings.Nodes.Shared.GuidNode.InitialBufferSize);
            Assert.Equal(256, StringEnricherSettings.Nodes.Shared.GuidNode.MaxBufferSize);
            Assert.Equal(3.0f, StringEnricherSettings.Nodes.Shared.GuidNode.GrowthFactor);
            Assert.Equal(512, StringEnricherSettings.Nodes.Shared.GuidNode.MaxStackAllocLength);
            Assert.Equal(32768, StringEnricherSettings.Nodes.Shared.GuidNode.MaxPooledArrayLength);

            // Cleanup
            StringEnricherSettings.Nodes.Shared.ResetGuidNodeSettings();
        }

        [Fact]
        public void ResetGuidNodeSettings_RestoresDefaultValues()
        {
            // Arrange
            StringEnricherSettings.EnableDebugLogs = false;
            StringEnricherSettings.Nodes.Shared.GuidNode.MaxBufferSize = 500;
            StringEnricherSettings.Nodes.Shared.GuidNode.InitialBufferSize = 100;
            StringEnricherSettings.Nodes.Shared.GuidNode.GrowthFactor = 5.0f;
            StringEnricherSettings.Nodes.Shared.GuidNode.MaxPooledArrayLength = 50000;
            StringEnricherSettings.Nodes.Shared.GuidNode.MaxStackAllocLength = 1024;

            // Act
            StringEnricherSettings.Nodes.Shared.ResetGuidNodeSettings();

            // Assert
            Assert.Equal(64, StringEnricherSettings.Nodes.Shared.GuidNode.InitialBufferSize);
            Assert.Equal(128, StringEnricherSettings.Nodes.Shared.GuidNode.MaxBufferSize);
            Assert.Equal(2, StringEnricherSettings.Nodes.Shared.GuidNode.GrowthFactor);
            Assert.Equal(128, StringEnricherSettings.Nodes.Shared.GuidNode.MaxStackAllocLength);
            Assert.Equal(128, StringEnricherSettings.Nodes.Shared.GuidNode.MaxPooledArrayLength);
        }

        [Fact]
        public void ResetGuidNodeSettings_CanBeCalledMultipleTimes()
        {
            // Arrange & Act
            StringEnricherSettings.Nodes.Shared.ResetGuidNodeSettings();
            StringEnricherSettings.Nodes.Shared.ResetGuidNodeSettings();
            var settings = StringEnricherSettings.Nodes.Shared.GuidNode;

            // Assert
            Assert.Equal(64, settings.InitialBufferSize);
            Assert.Equal(128, settings.MaxBufferSize);
            Assert.Equal(2, settings.GrowthFactor);
            Assert.Equal(128, settings.MaxStackAllocLength);
            Assert.Equal(128, settings.MaxPooledArrayLength);
        }

        [Fact]
        public void GuidNode_ModificationPersistsAcrossMultipleAccesses()
        {
            // Arrange
            StringEnricherSettings.EnableDebugLogs = false;
            StringEnricherSettings.Nodes.Shared.ResetGuidNodeSettings();
            StringEnricherSettings.Nodes.Shared.GuidNode.InitialBufferSize = 96;

            // Act
            var firstAccess = StringEnricherSettings.Nodes.Shared.GuidNode.InitialBufferSize;
            var secondAccess = StringEnricherSettings.Nodes.Shared.GuidNode.InitialBufferSize;

            // Assert
            Assert.Equal(96, firstAccess);
            Assert.Equal(96, secondAccess);

            // Cleanup
            StringEnricherSettings.Nodes.Shared.ResetGuidNodeSettings();
        }
    }
}
