using StringEnricher.Configuration;

namespace StringEnricher.Tests.Configuration;

public class BufferAllocationThresholdsTests
{
    [Fact]
    public void Constructor_WithNameOnly_InitializesWithDefaultValues()
    {
        // Arrange & Act
        var thresholds = new BufferAllocationThresholds("TestConfig");

        // Assert
        Assert.Equal(512, thresholds.MaxStackAllocLength);
        Assert.Equal(1_000_000, thresholds.MaxPooledArrayLength);
    }

    [Theory]
    [InlineData(128, 100_000)]
    [InlineData(512, 1_000_000)]
    [InlineData(1024, 5_000_000)]
    public void Constructor_WithAllParameters_InitializesCorrectly(int maxStack, int maxPooled)
    {
        // Arrange & Act
        var thresholds = new BufferAllocationThresholds("TestConfig", maxStack, maxPooled);

        // Assert
        Assert.Equal(maxStack, thresholds.MaxStackAllocLength);
        Assert.Equal(maxPooled, thresholds.MaxPooledArrayLength);
    }

    #region MaxStackAllocLength Tests

    [Theory]
    [InlineData(-1)]
    [InlineData(-100)]
    public void Constructor_WithNegativeMaxStackAllocLength_ThrowsArgumentOutOfRangeException(int value)
    {
        // Arrange, Act & Assert
        var exception = Assert.Throws<ArgumentOutOfRangeException>(() =>
            new BufferAllocationThresholds("TestConfig", value, 1_000_000));
        Assert.Contains("must be positive", exception.Message);
    }

    [Fact]
    public void Constructor_WithMaxStackAllocLengthZero_Succeeds()
    {
        // Arrange & Act
        var thresholds = new BufferAllocationThresholds("TestConfig", 0, 1_000_000);

        // Assert
        Assert.Equal(0, thresholds.MaxStackAllocLength);
    }

    [Theory]
    [InlineData(2049)]
    [InlineData(3000)]
    public void Constructor_WithMaxStackAllocLengthAboveHardLimit_ThrowsArgumentOutOfRangeException(int value)
    {
        // Arrange, Act & Assert
        var exception = Assert.Throws<ArgumentOutOfRangeException>(() =>
            new BufferAllocationThresholds("TestConfig", value, 10_000_000));
        Assert.Contains("cannot be greater than 2048", exception.Message);
    }

    [Fact]
    public void Constructor_WithMaxStackAllocLengthGreaterThanMaxPooled_ThrowsArgumentOutOfRangeException()
    {
        // Arrange, Act & Assert
        var exception = Assert.Throws<ArgumentOutOfRangeException>(() =>
            new BufferAllocationThresholds("TestConfig", 2000, 1000));
        Assert.Contains("cannot be greater than MaxPooledArrayLength", exception.Message);
    }

    [Theory]
    [InlineData(128)]
    [InlineData(512)]
    [InlineData(1024)]
    public void MaxStackAllocLength_Setter_WithValidValue_UpdatesValue(int value)
    {
        // Arrange
        StringEnricherSettings.EnableDebugLogs = false;
        var thresholds = new BufferAllocationThresholds("TestConfig", 512, 10_000_000);

        // Act
        thresholds.MaxStackAllocLength = value;

        // Assert
        Assert.Equal(value, thresholds.MaxStackAllocLength);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(-100)]
    public void MaxStackAllocLength_Setter_WithNegativeValue_ThrowsArgumentOutOfRangeException(int value)
    {
        // Arrange
        var thresholds = new BufferAllocationThresholds("TestConfig", 512, 1_000_000);

        // Act & Assert
        var exception = Assert.Throws<ArgumentOutOfRangeException>(() =>
            thresholds.MaxStackAllocLength = value);
        Assert.Contains("must be positive", exception.Message);
    }

    [Theory]
    [InlineData(2049)]
    [InlineData(5000)]
    public void MaxStackAllocLength_Setter_WithValueAboveHardLimit_ThrowsArgumentOutOfRangeException(int value)
    {
        // Arrange
        var thresholds = new BufferAllocationThresholds("TestConfig", 512, 10_000_000);

        // Act & Assert
        var exception = Assert.Throws<ArgumentOutOfRangeException>(() =>
            thresholds.MaxStackAllocLength = value);
        Assert.Contains("cannot be greater than 2048", exception.Message);
    }

