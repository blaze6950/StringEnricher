using StringEnricher.Configuration;

namespace StringEnricher.Tests.Configuration;

public partial class StringEnricherSettingsTests
{
    public class UIntegerNodeTests
    {
        [Fact]
        public void UIntegerNode_InitializesWithDefaultValues()
        {
            // Arrange & Act
            StringEnricherSettings.Nodes.Shared.ResetUIntegerNodeSettings();
            var settings = StringEnricherSettings.Nodes.Shared.UIntegerNode;

            // Assert
            Assert.Equal("StringEnricherSettings.Nodes.Shared.UIntegerNode", settings.Name);
            Assert.Equal(16, settings.InitialBufferSize);
            Assert.Equal(64, settings.MaxBufferSize);
            Assert.Equal(2, settings.GrowthFactor);
            Assert.Equal(64, settings.MaxStackAllocLength);
            Assert.Equal(64, settings.MaxPooledArrayLength);
        }

        [Fact]
        public void UIntegerNode_CanBeModified()
        {
            // Arrange
            StringEnricherSettings.EnableDebugLogs = false;
            StringEnricherSettings.Nodes.Shared.ResetUIntegerNodeSettings();

            // Act
            StringEnricherSettings.Nodes.Shared.UIntegerNode.InitialBufferSize = 32;
            StringEnricherSettings.Nodes.Shared.UIntegerNode.MaxBufferSize = 128;
            StringEnricherSettings.Nodes.Shared.UIntegerNode.GrowthFactor = 3.0f;
            StringEnricherSettings.Nodes.Shared.UIntegerNode.MaxPooledArrayLength = 16384;
            StringEnricherSettings.Nodes.Shared.UIntegerNode.MaxStackAllocLength = 256;

            // Assert
            Assert.Equal(32, StringEnricherSettings.Nodes.Shared.UIntegerNode.InitialBufferSize);
            Assert.Equal(128, StringEnricherSettings.Nodes.Shared.UIntegerNode.MaxBufferSize);
            Assert.Equal(3.0f, StringEnricherSettings.Nodes.Shared.UIntegerNode.GrowthFactor);
            Assert.Equal(256, StringEnricherSettings.Nodes.Shared.UIntegerNode.MaxStackAllocLength);
            Assert.Equal(16384, StringEnricherSettings.Nodes.Shared.UIntegerNode.MaxPooledArrayLength);

            // Cleanup
            StringEnricherSettings.Nodes.Shared.ResetUIntegerNodeSettings();
        }

        [Fact]
        public void ResetUIntegerNodeSettings_RestoresDefaultValues()
        {
            // Arrange
            StringEnricherSettings.EnableDebugLogs = false;
            StringEnricherSettings.Nodes.Shared.UIntegerNode.MaxBufferSize = 500;
            StringEnricherSettings.Nodes.Shared.UIntegerNode.InitialBufferSize = 100;
            StringEnricherSettings.Nodes.Shared.UIntegerNode.GrowthFactor = 5.0f;
            StringEnricherSettings.Nodes.Shared.UIntegerNode.MaxPooledArrayLength = 50000;
            StringEnricherSettings.Nodes.Shared.UIntegerNode.MaxStackAllocLength = 1024;

            // Act
            StringEnricherSettings.Nodes.Shared.ResetUIntegerNodeSettings();

            // Assert
            Assert.Equal(16, StringEnricherSettings.Nodes.Shared.UIntegerNode.InitialBufferSize);
            Assert.Equal(64, StringEnricherSettings.Nodes.Shared.UIntegerNode.MaxBufferSize);
            Assert.Equal(2, StringEnricherSettings.Nodes.Shared.UIntegerNode.GrowthFactor);
            Assert.Equal(64, StringEnricherSettings.Nodes.Shared.UIntegerNode.MaxStackAllocLength);
            Assert.Equal(64, StringEnricherSettings.Nodes.Shared.UIntegerNode.MaxPooledArrayLength);
        }

        [Fact]
        public void ResetUIntegerNodeSettings_CanBeCalledMultipleTimes()
        {
            // Arrange & Act
            StringEnricherSettings.Nodes.Shared.ResetUIntegerNodeSettings();
            StringEnricherSettings.Nodes.Shared.ResetUIntegerNodeSettings();
            var settings = StringEnricherSettings.Nodes.Shared.UIntegerNode;

            // Assert
            Assert.Equal(16, settings.InitialBufferSize);
            Assert.Equal(64, settings.MaxBufferSize);
            Assert.Equal(2, settings.GrowthFactor);
            Assert.Equal(64, settings.MaxStackAllocLength);
            Assert.Equal(64, settings.MaxPooledArrayLength);
        }

        [Fact]
        public void UIntegerNode_ModificationPersistsAcrossMultipleAccesses()
        {
            // Arrange
            StringEnricherSettings.EnableDebugLogs = false;
            StringEnricherSettings.Nodes.Shared.ResetUIntegerNodeSettings();
            StringEnricherSettings.Nodes.Shared.UIntegerNode.InitialBufferSize = 24;

            // Act
            var firstAccess = StringEnricherSettings.Nodes.Shared.UIntegerNode.InitialBufferSize;
            var secondAccess = StringEnricherSettings.Nodes.Shared.UIntegerNode.InitialBufferSize;

            // Assert
            Assert.Equal(24, firstAccess);
            Assert.Equal(24, secondAccess);

            // Cleanup
            StringEnricherSettings.Nodes.Shared.ResetUIntegerNodeSettings();
        }
    }
}
