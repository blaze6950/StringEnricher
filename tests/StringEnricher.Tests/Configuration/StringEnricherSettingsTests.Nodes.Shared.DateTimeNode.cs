using StringEnricher.Configuration;

namespace StringEnricher.Tests.Configuration;

public partial class StringEnricherSettingsTests
{
    public class DateTimeNodeTests
    {
        [Fact]
        public void DateTimeNode_InitializesWithDefaultValues()
        {
            // Arrange & Act
            StringEnricherSettings.Nodes.Shared.ResetDateTimeNodeSettings();
            var settings = StringEnricherSettings.Nodes.Shared.DateTimeNode;

            // Assert
            Assert.Equal("StringEnricherSettings.Nodes.Shared.DateTimeNode", settings.Name);
            Assert.Equal(32, settings.InitialBufferSize);
            Assert.Equal(512, settings.MaxBufferSize);
            Assert.Equal(2, settings.GrowthFactor);
            Assert.Equal(512, settings.MaxStackAllocLength);
            Assert.Equal(512, settings.MaxPooledArrayLength);
        }

        [Fact]
        public void DateTimeNode_CanBeModified()
        {
            // Arrange
            StringEnricherSettings.EnableDebugLogs = false;
            StringEnricherSettings.Nodes.Shared.ResetDateTimeNodeSettings();

            // Act
            StringEnricherSettings.Nodes.Shared.DateTimeNode.InitialBufferSize = 64;
            StringEnricherSettings.Nodes.Shared.DateTimeNode.MaxBufferSize = 1024;
            StringEnricherSettings.Nodes.Shared.DateTimeNode.GrowthFactor = 3.0f;
            StringEnricherSettings.Nodes.Shared.DateTimeNode.MaxPooledArrayLength = 16384;
            StringEnricherSettings.Nodes.Shared.DateTimeNode.MaxStackAllocLength = 2048;

            // Assert
            Assert.Equal(64, StringEnricherSettings.Nodes.Shared.DateTimeNode.InitialBufferSize);
            Assert.Equal(1024, StringEnricherSettings.Nodes.Shared.DateTimeNode.MaxBufferSize);
            Assert.Equal(3.0f, StringEnricherSettings.Nodes.Shared.DateTimeNode.GrowthFactor);
            Assert.Equal(2048, StringEnricherSettings.Nodes.Shared.DateTimeNode.MaxStackAllocLength);
            Assert.Equal(16384, StringEnricherSettings.Nodes.Shared.DateTimeNode.MaxPooledArrayLength);

            // Cleanup
            StringEnricherSettings.Nodes.Shared.ResetDateTimeNodeSettings();
        }

        [Fact]
        public void ResetDateTimeNodeSettings_RestoresDefaultValues()
        {
            // Arrange
            StringEnricherSettings.EnableDebugLogs = false;
            StringEnricherSettings.Nodes.Shared.DateTimeNode.MaxBufferSize = 500;
            StringEnricherSettings.Nodes.Shared.DateTimeNode.InitialBufferSize = 100;
            StringEnricherSettings.Nodes.Shared.DateTimeNode.GrowthFactor = 5.0f;
            StringEnricherSettings.Nodes.Shared.DateTimeNode.MaxPooledArrayLength = 50000;
            StringEnricherSettings.Nodes.Shared.DateTimeNode.MaxStackAllocLength = 2048;

            // Act
            StringEnricherSettings.Nodes.Shared.ResetDateTimeNodeSettings();

            // Assert
            Assert.Equal(32, StringEnricherSettings.Nodes.Shared.DateTimeNode.InitialBufferSize);
            Assert.Equal(512, StringEnricherSettings.Nodes.Shared.DateTimeNode.MaxBufferSize);
            Assert.Equal(2, StringEnricherSettings.Nodes.Shared.DateTimeNode.GrowthFactor);
            Assert.Equal(512, StringEnricherSettings.Nodes.Shared.DateTimeNode.MaxStackAllocLength);
            Assert.Equal(512, StringEnricherSettings.Nodes.Shared.DateTimeNode.MaxPooledArrayLength);
        }

        [Fact]
        public void ResetDateTimeNodeSettings_CanBeCalledMultipleTimes()
        {
            // Arrange & Act
            StringEnricherSettings.Nodes.Shared.ResetDateTimeNodeSettings();
            StringEnricherSettings.Nodes.Shared.ResetDateTimeNodeSettings();
            var settings = StringEnricherSettings.Nodes.Shared.DateTimeNode;

            // Assert
            Assert.Equal(32, settings.InitialBufferSize);
            Assert.Equal(512, settings.MaxBufferSize);
            Assert.Equal(2, settings.GrowthFactor);
            Assert.Equal(512, settings.MaxStackAllocLength);
            Assert.Equal(512, settings.MaxPooledArrayLength);
        }

        [Fact]
        public void DateTimeNode_ModificationPersistsAcrossMultipleAccesses()
        {
            // Arrange
            StringEnricherSettings.EnableDebugLogs = false;
            StringEnricherSettings.Nodes.Shared.ResetDateTimeNodeSettings();
            StringEnricherSettings.Nodes.Shared.DateTimeNode.InitialBufferSize = 128;

            // Act
            var firstAccess = StringEnricherSettings.Nodes.Shared.DateTimeNode.InitialBufferSize;
            var secondAccess = StringEnricherSettings.Nodes.Shared.DateTimeNode.InitialBufferSize;

            // Assert
            Assert.Equal(128, firstAccess);
            Assert.Equal(128, secondAccess);

            // Cleanup
            StringEnricherSettings.Nodes.Shared.ResetDateTimeNodeSettings();
        }
    }
}
