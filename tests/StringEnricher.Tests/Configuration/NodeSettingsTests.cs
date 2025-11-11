using StringEnricher.Configuration;

namespace StringEnricher.Tests.Configuration;

public class NodeSettingsTests
{
    [Fact]
    public void Constructor_WithBufferSizesAndThresholds_InitializesCorrectly()
    {
        // Arrange
        var bufferSizes = new BufferSizes("TestNode", 100, 1000, 2.0f);
        var thresholds = new BufferAllocationThresholds("TestNode", 256, 500000);

        // Act
        var settings = new NodeSettings("TestNode", bufferSizes, thresholds);

        // Assert
        Assert.Equal("TestNode", settings.Name);
        Assert.Equal(100, settings.InitialBufferSize);
        Assert.Equal(1000, settings.MaxBufferSize);
        Assert.Equal(2.0f, settings.GrowthFactor);
        Assert.Equal(256, settings.MaxStackAllocLength);
        Assert.Equal(500000, settings.MaxPooledArrayLength);
    }

    [Fact]
    public void Constructor_WithIndividualParameters_InitializesCorrectly()
    {
        // Arrange & Act
        var settings = new NodeSettings(
            "TestNode",
            initialBufferLength: 150,
            maxBufferLength: 2000,
            growthFactor: 1.5f,
            maxStackAllocLength: 512,
            maxPooledArrayLength: 800000
        );

        // Assert
        Assert.Equal("TestNode", settings.Name);
        Assert.Equal(150, settings.InitialBufferSize);
        Assert.Equal(2000, settings.MaxBufferSize);
        Assert.Equal(1.5f, settings.GrowthFactor);
        Assert.Equal(512, settings.MaxStackAllocLength);
        Assert.Equal(800000, settings.MaxPooledArrayLength);
    }

    [Fact]
    public void GrowthFactor_PropertyGetterAndSetter_WorksCorrectly()
    {
        // Arrange
        StringEnricherSettings.EnableDebugLogs = false;
        var settings = new NodeSettings("TestNode", 100, 1000, 2.0f, 256, 500000);

        // Act
        settings.GrowthFactor = 3.0f;

        // Assert
        Assert.Equal(3.0f, settings.GrowthFactor);
    }

    [Fact]
    public void InitialBufferSize_PropertyGetterAndSetter_WorksCorrectly()
    {
        // Arrange
        StringEnricherSettings.EnableDebugLogs = false;
        var settings = new NodeSettings("TestNode", 100, 1000, 2.0f, 256, 500000);

        // Act
        settings.InitialBufferSize = 200;

        // Assert
        Assert.Equal(200, settings.InitialBufferSize);
    }

    [Fact]
    public void MaxBufferSize_PropertyGetterAndSetter_WorksCorrectly()
    {
        // Arrange
        StringEnricherSettings.EnableDebugLogs = false;
        var settings = new NodeSettings("TestNode", 100, 1000, 2.0f, 256, 500000);

        // Act
        settings.MaxBufferSize = 5000;

        // Assert
        Assert.Equal(5000, settings.MaxBufferSize);
    }

    [Fact]
    public void MaxStackAllocLength_PropertyGetterAndSetter_WorksCorrectly()
    {
        // Arrange
        StringEnricherSettings.EnableDebugLogs = false;
        var settings = new NodeSettings("TestNode", 100, 1000, 2.0f, 256, 500000);

        // Act
        settings.MaxStackAllocLength = 1024;

        // Assert
        Assert.Equal(1024, settings.MaxStackAllocLength);
    }

    [Fact]
    public void MaxPooledArrayLength_PropertyGetterAndSetter_WorksCorrectly()
    {
        // Arrange
        StringEnricherSettings.EnableDebugLogs = false;
        var settings = new NodeSettings("TestNode", 100, 1000, 2.0f, 256, 500000);

        // Act
        settings.MaxPooledArrayLength = 1000000;

        // Assert
        Assert.Equal(1000000, settings.MaxPooledArrayLength);
    }

    [Fact]
    public void Properties_ModifyUnderlyingStructs_ChangesReflectedThroughGetters()
    {
        // Arrange
        StringEnricherSettings.EnableDebugLogs = false;
        var settings = new NodeSettings("TestNode", 100, 1000, 2.0f, 256, 500000);

        // Act - modify multiple properties
        settings.InitialBufferSize = 300;
        settings.MaxBufferSize = 3000;
        settings.GrowthFactor = 2.5f;
        settings.MaxStackAllocLength = 512;
        settings.MaxPooledArrayLength = 750000;

        // Assert - verify all changes persist
        Assert.Equal(300, settings.InitialBufferSize);
        Assert.Equal(3000, settings.MaxBufferSize);
        Assert.Equal(2.5f, settings.GrowthFactor);
        Assert.Equal(512, settings.MaxStackAllocLength);
        Assert.Equal(750000, settings.MaxPooledArrayLength);
    }

    [Fact]
    public void Name_Property_IsReadOnly()
    {
        // Arrange
        var settings = new NodeSettings("TestNode", 100, 1000, 2.0f, 256, 500000);

        // Assert - Name should only have getter
        Assert.Equal("TestNode", settings.Name);
    }
}