using StringEnricher.Configuration;

namespace StringEnricher.Tests.Configuration;

public partial class StringEnricherSettingsTests
{
    public class DateTimeOffsetNodeTests
    {
        [Fact]
        public void DateTimeOffsetNode_InitializesWithDefaultValues()
        {
            // Arrange & Act
            StringEnricherSettings.Nodes.Shared.ResetDateTimeOffsetNodeSettings();
            var settings = StringEnricherSettings.Nodes.Shared.DateTimeOffsetNode;

            // Assert
            Assert.Equal("StringEnricherSettings.Nodes.Shared.DateTimeOffsetNode", settings.Name);
            Assert.Equal(32, settings.InitialBufferSize);
            Assert.Equal(512, settings.MaxBufferSize);
            Assert.Equal(2, settings.GrowthFactor);
            Assert.Equal(512, settings.MaxStackAllocLength);
            Assert.Equal(512, settings.MaxPooledArrayLength);
        }

        [Fact]
        public void DateTimeOffsetNode_CanBeModified()
        {
            // Arrange
            StringEnricherSettings.EnableDebugLogs = false;
            StringEnricherSettings.Nodes.Shared.ResetDateTimeOffsetNodeSettings();

            // Act
            StringEnricherSettings.Nodes.Shared.DateTimeOffsetNode.InitialBufferSize = 64;
            StringEnricherSettings.Nodes.Shared.DateTimeOffsetNode.MaxBufferSize = 1024;
            StringEnricherSettings.Nodes.Shared.DateTimeOffsetNode.GrowthFactor = 3.0f;
            StringEnricherSettings.Nodes.Shared.DateTimeOffsetNode.MaxPooledArrayLength = 16384;
            StringEnricherSettings.Nodes.Shared.DateTimeOffsetNode.MaxStackAllocLength = 2048;

            // Assert
            Assert.Equal(64, StringEnricherSettings.Nodes.Shared.DateTimeOffsetNode.InitialBufferSize);
            Assert.Equal(1024, StringEnricherSettings.Nodes.Shared.DateTimeOffsetNode.MaxBufferSize);
            Assert.Equal(3.0f, StringEnricherSettings.Nodes.Shared.DateTimeOffsetNode.GrowthFactor);
            Assert.Equal(2048, StringEnricherSettings.Nodes.Shared.DateTimeOffsetNode.MaxStackAllocLength);
            Assert.Equal(16384, StringEnricherSettings.Nodes.Shared.DateTimeOffsetNode.MaxPooledArrayLength);

            // Cleanup
            StringEnricherSettings.Nodes.Shared.ResetDateTimeOffsetNodeSettings();
        }

        [Fact]
        public void ResetDateTimeOffsetNodeSettings_RestoresDefaultValues()
        {
            // Arrange
            StringEnricherSettings.EnableDebugLogs = false;
            StringEnricherSettings.Nodes.Shared.DateTimeOffsetNode.MaxBufferSize = 500;
            StringEnricherSettings.Nodes.Shared.DateTimeOffsetNode.InitialBufferSize = 100;
            StringEnricherSettings.Nodes.Shared.DateTimeOffsetNode.GrowthFactor = 5.0f;
            StringEnricherSettings.Nodes.Shared.DateTimeOffsetNode.MaxPooledArrayLength = 50000;
            StringEnricherSettings.Nodes.Shared.DateTimeOffsetNode.MaxStackAllocLength = 2048;

            // Act
            StringEnricherSettings.Nodes.Shared.ResetDateTimeOffsetNodeSettings();

            // Assert
            Assert.Equal(32, StringEnricherSettings.Nodes.Shared.DateTimeOffsetNode.InitialBufferSize);
            Assert.Equal(512, StringEnricherSettings.Nodes.Shared.DateTimeOffsetNode.MaxBufferSize);
            Assert.Equal(2, StringEnricherSettings.Nodes.Shared.DateTimeOffsetNode.GrowthFactor);
            Assert.Equal(512, StringEnricherSettings.Nodes.Shared.DateTimeOffsetNode.MaxStackAllocLength);
            Assert.Equal(512, StringEnricherSettings.Nodes.Shared.DateTimeOffsetNode.MaxPooledArrayLength);
        }

        [Fact]
        public void ResetDateTimeOffsetNodeSettings_CanBeCalledMultipleTimes()
        {
            // Arrange & Act
            StringEnricherSettings.Nodes.Shared.ResetDateTimeOffsetNodeSettings();
            StringEnricherSettings.Nodes.Shared.ResetDateTimeOffsetNodeSettings();
            var settings = StringEnricherSettings.Nodes.Shared.DateTimeOffsetNode;

            // Assert
            Assert.Equal(32, settings.InitialBufferSize);
            Assert.Equal(512, settings.MaxBufferSize);
            Assert.Equal(2, settings.GrowthFactor);
            Assert.Equal(512, settings.MaxStackAllocLength);
            Assert.Equal(512, settings.MaxPooledArrayLength);
        }

        [Fact]
        public void DateTimeOffsetNode_ModificationPersistsAcrossMultipleAccesses()
        {
            // Arrange
            StringEnricherSettings.EnableDebugLogs = false;
            StringEnricherSettings.Nodes.Shared.ResetDateTimeOffsetNodeSettings();
            StringEnricherSettings.Nodes.Shared.DateTimeOffsetNode.InitialBufferSize = 128;

            // Act
            var firstAccess = StringEnricherSettings.Nodes.Shared.DateTimeOffsetNode.InitialBufferSize;
            var secondAccess = StringEnricherSettings.Nodes.Shared.DateTimeOffsetNode.InitialBufferSize;

            // Assert
            Assert.Equal(128, firstAccess);
            Assert.Equal(128, secondAccess);

            // Cleanup
            StringEnricherSettings.Nodes.Shared.ResetDateTimeOffsetNodeSettings();
        }
    }
}
