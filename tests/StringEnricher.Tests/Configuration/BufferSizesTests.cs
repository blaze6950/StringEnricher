using StringEnricher.Configuration;

namespace StringEnricher.Tests.Configuration;

public class BufferSizesTests
{
    [Fact]
    public void Constructor_WithNameOnly_InitializesWithDefaultValues()
    {
        // Arrange & Act
        var bufferSizes = new BufferSizes("TestConfig");

        // Assert
        Assert.Equal(1, bufferSizes.InitialBufferLength);
        Assert.Equal(1_000_000, bufferSizes.MaxBufferLength);
        Assert.Equal(2.0f, bufferSizes.GrowthFactor);
    }

    [Theory]
    [InlineData(1, 1)]
    [InlineData(100, 1000)]
    [InlineData(2048, 2048)]
    public void Constructor_WithInitialAndMaxBufferLength_InitializesCorrectly(int initial, int max)
    {
        // Arrange & Act
        var bufferSizes = new BufferSizes("TestConfig", initial, max);

        // Assert
        Assert.Equal(initial, bufferSizes.InitialBufferLength);
        Assert.Equal(max, bufferSizes.MaxBufferLength);
    }

    [Theory]
    [InlineData(100, 1000, 1.5f)]
    [InlineData(50, 500, 3.0f)]
    public void Constructor_WithAllParameters_InitializesCorrectly(int initial, int max, float growth)
    {
        // Arrange & Act
        var bufferSizes = new BufferSizes("TestConfig", initial, max, growth);

        // Assert
        Assert.Equal(initial, bufferSizes.InitialBufferLength);
        Assert.Equal(max, bufferSizes.MaxBufferLength);
        Assert.Equal(growth, bufferSizes.GrowthFactor);
    }

