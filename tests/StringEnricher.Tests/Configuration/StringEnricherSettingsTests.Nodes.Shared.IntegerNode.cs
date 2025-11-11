using StringEnricher.Configuration;

namespace StringEnricher.Tests.Configuration;

public partial class StringEnricherSettingsTests
{
    public class IntegerNodeTests
    {
        [Fact]
        public void IntegerNode_InitializesWithDefaultValues()
        {
            // Arrange & Act
            StringEnricherSettings.Nodes.Shared.ResetIntegerNodeSettings();
            var settings = StringEnricherSettings.Nodes.Shared.IntegerNode;

            // Assert
            Assert.Equal("StringEnricherSettings.Nodes.Shared.IntegerNode", settings.Name);
            Assert.Equal(16, settings.InitialBufferSize);
            Assert.Equal(64, settings.MaxBufferSize);
            Assert.Equal(2, settings.GrowthFactor);
            Assert.Equal(64, settings.MaxStackAllocLength);
            Assert.Equal(64, settings.MaxPooledArrayLength);
        }

        [Fact]
        public void IntegerNode_CanBeModified()
        {
            // Arrange
            StringEnricherSettings.EnableDebugLogs = false;
            StringEnricherSettings.Nodes.Shared.ResetIntegerNodeSettings();

            // Act
            StringEnricherSettings.Nodes.Shared.IntegerNode.InitialBufferSize = 32;
            StringEnricherSettings.Nodes.Shared.IntegerNode.MaxBufferSize = 128;
            StringEnricherSettings.Nodes.Shared.IntegerNode.GrowthFactor = 3.0f;
            StringEnricherSettings.Nodes.Shared.IntegerNode.MaxPooledArrayLength = 16384;
            StringEnricherSettings.Nodes.Shared.IntegerNode.MaxStackAllocLength = 256;

            // Assert
            Assert.Equal(32, StringEnricherSettings.Nodes.Shared.IntegerNode.InitialBufferSize);
            Assert.Equal(128, StringEnricherSettings.Nodes.Shared.IntegerNode.MaxBufferSize);
            Assert.Equal(3.0f, StringEnricherSettings.Nodes.Shared.IntegerNode.GrowthFactor);
            Assert.Equal(256, StringEnricherSettings.Nodes.Shared.IntegerNode.MaxStackAllocLength);
            Assert.Equal(16384, StringEnricherSettings.Nodes.Shared.IntegerNode.MaxPooledArrayLength);

            // Cleanup
            StringEnricherSettings.Nodes.Shared.ResetIntegerNodeSettings();
        }

        [Fact]
        public void ResetIntegerNodeSettings_RestoresDefaultValues()
        {
            // Arrange
            StringEnricherSettings.EnableDebugLogs = false;
            StringEnricherSettings.Nodes.Shared.IntegerNode.MaxBufferSize = 500;
            StringEnricherSettings.Nodes.Shared.IntegerNode.InitialBufferSize = 100;
            StringEnricherSettings.Nodes.Shared.IntegerNode.GrowthFactor = 5.0f;
            StringEnricherSettings.Nodes.Shared.IntegerNode.MaxPooledArrayLength = 50000;
            StringEnricherSettings.Nodes.Shared.IntegerNode.MaxStackAllocLength = 1024;

            // Act
            StringEnricherSettings.Nodes.Shared.ResetIntegerNodeSettings();

            // Assert
            Assert.Equal(16, StringEnricherSettings.Nodes.Shared.IntegerNode.InitialBufferSize);
            Assert.Equal(64, StringEnricherSettings.Nodes.Shared.IntegerNode.MaxBufferSize);
            Assert.Equal(2, StringEnricherSettings.Nodes.Shared.IntegerNode.GrowthFactor);
            Assert.Equal(64, StringEnricherSettings.Nodes.Shared.IntegerNode.MaxStackAllocLength);
            Assert.Equal(64, StringEnricherSettings.Nodes.Shared.IntegerNode.MaxPooledArrayLength);
        }

        [Fact]
        public void ResetIntegerNodeSettings_CanBeCalledMultipleTimes()
        {
            // Arrange & Act
            StringEnricherSettings.Nodes.Shared.ResetIntegerNodeSettings();
            StringEnricherSettings.Nodes.Shared.ResetIntegerNodeSettings();
            var settings = StringEnricherSettings.Nodes.Shared.IntegerNode;

            // Assert
            Assert.Equal(16, settings.InitialBufferSize);
            Assert.Equal(64, settings.MaxBufferSize);
            Assert.Equal(2, settings.GrowthFactor);
            Assert.Equal(64, settings.MaxStackAllocLength);
            Assert.Equal(64, settings.MaxPooledArrayLength);
        }

        [Fact]
        public void IntegerNode_ModificationPersistsAcrossMultipleAccesses()
        {
            // Arrange
            StringEnricherSettings.EnableDebugLogs = false;
            StringEnricherSettings.Nodes.Shared.ResetIntegerNodeSettings();
            StringEnricherSettings.Nodes.Shared.IntegerNode.InitialBufferSize = 24;

            // Act
            var firstAccess = StringEnricherSettings.Nodes.Shared.IntegerNode.InitialBufferSize;
            var secondAccess = StringEnricherSettings.Nodes.Shared.IntegerNode.InitialBufferSize;

            // Assert
            Assert.Equal(24, firstAccess);
            Assert.Equal(24, secondAccess);

            // Cleanup
            StringEnricherSettings.Nodes.Shared.ResetIntegerNodeSettings();
        }
    }
}
