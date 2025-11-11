using StringEnricher.Configuration;

namespace StringEnricher.Tests.Configuration;

public partial class StringEnricherSettingsTests
{
    public class TimeSpanNodeTests
    {
        [Fact]
        public void TimeSpanNode_InitializesWithDefaultValues()
        {
            // Arrange & Act
            StringEnricherSettings.Nodes.Shared.ResetTimeSpanNodeSettings();
            var settings = StringEnricherSettings.Nodes.Shared.TimeSpanNode;

            // Assert
            Assert.Equal("StringEnricherSettings.Nodes.Shared.TimeSpanNode", settings.Name);
            Assert.Equal(32, settings.InitialBufferSize);
            Assert.Equal(128, settings.MaxBufferSize);
            Assert.Equal(2, settings.GrowthFactor);
            Assert.Equal(128, settings.MaxStackAllocLength);
            Assert.Equal(128, settings.MaxPooledArrayLength);
        }

        [Fact]
        public void TimeSpanNode_CanBeModified()
        {
            // Arrange
            StringEnricherSettings.EnableDebugLogs = false;
            StringEnricherSettings.Nodes.Shared.ResetTimeSpanNodeSettings();

            // Act
            StringEnricherSettings.Nodes.Shared.TimeSpanNode.InitialBufferSize = 64;
            StringEnricherSettings.Nodes.Shared.TimeSpanNode.MaxBufferSize = 256;
            StringEnricherSettings.Nodes.Shared.TimeSpanNode.GrowthFactor = 3.0f;
            StringEnricherSettings.Nodes.Shared.TimeSpanNode.MaxPooledArrayLength = 16384;
            StringEnricherSettings.Nodes.Shared.TimeSpanNode.MaxStackAllocLength = 512;

            // Assert
            Assert.Equal(64, StringEnricherSettings.Nodes.Shared.TimeSpanNode.InitialBufferSize);
            Assert.Equal(256, StringEnricherSettings.Nodes.Shared.TimeSpanNode.MaxBufferSize);
            Assert.Equal(3.0f, StringEnricherSettings.Nodes.Shared.TimeSpanNode.GrowthFactor);
            Assert.Equal(512, StringEnricherSettings.Nodes.Shared.TimeSpanNode.MaxStackAllocLength);
            Assert.Equal(16384, StringEnricherSettings.Nodes.Shared.TimeSpanNode.MaxPooledArrayLength);

            // Cleanup
            StringEnricherSettings.Nodes.Shared.ResetTimeSpanNodeSettings();
        }

        [Fact]
        public void ResetTimeSpanNodeSettings_RestoresDefaultValues()
        {
            // Arrange
            StringEnricherSettings.EnableDebugLogs = false;
            StringEnricherSettings.Nodes.Shared.TimeSpanNode.MaxBufferSize = 500;
            StringEnricherSettings.Nodes.Shared.TimeSpanNode.InitialBufferSize = 100;
            StringEnricherSettings.Nodes.Shared.TimeSpanNode.GrowthFactor = 5.0f;
            StringEnricherSettings.Nodes.Shared.TimeSpanNode.MaxPooledArrayLength = 50000;
            StringEnricherSettings.Nodes.Shared.TimeSpanNode.MaxStackAllocLength = 1024;

            // Act
            StringEnricherSettings.Nodes.Shared.ResetTimeSpanNodeSettings();

            // Assert
            Assert.Equal(32, StringEnricherSettings.Nodes.Shared.TimeSpanNode.InitialBufferSize);
            Assert.Equal(128, StringEnricherSettings.Nodes.Shared.TimeSpanNode.MaxBufferSize);
            Assert.Equal(2, StringEnricherSettings.Nodes.Shared.TimeSpanNode.GrowthFactor);
            Assert.Equal(128, StringEnricherSettings.Nodes.Shared.TimeSpanNode.MaxStackAllocLength);
            Assert.Equal(128, StringEnricherSettings.Nodes.Shared.TimeSpanNode.MaxPooledArrayLength);
        }

        [Fact]
        public void ResetTimeSpanNodeSettings_CanBeCalledMultipleTimes()
        {
            // Arrange & Act
            StringEnricherSettings.Nodes.Shared.ResetTimeSpanNodeSettings();
            StringEnricherSettings.Nodes.Shared.ResetTimeSpanNodeSettings();
            var settings = StringEnricherSettings.Nodes.Shared.TimeSpanNode;

            // Assert
            Assert.Equal(32, settings.InitialBufferSize);
            Assert.Equal(128, settings.MaxBufferSize);
            Assert.Equal(2, settings.GrowthFactor);
            Assert.Equal(128, settings.MaxStackAllocLength);
            Assert.Equal(128, settings.MaxPooledArrayLength);
        }

        [Fact]
        public void TimeSpanNode_ModificationPersistsAcrossMultipleAccesses()
        {
            // Arrange
            StringEnricherSettings.EnableDebugLogs = false;
            StringEnricherSettings.Nodes.Shared.ResetTimeSpanNodeSettings();
            StringEnricherSettings.Nodes.Shared.TimeSpanNode.InitialBufferSize = 80;

            // Act
            var firstAccess = StringEnricherSettings.Nodes.Shared.TimeSpanNode.InitialBufferSize;
            var secondAccess = StringEnricherSettings.Nodes.Shared.TimeSpanNode.InitialBufferSize;

            // Assert
            Assert.Equal(80, firstAccess);
            Assert.Equal(80, secondAccess);

            // Cleanup
            StringEnricherSettings.Nodes.Shared.ResetTimeSpanNodeSettings();
        }
    }
}
