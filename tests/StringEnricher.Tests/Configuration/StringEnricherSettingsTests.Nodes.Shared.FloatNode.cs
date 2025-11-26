using StringEnricher.Configuration;

namespace StringEnricher.Tests.Configuration;

public partial class StringEnricherSettingsTests
{
    public class FloatNodeTests
    {
        [Fact]
        public void FloatNode_InitializesWithDefaultValues()
        {
            // Arrange & Act
            StringEnricherSettings.Nodes.Shared.ResetFloatNodeSettings();
            var settings = StringEnricherSettings.Nodes.Shared.FloatNode;

            // Assert
            Assert.Equal("StringEnricherSettings.Nodes.Shared.FloatNode", settings.Name);
            Assert.Equal(32, settings.InitialBufferSize);
            Assert.Equal(128, settings.MaxBufferSize);
            Assert.Equal(2, settings.GrowthFactor);
            Assert.Equal(128, settings.MaxStackAllocLength);
            Assert.Equal(128, settings.MaxPooledArrayLength);
        }

        [Fact]
        public void FloatNode_CanBeModified()
        {
            // Arrange
            StringEnricherSettings.EnableDebugLogs = false;
            StringEnricherSettings.Nodes.Shared.ResetFloatNodeSettings();

            // Act
            StringEnricherSettings.Nodes.Shared.FloatNode.InitialBufferSize = 64;
            StringEnricherSettings.Nodes.Shared.FloatNode.MaxBufferSize = 256;
            StringEnricherSettings.Nodes.Shared.FloatNode.GrowthFactor = 3.0f;
            StringEnricherSettings.Nodes.Shared.FloatNode.MaxPooledArrayLength = 32768;
            StringEnricherSettings.Nodes.Shared.FloatNode.MaxStackAllocLength = 512;

            // Assert
            Assert.Equal(64, StringEnricherSettings.Nodes.Shared.FloatNode.InitialBufferSize);
            Assert.Equal(256, StringEnricherSettings.Nodes.Shared.FloatNode.MaxBufferSize);
            Assert.Equal(3.0f, StringEnricherSettings.Nodes.Shared.FloatNode.GrowthFactor);
            Assert.Equal(512, StringEnricherSettings.Nodes.Shared.FloatNode.MaxStackAllocLength);
            Assert.Equal(32768, StringEnricherSettings.Nodes.Shared.FloatNode.MaxPooledArrayLength);

            // Cleanup
            StringEnricherSettings.Nodes.Shared.ResetFloatNodeSettings();
        }

        [Fact]
        public void ResetFloatNodeSettings_RestoresDefaultValues()
        {
            // Arrange
            StringEnricherSettings.EnableDebugLogs = false;
            StringEnricherSettings.Nodes.Shared.FloatNode.MaxBufferSize = 500;
            StringEnricherSettings.Nodes.Shared.FloatNode.InitialBufferSize = 100;
            StringEnricherSettings.Nodes.Shared.FloatNode.GrowthFactor = 5.0f;
            StringEnricherSettings.Nodes.Shared.FloatNode.MaxPooledArrayLength = 50000;
            StringEnricherSettings.Nodes.Shared.FloatNode.MaxStackAllocLength = 1024;

            // Act
            StringEnricherSettings.Nodes.Shared.ResetFloatNodeSettings();

            // Assert
            Assert.Equal(32, StringEnricherSettings.Nodes.Shared.FloatNode.InitialBufferSize);
            Assert.Equal(128, StringEnricherSettings.Nodes.Shared.FloatNode.MaxBufferSize);
            Assert.Equal(2, StringEnricherSettings.Nodes.Shared.FloatNode.GrowthFactor);
            Assert.Equal(128, StringEnricherSettings.Nodes.Shared.FloatNode.MaxStackAllocLength);
            Assert.Equal(128, StringEnricherSettings.Nodes.Shared.FloatNode.MaxPooledArrayLength);
        }

        [Fact]
        public void ResetFloatNodeSettings_CanBeCalledMultipleTimes()
        {
            // Arrange & Act
            StringEnricherSettings.Nodes.Shared.ResetFloatNodeSettings();
            StringEnricherSettings.Nodes.Shared.ResetFloatNodeSettings();
            var settings = StringEnricherSettings.Nodes.Shared.FloatNode;

            // Assert
            Assert.Equal(32, settings.InitialBufferSize);
            Assert.Equal(128, settings.MaxBufferSize);
            Assert.Equal(2, settings.GrowthFactor);
            Assert.Equal(128, settings.MaxStackAllocLength);
            Assert.Equal(128, settings.MaxPooledArrayLength);
        }

        [Fact]
        public void FloatNode_ModificationPersistsAcrossMultipleAccesses()
        {
            // Arrange
            StringEnricherSettings.EnableDebugLogs = false;
            StringEnricherSettings.Nodes.Shared.ResetFloatNodeSettings();
            StringEnricherSettings.Nodes.Shared.FloatNode.InitialBufferSize = 48;

            // Act
            var firstAccess = StringEnricherSettings.Nodes.Shared.FloatNode.InitialBufferSize;
            var secondAccess = StringEnricherSettings.Nodes.Shared.FloatNode.InitialBufferSize;

            // Assert
            Assert.Equal(48, firstAccess);
            Assert.Equal(48, secondAccess);

            // Cleanup
            StringEnricherSettings.Nodes.Shared.ResetFloatNodeSettings();
        }
    }
}