    [Fact]
    public void MaxStackAllocLength_Setter_WithValueGreaterThanMaxPooled_ThrowsArgumentOutOfRangeException()
    {
        // Arrange
        var thresholds = new BufferAllocationThresholds("TestConfig", 512, 1000);

        // Act & Assert
        var exception = Assert.Throws<ArgumentOutOfRangeException>(() =>
            thresholds.MaxStackAllocLength = 2000);
        Assert.Contains("cannot be greater than MaxPooledArrayLength", exception.Message);
    }

    [Fact]
    public void MaxStackAllocLength_Setter_WithDebugLogsEnabled_WritesToConsole()
    {
        // Arrange
        StringEnricherSettings.EnableDebugLogs = true;
        var thresholds = new BufferAllocationThresholds("TestConfig", 512, 1_000_000);
        var originalOut = Console.Out;
        using var writer = new StringWriter();
        Console.SetOut(writer);

        // Act
        thresholds.MaxStackAllocLength = 256;

        // Assert
        var output = writer.ToString();
        Assert.Contains("[TestConfig].MaxStackAllocLength set to 256", output);

        // Cleanup
        Console.SetOut(originalOut);
        StringEnricherSettings.EnableDebugLogs = false;
    }

    [Theory]
    [InlineData(16)]
    [InlineData(31)]
    public void Constructor_WithMaxStackAllocLengthBelowSoftLimit_AndDebugLogsEnabled_WritesWarning(int value)
    {
        // Arrange
        StringEnricherSettings.EnableDebugLogs = true;
        var originalOut = Console.Out;
        using var writer = new StringWriter();
        Console.SetOut(writer);

        // Act
        _ = new BufferAllocationThresholds("TestConfig", value, 1_000_000);

        // Assert
        var output = writer.ToString();
        Assert.Contains("WARNING", output);
        Assert.Contains("MaxStackAllocLength", output);
        Assert.Contains($"very low value ({value})", output);

        // Cleanup
        Console.SetOut(originalOut);
        StringEnricherSettings.EnableDebugLogs = false;
    }

    [Theory]
    [InlineData(1025)]
    [InlineData(2048)]
    public void Constructor_WithMaxStackAllocLengthAboveSoftLimit_AndDebugLogsEnabled_WritesWarning(int value)
    {
        // Arrange
        StringEnricherSettings.EnableDebugLogs = true;
        var originalOut = Console.Out;
        using var writer = new StringWriter();
        Console.SetOut(writer);

        // Act
        _ = new BufferAllocationThresholds("TestConfig", value, 10_000_000);

        // Assert
        var output = writer.ToString();
        Assert.Contains("WARNING", output);
        Assert.Contains("MaxStackAllocLength", output);
        Assert.Contains($"high value ({value})", output);

        // Cleanup
        Console.SetOut(originalOut);
        StringEnricherSettings.EnableDebugLogs = false;
    }

    #endregion

    #region MaxPooledArrayLength Tests

    [Theory]
    [InlineData(-1)]
    [InlineData(-100)]
    public void Constructor_WithNegativeMaxPooledArrayLength_ThrowsArgumentOutOfRangeException(int value)
    {
        // Arrange, Act & Assert
        var exception = Assert.Throws<ArgumentOutOfRangeException>(() =>
            new BufferAllocationThresholds("TestConfig", 512, value));
        Assert.Contains("cannot be greater than", exception.Message);
    }

    [Fact]
    public void Constructor_WithMaxPooledArrayLengthZero_Succeeds()
    {
        // Arrange & Act
        var thresholds = new BufferAllocationThresholds("TestConfig", 0, 0);

        // Assert
        Assert.Equal(0, thresholds.MaxPooledArrayLength);
    }

    [Theory]
    [InlineData(10_485_761)]
    [InlineData(20_000_000)]
    public void Constructor_WithMaxPooledArrayLengthAboveHardLimit_ThrowsArgumentOutOfRangeException(int value)
    {
        // Arrange, Act & Assert
        var exception = Assert.Throws<ArgumentOutOfRangeException>(() =>
            new BufferAllocationThresholds("TestConfig", 512, value));
        Assert.Contains("cannot be greater than 10485760", exception.Message);
    }

    [Fact]
    public void Constructor_WithMaxPooledArrayLengthLessThanMaxStack_ThrowsArgumentOutOfRangeException()
    {
        // Arrange, Act & Assert
        var exception = Assert.Throws<ArgumentOutOfRangeException>(() =>
            new BufferAllocationThresholds("TestConfig", 1000, 500));
        Assert.Contains("cannot be greater than", exception.Message);
    }

