using StringEnricher.Configuration;

namespace StringEnricher.Tests.Configuration;

public partial class StringEnricherSettingsTests
{
    public class TimeOnlyNodeTests
    {
        [Fact]
        public void TimeOnlyNode_InitializesWithDefaultValues()
        {
            // Arrange & Act
            StringEnricherSettings.Nodes.Shared.ResetTimeOnlyNodeSettings();
            var settings = StringEnricherSettings.Nodes.Shared.TimeOnlyNode;

            // Assert
            Assert.Equal("StringEnricherSettings.Nodes.Shared.TimeOnlyNode", settings.Name);
            Assert.Equal(32, settings.InitialBufferSize);
            Assert.Equal(128, settings.MaxBufferSize);
            Assert.Equal(2, settings.GrowthFactor);
            Assert.Equal(128, settings.MaxStackAllocLength);
            Assert.Equal(128, settings.MaxPooledArrayLength);
        }

        [Fact]
        public void TimeOnlyNode_CanBeModified()
        {
            // Arrange
            StringEnricherSettings.EnableDebugLogs = false;
            StringEnricherSettings.Nodes.Shared.ResetTimeOnlyNodeSettings();

            // Act
            StringEnricherSettings.Nodes.Shared.TimeOnlyNode.InitialBufferSize = 64;
            StringEnricherSettings.Nodes.Shared.TimeOnlyNode.MaxBufferSize = 256;
            StringEnricherSettings.Nodes.Shared.TimeOnlyNode.GrowthFactor = 3.0f;
            StringEnricherSettings.Nodes.Shared.TimeOnlyNode.MaxPooledArrayLength = 16384;
            StringEnricherSettings.Nodes.Shared.TimeOnlyNode.MaxStackAllocLength = 512;

            // Assert
            Assert.Equal(64, StringEnricherSettings.Nodes.Shared.TimeOnlyNode.InitialBufferSize);
            Assert.Equal(256, StringEnricherSettings.Nodes.Shared.TimeOnlyNode.MaxBufferSize);
            Assert.Equal(3.0f, StringEnricherSettings.Nodes.Shared.TimeOnlyNode.GrowthFactor);
            Assert.Equal(512, StringEnricherSettings.Nodes.Shared.TimeOnlyNode.MaxStackAllocLength);
            Assert.Equal(16384, StringEnricherSettings.Nodes.Shared.TimeOnlyNode.MaxPooledArrayLength);

            // Cleanup
            StringEnricherSettings.Nodes.Shared.ResetTimeOnlyNodeSettings();
        }

        [Fact]
        public void ResetTimeOnlyNodeSettings_RestoresDefaultValues()
        {
            // Arrange
            StringEnricherSettings.EnableDebugLogs = false;
            StringEnricherSettings.Nodes.Shared.TimeOnlyNode.MaxBufferSize = 500;
            StringEnricherSettings.Nodes.Shared.TimeOnlyNode.InitialBufferSize = 100;
            StringEnricherSettings.Nodes.Shared.TimeOnlyNode.GrowthFactor = 5.0f;
            StringEnricherSettings.Nodes.Shared.TimeOnlyNode.MaxPooledArrayLength = 50000;
            StringEnricherSettings.Nodes.Shared.TimeOnlyNode.MaxStackAllocLength = 1024;

            // Act
            StringEnricherSettings.Nodes.Shared.ResetTimeOnlyNodeSettings();

            // Assert
            Assert.Equal(32, StringEnricherSettings.Nodes.Shared.TimeOnlyNode.InitialBufferSize);
            Assert.Equal(128, StringEnricherSettings.Nodes.Shared.TimeOnlyNode.MaxBufferSize);
            Assert.Equal(2, StringEnricherSettings.Nodes.Shared.TimeOnlyNode.GrowthFactor);
            Assert.Equal(128, StringEnricherSettings.Nodes.Shared.TimeOnlyNode.MaxStackAllocLength);
            Assert.Equal(128, StringEnricherSettings.Nodes.Shared.TimeOnlyNode.MaxPooledArrayLength);
        }

        [Fact]
        public void ResetTimeOnlyNodeSettings_CanBeCalledMultipleTimes()
        {
            // Arrange & Act
            StringEnricherSettings.Nodes.Shared.ResetTimeOnlyNodeSettings();
            StringEnricherSettings.Nodes.Shared.ResetTimeOnlyNodeSettings();
            var settings = StringEnricherSettings.Nodes.Shared.TimeOnlyNode;

            // Assert
            Assert.Equal(32, settings.InitialBufferSize);
            Assert.Equal(128, settings.MaxBufferSize);
            Assert.Equal(2, settings.GrowthFactor);
            Assert.Equal(128, settings.MaxStackAllocLength);
            Assert.Equal(128, settings.MaxPooledArrayLength);
        }

        [Fact]
        public void TimeOnlyNode_ModificationPersistsAcrossMultipleAccesses()
        {
            // Arrange
            StringEnricherSettings.EnableDebugLogs = false;
            StringEnricherSettings.Nodes.Shared.ResetTimeOnlyNodeSettings();
            StringEnricherSettings.Nodes.Shared.TimeOnlyNode.InitialBufferSize = 80;

            // Act
            var firstAccess = StringEnricherSettings.Nodes.Shared.TimeOnlyNode.InitialBufferSize;
            var secondAccess = StringEnricherSettings.Nodes.Shared.TimeOnlyNode.InitialBufferSize;

            // Assert
            Assert.Equal(80, firstAccess);
            Assert.Equal(80, secondAccess);

            // Cleanup
            StringEnricherSettings.Nodes.Shared.ResetTimeOnlyNodeSettings();
        }
    }
}