    #region InitialBufferLength Tests

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-100)]
    public void Constructor_WithInvalidInitialBufferLength_ThrowsArgumentOutOfRangeException(int initial)
    {
        // Arrange, Act & Assert
        var exception = Assert.Throws<ArgumentOutOfRangeException>(() =>
            new BufferSizes("TestConfig", initial, 1000));
        Assert.Contains("must be greater than zero", exception.Message);
    }

    [Theory]
    [InlineData(2049)]
    [InlineData(3000)]
    public void Constructor_WithInitialBufferLengthAboveHardLimit_ThrowsArgumentOutOfRangeException(int initial)
    {
        // Arrange, Act & Assert
        var exception = Assert.Throws<ArgumentOutOfRangeException>(() =>
            new BufferSizes("TestConfig", initial, initial + 1000));
        Assert.Contains("cannot be greater than 2048", exception.Message);
    }

    [Fact]
    public void Constructor_WithInitialBufferLengthGreaterThanMax_ThrowsArgumentOutOfRangeException()
    {
        // Arrange, Act & Assert
        var exception = Assert.Throws<ArgumentOutOfRangeException>(() =>
            new BufferSizes("TestConfig", 1000, 500));
        Assert.Contains("cannot be greater than MaxBufferLength", exception.Message);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(512)]
    [InlineData(2048)]
    public void InitialBufferLength_Setter_WithValidValue_UpdatesValue(int value)
    {
        // Arrange
        StringEnricherSettings.EnableDebugLogs = false;
        var bufferSizes = new BufferSizes("TestConfig", 1, 10000);

        // Act
        bufferSizes.InitialBufferLength = value;

        // Assert
        Assert.Equal(value, bufferSizes.InitialBufferLength);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void InitialBufferLength_Setter_WithZeroOrNegative_ThrowsArgumentOutOfRangeException(int value)
    {
        // Arrange
        var bufferSizes = new BufferSizes("TestConfig", 1, 1000);

        // Act & Assert
        var exception = Assert.Throws<ArgumentOutOfRangeException>(() =>
            bufferSizes.InitialBufferLength = value);
        Assert.Contains("must be greater than zero", exception.Message);
    }

    [Theory]
    [InlineData(2049)]
    [InlineData(5000)]
    public void InitialBufferLength_Setter_WithValueAboveHardLimit_ThrowsArgumentOutOfRangeException(int value)
    {
        // Arrange
        var bufferSizes = new BufferSizes("TestConfig", 1, 10000);

        // Act & Assert
        var exception = Assert.Throws<ArgumentOutOfRangeException>(() =>
            bufferSizes.InitialBufferLength = value);
        Assert.Contains("cannot be greater than 2048", exception.Message);
    }

    [Fact]
    public void InitialBufferLength_Setter_WithValueGreaterThanMax_ThrowsArgumentOutOfRangeException()
    {
        // Arrange
        var bufferSizes = new BufferSizes("TestConfig", 1, 100);

        // Act & Assert
        var exception = Assert.Throws<ArgumentOutOfRangeException>(() =>
            bufferSizes.InitialBufferLength = 200);
        Assert.Contains("cannot be greater than MaxBufferLength", exception.Message);
    }

    #endregion

    #region MaxBufferLength Tests

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-100)]
    public void Constructor_WithInvalidMaxBufferLength_ThrowsArgumentOutOfRangeException(int max)
    {
        // Arrange, Act & Assert
        var exception = Assert.Throws<ArgumentOutOfRangeException>(() =>
            new BufferSizes("TestConfig", 1, max));
        Assert.Contains("cannot be greater than", exception.Message);
    }

    [Theory]
    [InlineData(100)]
    [InlineData(1000)]
    [InlineData(1_000_000)]
    public void MaxBufferLength_Setter_WithValidValue_UpdatesValue(int value)
    {
        // Arrange
        StringEnricherSettings.EnableDebugLogs = false;
        var bufferSizes = new BufferSizes("TestConfig", 1, 1000);

        // Act
        bufferSizes.MaxBufferLength = value;

        // Assert
        Assert.Equal(value, bufferSizes.MaxBufferLength);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void MaxBufferLength_Setter_WithZeroOrNegative_ThrowsArgumentOutOfRangeException(int value)
    {
        // Arrange
        var bufferSizes = new BufferSizes("TestConfig", 1, 1000);

        // Act & Assert
        var exception = Assert.Throws<ArgumentOutOfRangeException>(() =>
            bufferSizes.MaxBufferLength = value);
        Assert.Contains("must be greater than zero", exception.Message);
    }

    [Fact]
    public void MaxBufferLength_Setter_WithValueLessThanInitial_ThrowsArgumentOutOfRangeException()
    {
        // Arrange
        var bufferSizes = new BufferSizes("TestConfig", 100, 1000);

        // Act & Assert
        var exception = Assert.Throws<ArgumentOutOfRangeException>(() =>
            bufferSizes.MaxBufferLength = 50);
        Assert.Contains("cannot be less than InitialBufferLength", exception.Message);
    }

    #endregion

    #region GrowthFactor Tests

    [Theory]
    [InlineData(1.5f)]
    [InlineData(2.0f)]
    [InlineData(5.0f)]
    public void GrowthFactor_Setter_WithValidValue_UpdatesValue(float value)
    {
        // Arrange
        StringEnricherSettings.EnableDebugLogs = false;
        var bufferSizes = new BufferSizes("TestConfig", 10, 1000);

        // Act
        bufferSizes.GrowthFactor = value;

        // Assert
        Assert.Equal(value, bufferSizes.GrowthFactor);
    }

    [Theory]
    [InlineData(0.5f)]
    [InlineData(1.0f)]
    [InlineData(0.0f)]
    [InlineData(-1.0f)]
    public void Constructor_WithGrowthFactorLessThanOrEqualToOne_ThrowsArgumentOutOfRangeException(float growth)
    {
        // Arrange, Act & Assert
        var exception = Assert.Throws<ArgumentOutOfRangeException>(() =>
            new BufferSizes("TestConfig", 100, 1000, growth));
        Assert.Contains("must be greater than 1.0", exception.Message);
    }

    [Theory]
    [InlineData(1.0f)]
    [InlineData(0.5f)]
    public void GrowthFactor_Setter_WithValueLessThanOrEqualToOne_ThrowsArgumentOutOfRangeException(float value)
    {
        // Arrange
        var bufferSizes = new BufferSizes("TestConfig", 100, 1000);

        // Act & Assert
        var exception = Assert.Throws<ArgumentOutOfRangeException>(() =>
            bufferSizes.GrowthFactor = value);
        Assert.Contains("must be greater than 1.0", exception.Message);
    }

    [Fact]
    public void Constructor_WithGrowthFactorTooSmallToCauseIncrease_ThrowsArgumentOutOfRangeException()
    {
        // Arrange
        // With initialBufferLength = 1000, growthFactor must be > 1.001 to cause increase
        float tooSmallGrowth = 1.0001f;

        // Act & Assert
        var exception = Assert.Throws<ArgumentOutOfRangeException>(() =>
            new BufferSizes("TestConfig", 1000, 10000, tooSmallGrowth));
        Assert.Contains("too small to cause any increase", exception.Message);
    }

    [Fact]
    public void GrowthFactor_Setter_WithValueTooSmallToCauseIncrease_ThrowsArgumentOutOfRangeException()
    {
        // Arrange
        var bufferSizes = new BufferSizes("TestConfig", 1000, 10000);
        float tooSmallGrowth = 1.0001f;

        // Act & Assert
        var exception = Assert.Throws<ArgumentOutOfRangeException>(() =>
            bufferSizes.GrowthFactor = tooSmallGrowth);
        Assert.Contains("too small to cause any increase", exception.Message);
    }

    #endregion

    #region Debug Logging Tests

    [Fact]
    public void InitialBufferLength_Setter_WithDebugLogsEnabled_WritesToConsole()
    {
        // Arrange
        StringEnricherSettings.EnableDebugLogs = true;
        var bufferSizes = new BufferSizes("TestConfig", 1, 1000);
        var originalOut = Console.Out;
        using var writer = new StringWriter();
        Console.SetOut(writer);

        // Act
        bufferSizes.InitialBufferLength = 100;

        // Assert
        var output = writer.ToString();
        Assert.Contains("[TestConfig].InitialBufferLength set to 100", output);

        // Cleanup
        Console.SetOut(originalOut);
        StringEnricherSettings.EnableDebugLogs = false;
    }

    [Fact]
    public void MaxBufferLength_Setter_WithDebugLogsEnabled_WritesToConsole()
    {
        // Arrange
        StringEnricherSettings.EnableDebugLogs = true;
        var bufferSizes = new BufferSizes("TestConfig", 1, 1000);
        var originalOut = Console.Out;
        using var writer = new StringWriter();
        Console.SetOut(writer);

        // Act
        bufferSizes.MaxBufferLength = 2000;

        // Assert
        var output = writer.ToString();
        Assert.Contains("[TestConfig].MaxBufferLength set to 2000", output);

        // Cleanup
        Console.SetOut(originalOut);
        StringEnricherSettings.EnableDebugLogs = false;
    }

    [Fact]
    public void GrowthFactor_Setter_WithDebugLogsEnabled_WritesToConsole()
    {
        // Arrange
        StringEnricherSettings.EnableDebugLogs = true;
        var bufferSizes = new BufferSizes("TestConfig", 100, 1000);
        var originalOut = Console.Out;
        using var writer = new StringWriter();
        Console.SetOut(writer);

        // Act
        bufferSizes.GrowthFactor = 3.0f;

        // Assert
        var output = writer.ToString();
        Assert.Contains("[TestConfig].GrowthFactor set to 3", output);

        // Cleanup
        Console.SetOut(originalOut);
        StringEnricherSettings.EnableDebugLogs = false;
    }

    [Fact]
    public void Constructor_WithHighInitialBufferLength_AndDebugLogsEnabled_WritesWarning()
    {
        // Arrange
        StringEnricherSettings.EnableDebugLogs = true;
        var originalOut = Console.Out;
        using var writer = new StringWriter();
        Console.SetOut(writer);

        // Act
        _ = new BufferSizes("TestConfig", 1025, 10000);

        // Assert
        var output = writer.ToString();
        Assert.Contains("WARNING", output);
        Assert.Contains("InitialBufferLength", output);
        Assert.Contains("1025", output);

        // Cleanup
        Console.SetOut(originalOut);
        StringEnricherSettings.EnableDebugLogs = false;
    }

    [Fact]
    public void Constructor_WithHighMaxBufferLength_AndDebugLogsEnabled_WritesWarning()
    {
        // Arrange
        StringEnricherSettings.EnableDebugLogs = true;
        var originalOut = Console.Out;
        using var writer = new StringWriter();
        Console.SetOut(writer);

        // Act
        _ = new BufferSizes("TestConfig", 1, 2000);

        // Assert
        var output = writer.ToString();
        Assert.Contains("WARNING", output);
        Assert.Contains("MaxBufferLength", output);
        Assert.Contains("2000", output);

        // Cleanup
        Console.SetOut(originalOut);
        StringEnricherSettings.EnableDebugLogs = false;
    }

    [Fact]
    public void Constructor_WithHighGrowthFactor_AndDebugLogsEnabled_WritesWarning()
    {
        // Arrange
        StringEnricherSettings.EnableDebugLogs = true;
        var originalOut = Console.Out;
        using var writer = new StringWriter();
        Console.SetOut(writer);

        // Act
        _ = new BufferSizes("TestConfig", 100, 10000, 11.0f);

        // Assert
        var output = writer.ToString();
        Assert.Contains("WARNING", output);
        Assert.Contains("GrowthFactor", output);
        Assert.Contains("11", output);

        // Cleanup
        Console.SetOut(originalOut);
        StringEnricherSettings.EnableDebugLogs = false;
    }

    #endregion
}