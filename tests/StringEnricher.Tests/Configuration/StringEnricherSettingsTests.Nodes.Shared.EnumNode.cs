using StringEnricher.Configuration;

namespace StringEnricher.Tests.Configuration;

public partial class StringEnricherSettingsTests
{
    public class EnumNodeTests
    {
        [Fact]
        public void EnumNode_InitializesWithDefaultValues()
        {
            // Arrange & Act
            StringEnricherSettings.Nodes.Shared.ResetEnumNodeSettings();
            var settings = StringEnricherSettings.Nodes.Shared.EnumNode;

            // Assert
            Assert.Equal("StringEnricherSettings.Nodes.Shared.EnumNode", settings.Name);
            Assert.Equal(16, settings.InitialBufferSize);
            Assert.Equal(512, settings.MaxBufferSize);
            Assert.Equal(2, settings.GrowthFactor);
            Assert.Equal(512, settings.MaxStackAllocLength);
            Assert.Equal(512, settings.MaxPooledArrayLength);
        }

        [Fact]
        public void EnumNode_CanBeModified()
        {
            // Arrange
            StringEnricherSettings.EnableDebugLogs = false;
            StringEnricherSettings.Nodes.Shared.ResetEnumNodeSettings();

            // Act
            StringEnricherSettings.Nodes.Shared.EnumNode.InitialBufferSize = 32;
            StringEnricherSettings.Nodes.Shared.EnumNode.MaxBufferSize = 1024;
            StringEnricherSettings.Nodes.Shared.EnumNode.GrowthFactor = 3.0f;
            StringEnricherSettings.Nodes.Shared.EnumNode.MaxPooledArrayLength = 65536;
            StringEnricherSettings.Nodes.Shared.EnumNode.MaxStackAllocLength = 1024;

            // Assert
            Assert.Equal(32, StringEnricherSettings.Nodes.Shared.EnumNode.InitialBufferSize);
            Assert.Equal(1024, StringEnricherSettings.Nodes.Shared.EnumNode.MaxBufferSize);
            Assert.Equal(3.0f, StringEnricherSettings.Nodes.Shared.EnumNode.GrowthFactor);
            Assert.Equal(1024, StringEnricherSettings.Nodes.Shared.EnumNode.MaxStackAllocLength);
            Assert.Equal(65536, StringEnricherSettings.Nodes.Shared.EnumNode.MaxPooledArrayLength);

            // Cleanup
            StringEnricherSettings.Nodes.Shared.ResetEnumNodeSettings();
        }

        [Fact]
        public void ResetEnumNodeSettings_RestoresDefaultValues()
        {
            // Arrange
            StringEnricherSettings.EnableDebugLogs = false;
            StringEnricherSettings.Nodes.Shared.EnumNode.MaxBufferSize = 2048;
            StringEnricherSettings.Nodes.Shared.EnumNode.InitialBufferSize = 64;
            StringEnricherSettings.Nodes.Shared.EnumNode.GrowthFactor = 5.0f;
            StringEnricherSettings.Nodes.Shared.EnumNode.MaxPooledArrayLength = 100000;
            StringEnricherSettings.Nodes.Shared.EnumNode.MaxStackAllocLength = 2048;

            // Act
            StringEnricherSettings.Nodes.Shared.ResetEnumNodeSettings();

            // Assert
            Assert.Equal(16, StringEnricherSettings.Nodes.Shared.EnumNode.InitialBufferSize);
            Assert.Equal(512, StringEnricherSettings.Nodes.Shared.EnumNode.MaxBufferSize);
            Assert.Equal(2, StringEnricherSettings.Nodes.Shared.EnumNode.GrowthFactor);
            Assert.Equal(512, StringEnricherSettings.Nodes.Shared.EnumNode.MaxStackAllocLength);
            Assert.Equal(512, StringEnricherSettings.Nodes.Shared.EnumNode.MaxPooledArrayLength);
        }

        [Fact]
        public void ResetEnumNodeSettings_CanBeCalledMultipleTimes()
        {
            // Arrange & Act
            StringEnricherSettings.Nodes.Shared.ResetEnumNodeSettings();
            StringEnricherSettings.Nodes.Shared.ResetEnumNodeSettings();
            var settings = StringEnricherSettings.Nodes.Shared.EnumNode;

            // Assert
            Assert.Equal(16, settings.InitialBufferSize);
            Assert.Equal(512, settings.MaxBufferSize);
            Assert.Equal(2, settings.GrowthFactor);
            Assert.Equal(512, settings.MaxStackAllocLength);
            Assert.Equal(512, settings.MaxPooledArrayLength);
        }

        [Fact]
        public void EnumNode_ModificationPersistsAcrossMultipleAccesses()
        {
            // Arrange
            StringEnricherSettings.EnableDebugLogs = false;
            StringEnricherSettings.Nodes.Shared.ResetEnumNodeSettings();
            StringEnricherSettings.Nodes.Shared.EnumNode.InitialBufferSize = 24;

            // Act
            var firstAccess = StringEnricherSettings.Nodes.Shared.EnumNode.InitialBufferSize;
            var secondAccess = StringEnricherSettings.Nodes.Shared.EnumNode.InitialBufferSize;

            // Assert
            Assert.Equal(24, firstAccess);
            Assert.Equal(24, secondAccess);

            // Cleanup
            StringEnricherSettings.Nodes.Shared.ResetEnumNodeSettings();
        }
    }
}
