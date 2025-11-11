using StringEnricher.Configuration;

namespace StringEnricher.Tests.Configuration;

public partial class StringEnricherSettingsTests
{
    public class DateOnlyNodeTests
    {
        [Fact]
        public void DateOnlyNode_InitializesWithDefaultValues()
        {
            // Arrange & Act
            StringEnricherSettings.Nodes.Shared.ResetDateOnlyNodeSettings();
            var settings = StringEnricherSettings.Nodes.Shared.DateOnlyNode;

            // Assert
            Assert.Equal("StringEnricherSettings.Nodes.Shared.DateOnlyNode", settings.Name);
            Assert.Equal(16, settings.InitialBufferSize);
            Assert.Equal(256, settings.MaxBufferSize);
            Assert.Equal(2, settings.GrowthFactor);
            Assert.Equal(256, settings.MaxStackAllocLength);
            Assert.Equal(256, settings.MaxPooledArrayLength);
        }

        [Fact]
        public void DateOnlyNode_CanBeModified()
        {
            // Arrange
            StringEnricherSettings.EnableDebugLogs = false;
            StringEnricherSettings.Nodes.Shared.ResetDateOnlyNodeSettings();

            // Act
            StringEnricherSettings.Nodes.Shared.DateOnlyNode.InitialBufferSize = 32;
            StringEnricherSettings.Nodes.Shared.DateOnlyNode.MaxBufferSize = 512;
            StringEnricherSettings.Nodes.Shared.DateOnlyNode.GrowthFactor = 3.0f;
            StringEnricherSettings.Nodes.Shared.DateOnlyNode.MaxPooledArrayLength = 16384;
            StringEnricherSettings.Nodes.Shared.DateOnlyNode.MaxStackAllocLength = 1024;

            // Assert
            Assert.Equal(32, StringEnricherSettings.Nodes.Shared.DateOnlyNode.InitialBufferSize);
            Assert.Equal(512, StringEnricherSettings.Nodes.Shared.DateOnlyNode.MaxBufferSize);
            Assert.Equal(3.0f, StringEnricherSettings.Nodes.Shared.DateOnlyNode.GrowthFactor);
            Assert.Equal(1024, StringEnricherSettings.Nodes.Shared.DateOnlyNode.MaxStackAllocLength);
            Assert.Equal(16384, StringEnricherSettings.Nodes.Shared.DateOnlyNode.MaxPooledArrayLength);

            // Cleanup
            StringEnricherSettings.Nodes.Shared.ResetDateOnlyNodeSettings();
        }

        [Fact]
        public void ResetDateOnlyNodeSettings_RestoresDefaultValues()
        {
            // Arrange
            StringEnricherSettings.EnableDebugLogs = false;
            StringEnricherSettings.Nodes.Shared.DateOnlyNode.MaxBufferSize = 500;
            StringEnricherSettings.Nodes.Shared.DateOnlyNode.InitialBufferSize = 100;
            StringEnricherSettings.Nodes.Shared.DateOnlyNode.GrowthFactor = 5.0f;
            StringEnricherSettings.Nodes.Shared.DateOnlyNode.MaxPooledArrayLength = 50000;
            StringEnricherSettings.Nodes.Shared.DateOnlyNode.MaxStackAllocLength = 2048;

            // Act
            StringEnricherSettings.Nodes.Shared.ResetDateOnlyNodeSettings();

            // Assert
            Assert.Equal(16, StringEnricherSettings.Nodes.Shared.DateOnlyNode.InitialBufferSize);
            Assert.Equal(256, StringEnricherSettings.Nodes.Shared.DateOnlyNode.MaxBufferSize);
            Assert.Equal(2, StringEnricherSettings.Nodes.Shared.DateOnlyNode.GrowthFactor);
            Assert.Equal(256, StringEnricherSettings.Nodes.Shared.DateOnlyNode.MaxStackAllocLength);
            Assert.Equal(256, StringEnricherSettings.Nodes.Shared.DateOnlyNode.MaxPooledArrayLength);
        }

        [Fact]
        public void ResetDateOnlyNodeSettings_CanBeCalledMultipleTimes()
        {
            // Arrange & Act
            StringEnricherSettings.Nodes.Shared.ResetDateOnlyNodeSettings();
            StringEnricherSettings.Nodes.Shared.ResetDateOnlyNodeSettings();
            var settings = StringEnricherSettings.Nodes.Shared.DateOnlyNode;

            // Assert
            Assert.Equal(16, settings.InitialBufferSize);
            Assert.Equal(256, settings.MaxBufferSize);
            Assert.Equal(2, settings.GrowthFactor);
            Assert.Equal(256, settings.MaxStackAllocLength);
            Assert.Equal(256, settings.MaxPooledArrayLength);
        }

        [Fact]
        public void DateOnlyNode_ModificationPersistsAcrossMultipleAccesses()
        {
            // Arrange
            StringEnricherSettings.EnableDebugLogs = false;
            StringEnricherSettings.Nodes.Shared.ResetDateOnlyNodeSettings();
            StringEnricherSettings.Nodes.Shared.DateOnlyNode.InitialBufferSize = 64;

            // Act
            var firstAccess = StringEnricherSettings.Nodes.Shared.DateOnlyNode.InitialBufferSize;
            var secondAccess = StringEnricherSettings.Nodes.Shared.DateOnlyNode.InitialBufferSize;

            // Assert
            Assert.Equal(64, firstAccess);
            Assert.Equal(64, secondAccess);

            // Cleanup
            StringEnricherSettings.Nodes.Shared.ResetDateOnlyNodeSettings();
        }
    }
}