    [Theory]
    [InlineData(100_000)]
    [InlineData(1_000_000)]
    [InlineData(5_000_000)]
    public void MaxPooledArrayLength_Setter_WithValidValue_UpdatesValue(int value)
    {
        // Arrange
        StringEnricherSettings.EnableDebugLogs = false;
        var thresholds = new BufferAllocationThresholds("TestConfig", 512, 1_000_000);

        // Act
        thresholds.MaxPooledArrayLength = value;

        // Assert
        Assert.Equal(value, thresholds.MaxPooledArrayLength);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(-100)]
    public void MaxPooledArrayLength_Setter_WithNegativeValue_ThrowsArgumentOutOfRangeException(int value)
    {
        // Arrange
        var thresholds = new BufferAllocationThresholds("TestConfig", 512, 1_000_000);

        // Act & Assert
        var exception = Assert.Throws<ArgumentOutOfRangeException>(() =>
            thresholds.MaxPooledArrayLength = value);
        Assert.Contains("must be positive", exception.Message);
    }

    [Theory]
    [InlineData(10_485_761)]
    [InlineData(20_000_000)]
    public void MaxPooledArrayLength_Setter_WithValueAboveHardLimit_ThrowsArgumentOutOfRangeException(int value)
    {
        // Arrange
        var thresholds = new BufferAllocationThresholds("TestConfig", 512, 1_000_000);

        // Act & Assert
        var exception = Assert.Throws<ArgumentOutOfRangeException>(() =>
            thresholds.MaxPooledArrayLength = value);
        Assert.Contains("cannot be greater than 10485760", exception.Message);
    }

    [Fact]
    public void MaxPooledArrayLength_Setter_WithValueLessThanMaxStack_ThrowsArgumentOutOfRangeException()
    {
        // Arrange
        var thresholds = new BufferAllocationThresholds("TestConfig", 1000, 10_000_000);

        // Act & Assert
        var exception = Assert.Throws<ArgumentOutOfRangeException>(() =>
            thresholds.MaxPooledArrayLength = 500);
        Assert.Contains("cannot be less than MaxStackAllocLength", exception.Message);
    }

    [Fact]
    public void MaxPooledArrayLength_Setter_WithDebugLogsEnabled_WritesToConsole()
    {
        // Arrange
        StringEnricherSettings.EnableDebugLogs = true;
        var thresholds = new BufferAllocationThresholds("TestConfig", 512, 1_000_000);
        var originalOut = Console.Out;
        using var writer = new StringWriter();
        Console.SetOut(writer);

        // Act
        thresholds.MaxPooledArrayLength = 2_000_000;

        // Assert
        var output = writer.ToString();
        Assert.Contains("[TestConfig].MaxPooledArrayLength set to 2000000", output);

        // Cleanup
        Console.SetOut(originalOut);
        StringEnricherSettings.EnableDebugLogs = false;
    }

    [Theory]
    [InlineData(100_000)]
    [InlineData(499_999)]
    public void Constructor_WithMaxPooledArrayLengthBelowSoftLimit_AndDebugLogsEnabled_WritesWarning(int value)
    {
        // Arrange
        StringEnricherSettings.EnableDebugLogs = true;
        var originalOut = Console.Out;
        using var writer = new StringWriter();
        Console.SetOut(writer);

        // Act
        _ = new BufferAllocationThresholds("TestConfig", 512, value);

        // Assert
        var output = writer.ToString();
        Assert.Contains("WARNING", output);
        Assert.Contains("MaxPooledArrayLength", output);
        Assert.Contains($"very low value ({value})", output);

        // Cleanup
        Console.SetOut(originalOut);
        StringEnricherSettings.EnableDebugLogs = false;
    }

    [Theory]
    [InlineData(5_242_881)]
    [InlineData(10_000_000)]
    public void Constructor_WithMaxPooledArrayLengthAboveSoftLimit_AndDebugLogsEnabled_WritesWarning(int value)
    {
        // Arrange
        StringEnricherSettings.EnableDebugLogs = true;
        var originalOut = Console.Out;
        using var writer = new StringWriter();
        Console.SetOut(writer);

        // Act
        _ = new BufferAllocationThresholds("TestConfig", 512, value);

        // Assert
        var output = writer.ToString();
        Assert.Contains("WARNING", output);
        Assert.Contains("MaxPooledArrayLength", output);
        Assert.Contains($"high value ({value})", output);

        // Cleanup
        Console.SetOut(originalOut);
        StringEnricherSettings.EnableDebugLogs = false;
    }

    #endregion
}