using StringEnricher.Configuration;

namespace StringEnricher.Tests.Configuration;

/// <summary>
/// Collection definition to disable parallelization for tests that modify static settings.
/// This ensures that tests do not interfere with each other by modifying shared state.
/// </summary>
[CollectionDefinition("NonParallelTests", DisableParallelization = true)]
public class NonParallelTestsCollection;

/// <summary>
/// Unit tests for StringEnricherSettings class.
/// </summary>
[Collection("NonParallelTests")]
public class StringEnricherSettingsTests
{
    /// <summary>
    /// Resets the settings to their default state before each test.
    /// This ensures test isolation by unsealing settings and restoring defaults.
    /// </summary>
    private static void ResetSettings()
    {
        // Use reflection to reset the sealed state for testing
        var sealedField = typeof(StringEnricherSettings).GetField("_isSealed",
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
        sealedField?.SetValue(null, false);

        // Reset to default values
        StringEnricherSettings.EnableDebugLogs = true;
        StringEnricherSettings.Extensions.StringBuilder.MaxStackAllocLength = 512;
        StringEnricherSettings.Extensions.StringBuilder.MaxPooledArrayLength = 1_000_000;
    }

    [Collection("NonParallelTests")]

    public class SealingTests
    {
        public SealingTests()
        {
            ResetSettings();
        }

        [Fact]
        public void Seal_ShouldPreventFurtherModifications()
        {
            // Arrange & Act
            StringEnricherSettings.Seal();

            // Assert
            var exception = Record.Exception(() =>
                StringEnricherSettings.EnableDebugLogs = false);
            Assert.Null(exception);

            Assert.Throws<InvalidOperationException>(() =>
                StringEnricherSettings.Extensions.StringBuilder.MaxStackAllocLength = 256);

            Assert.Throws<InvalidOperationException>(() =>
                StringEnricherSettings.Extensions.StringBuilder.MaxPooledArrayLength = 500_000);
        }

        [Fact]
        public void Seal_ExceptionMessage_ShouldBeDescriptive()
        {
            // Arrange
            StringEnricherSettings.Seal();

            // Act & Assert
            var exception = Assert.Throws<InvalidOperationException>(() =>
                StringEnricherSettings.Extensions.StringBuilder.MaxStackAllocLength = 256);

            Assert.Equal("StringEnricherSettings is sealed and cannot be modified.", exception.Message);
        }

        [Fact]
        public void Settings_WhenNotSealed_ShouldAllowModifications()
        {
            // Arrange
            // Settings are not sealed by default

            // Act & Assert - Should not throw
            StringEnricherSettings.EnableDebugLogs = false;
            StringEnricherSettings.Extensions.StringBuilder.MaxStackAllocLength = 256;
            StringEnricherSettings.Extensions.StringBuilder.MaxPooledArrayLength = 500_000;

            Assert.False(StringEnricherSettings.EnableDebugLogs);
            Assert.Equal(256, StringEnricherSettings.Extensions.StringBuilder.MaxStackAllocLength);
            Assert.Equal(500_000, StringEnricherSettings.Extensions.StringBuilder.MaxPooledArrayLength);
        }
    }

    [Collection("NonParallelTests")]

    public class EnableDebugLogsTests
    {
        public EnableDebugLogsTests()
        {
            ResetSettings();
        }

        [Fact]
        public void EnableDebugLogs_DefaultValue_ShouldBeTrue()
        {
            // Assert
            Assert.True(StringEnricherSettings.EnableDebugLogs);
        }

        [Fact]
        public void EnableDebugLogs_CanBeSetToFalse()
        {
            // Act
            StringEnricherSettings.EnableDebugLogs = false;

            // Assert
            Assert.False(StringEnricherSettings.EnableDebugLogs);
        }

        [Fact]
        public void EnableDebugLogs_CanBeSetToTrue()
        {
            // Arrange
            StringEnricherSettings.EnableDebugLogs = false;

            // Act
            StringEnricherSettings.EnableDebugLogs = true;

            // Assert
            Assert.True(StringEnricherSettings.EnableDebugLogs);
        }
    }

    [Collection("NonParallelTests")]

    public class MaxStackAllocLengthTests
    {
        public MaxStackAllocLengthTests()
        {
            ResetSettings();
        }

        [Fact]
        public void MaxStackAllocLength_DefaultValue_ShouldBe512()
        {
            // Assert
            Assert.Equal(512, StringEnricherSettings.Extensions.StringBuilder.MaxStackAllocLength);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(32)]
        [InlineData(128)]
        [InlineData(512)]
        [InlineData(1024)]
        [InlineData(2048)]
        public void MaxStackAllocLength_ValidValues_ShouldBeAccepted(int value)
        {
            // Act
            StringEnricherSettings.Extensions.StringBuilder.MaxStackAllocLength = value;

            // Assert
            Assert.Equal(value, StringEnricherSettings.Extensions.StringBuilder.MaxStackAllocLength);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-100)]
        public void MaxStackAllocLength_ZeroOrNegativeValues_ShouldThrowArgumentOutOfRangeException(int value)
        {
            // Act & Assert
            var exception = Assert.Throws<ArgumentOutOfRangeException>(() =>
                StringEnricherSettings.Extensions.StringBuilder.MaxStackAllocLength = value);

            Assert.Equal("value", exception.ParamName);
            Assert.Contains("must be greater than zero", exception.Message);
        }

        [Theory]
        [InlineData(2049)]
        [InlineData(5000)]
        [InlineData(10000)]
        public void MaxStackAllocLength_ValuesAboveHardLimit_ShouldThrowArgumentOutOfRangeException(int value)
        {
            // Act & Assert
            var exception = Assert.Throws<ArgumentOutOfRangeException>(() =>
                StringEnricherSettings.Extensions.StringBuilder.MaxStackAllocLength = value);

            Assert.Equal("value", exception.ParamName);
        }

        [Fact]
        public void MaxStackAllocLength_ValueGreaterThanMaxPooledArrayLength_ShouldThrowArgumentOutOfRangeException()
        {
            // Arrange
            StringEnricherSettings.Extensions.StringBuilder.MaxPooledArrayLength = 1000;
            int invalidValue = 1500;

            // Act & Assert
            var exception = Assert.Throws<ArgumentOutOfRangeException>(() =>
                StringEnricherSettings.Extensions.StringBuilder.MaxStackAllocLength = invalidValue);

            Assert.Equal("value", exception.ParamName);
            Assert.Contains("cannot be greater than MaxPooledArrayLength", exception.Message);
        }

        [Fact]
        public void MaxStackAllocLength_WhenSealed_ShouldThrowInvalidOperationException()
        {
            // Arrange
            StringEnricherSettings.Seal();

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() =>
                StringEnricherSettings.Extensions.StringBuilder.MaxStackAllocLength = 256);
        }
    }

    [Collection("NonParallelTests")]

    public class MaxPooledArrayLengthTests
    {
        public MaxPooledArrayLengthTests()
        {
            ResetSettings();
        }

        [Fact]
        public void MaxPooledArrayLength_DefaultValue_ShouldBe1Million()
        {
            // Assert
            Assert.Equal(1_000_000, StringEnricherSettings.Extensions.StringBuilder.MaxPooledArrayLength);
        }

        [Theory]
        [InlineData(512)] // Should match default MaxStackAllocLength
        [InlineData(1000)]
        [InlineData(100_000)]
        [InlineData(1_000_000)]
        [InlineData(5_000_000)]
        [InlineData(10_485_760)] // Hard limit
        public void MaxPooledArrayLength_ValidValues_ShouldBeAccepted(int value)
        {
            // Act
            StringEnricherSettings.Extensions.StringBuilder.MaxPooledArrayLength = value;

            // Assert
            Assert.Equal(value, StringEnricherSettings.Extensions.StringBuilder.MaxPooledArrayLength);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-100)]
        public void MaxPooledArrayLength_ZeroOrNegativeValues_ShouldThrowArgumentOutOfRangeException(int value)
        {
            // Act & Assert
            var exception = Assert.Throws<ArgumentOutOfRangeException>(() =>
                StringEnricherSettings.Extensions.StringBuilder.MaxPooledArrayLength = value);

            Assert.Equal("value", exception.ParamName);
            Assert.Contains("must be greater than zero", exception.Message);
        }

        [Theory]
        [InlineData(10_485_761)]
        [InlineData(20_000_000)]
        [InlineData(int.MaxValue)]
        public void MaxPooledArrayLength_ValuesAboveHardLimit_ShouldThrowArgumentOutOfRangeException(int value)
        {
            // Act & Assert
            var exception = Assert.Throws<ArgumentOutOfRangeException>(() =>
                StringEnricherSettings.Extensions.StringBuilder.MaxPooledArrayLength = value);

            Assert.Equal("value", exception.ParamName);
            Assert.Contains("cannot be greater than 10485760", exception.Message);
        }

        [Fact]
        public void MaxPooledArrayLength_ValueLessThanMaxStackAllocLength_ShouldThrowArgumentOutOfRangeException()
        {
            // Arrange
            StringEnricherSettings.Extensions.StringBuilder.MaxStackAllocLength = 1000;
            int invalidValue = 500;

            // Act & Assert
            var exception = Assert.Throws<ArgumentOutOfRangeException>(() =>
                StringEnricherSettings.Extensions.StringBuilder.MaxPooledArrayLength = invalidValue);

            Assert.Equal("value", exception.ParamName);
            Assert.Contains("cannot be less than MaxStackAllocLength", exception.Message);
        }

        [Fact]
        public void MaxPooledArrayLength_WhenSealed_ShouldThrowInvalidOperationException()
        {
            // Arrange
            StringEnricherSettings.Seal();

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() =>
                StringEnricherSettings.Extensions.StringBuilder.MaxPooledArrayLength = 500_000);
        }
    }

    [Collection("NonParallelTests")]

    public class ValidationOrderTests
    {
        public ValidationOrderTests()
        {
            ResetSettings();
        }

        [Fact]
        public void SetMaxStackAllocLength_AfterChangingMaxPooledArrayLength_ShouldWorkCorrectly()
        {
            // Arrange
            StringEnricherSettings.Extensions.StringBuilder.MaxPooledArrayLength = 2000;

            // Act & Assert - Should not throw
            StringEnricherSettings.Extensions.StringBuilder.MaxStackAllocLength = 1500;

            Assert.Equal(1500, StringEnricherSettings.Extensions.StringBuilder.MaxStackAllocLength);
            Assert.Equal(2000, StringEnricherSettings.Extensions.StringBuilder.MaxPooledArrayLength);
        }

        [Fact]
        public void SetMaxPooledArrayLength_AfterChangingMaxStackAllocLength_ShouldWorkCorrectly()
        {
            // Arrange
            StringEnricherSettings.Extensions.StringBuilder.MaxStackAllocLength = 1000;

            // Act & Assert - Should not throw
            StringEnricherSettings.Extensions.StringBuilder.MaxPooledArrayLength = 2000;

            Assert.Equal(1000, StringEnricherSettings.Extensions.StringBuilder.MaxStackAllocLength);
            Assert.Equal(2000, StringEnricherSettings.Extensions.StringBuilder.MaxPooledArrayLength);
        }

        [Fact]
        public void SetBothValues_ToSameValue_ShouldWork()
        {
            // Act & Assert - Should not throw
            StringEnricherSettings.Extensions.StringBuilder.MaxStackAllocLength = 1000;
            StringEnricherSettings.Extensions.StringBuilder.MaxPooledArrayLength = 1000;

            Assert.Equal(1000, StringEnricherSettings.Extensions.StringBuilder.MaxStackAllocLength);
            Assert.Equal(1000, StringEnricherSettings.Extensions.StringBuilder.MaxPooledArrayLength);
        }
    }

    [Collection("NonParallelTests")]

    public class DebugLoggingTests
    {
        public DebugLoggingTests()
        {
            ResetSettings();
        }

        [Fact]
        public void MaxStackAllocLength_WithDebugLogsEnabled_ShouldOutputToConsole()
        {
            // Arrange
            StringEnricherSettings.EnableDebugLogs = true;
            var originalOut = Console.Out;
            var stringWriter = new StringWriter();
            Console.SetOut(stringWriter);

            try
            {
                // Act
                StringEnricherSettings.Extensions.StringBuilder.MaxStackAllocLength = 256;

                // Assert
                var output = stringWriter.ToString();
                Assert.Contains("MaxStackAllocLength", output);
                Assert.Contains("set to 256", output);
            }
            finally
            {
                Console.SetOut(originalOut);
            }
        }

        [Fact]
        public void MaxPooledArrayLength_WithDebugLogsEnabled_ShouldOutputToConsole()
        {
            // Arrange
            StringEnricherSettings.EnableDebugLogs = true;
            var originalOut = Console.Out;
            var stringWriter = new StringWriter();
            Console.SetOut(stringWriter);

            try
            {
                // Act
                StringEnricherSettings.Extensions.StringBuilder.MaxPooledArrayLength = 500_000;

                // Assert
                var output = stringWriter.ToString();
                Assert.Contains("MaxPooledArrayLength", output);
                Assert.Contains("set to 500000", output);
            }
            finally
            {
                Console.SetOut(originalOut);
            }
        }

        [Fact]
        public void Settings_WithDebugLogsDisabled_ShouldNotOutputToConsole()
        {
            // Arrange
            StringEnricherSettings.EnableDebugLogs = false;
            var originalOut = Console.Out;
            var stringWriter = new StringWriter();
            Console.SetOut(stringWriter);

            try
            {
                // Act
                StringEnricherSettings.Extensions.StringBuilder.MaxStackAllocLength = 256;
                StringEnricherSettings.Extensions.StringBuilder.MaxPooledArrayLength = 500_000;

                // Assert
                var output = stringWriter.ToString();
                Assert.Empty(output);
            }
            finally
            {
                Console.SetOut(originalOut);
            }
        }

        [Fact]
        public void MaxStackAllocLength_LowValue_WithDebugLogsEnabled_ShouldShowWarning()
        {
            // Arrange
            StringEnricherSettings.EnableDebugLogs = true;
            var originalOut = Console.Out;
            var stringWriter = new StringWriter();
            Console.SetOut(stringWriter);

            try
            {
                // Act
                StringEnricherSettings.Extensions.StringBuilder.MaxStackAllocLength = 16; // Below soft limit of 32

                // Assert
                var output = stringWriter.ToString();
                Assert.Contains("WARNING", output);
                Assert.Contains("very low value", output);
            }
            finally
            {
                Console.SetOut(originalOut);
            }
        }

        [Fact]
        public void MaxStackAllocLength_HighValue_WithDebugLogsEnabled_ShouldShowWarning()
        {
            // Arrange
            StringEnricherSettings.EnableDebugLogs = true;
            var originalOut = Console.Out;
            var stringWriter = new StringWriter();
            Console.SetOut(stringWriter);

            try
            {
                // Act
                StringEnricherSettings.Extensions.StringBuilder.MaxStackAllocLength = 1500; // Above soft limit of 1024

                // Assert
                var output = stringWriter.ToString();
                Assert.Contains("WARNING", output);
                Assert.Contains("high value", output);
            }
            finally
            {
                Console.SetOut(originalOut);
            }
        }

        [Fact]
        public void MaxPooledArrayLength_LowValue_WithDebugLogsEnabled_ShouldShowWarning()
        {
            // Arrange
            StringEnricherSettings.EnableDebugLogs = true;
            var originalOut = Console.Out;
            var stringWriter = new StringWriter();
            Console.SetOut(stringWriter);

            try
            {
                // Act
                StringEnricherSettings.Extensions.StringBuilder.MaxPooledArrayLength =
                    100_000; // Below soft limit of 500_000

                // Assert
                var output = stringWriter.ToString();
                Assert.Contains("WARNING", output);
                Assert.Contains("very low value", output);
            }
            finally
            {
                Console.SetOut(originalOut);
            }
        }

        [Fact]
        public void MaxPooledArrayLength_HighValue_WithDebugLogsEnabled_ShouldShowWarning()
        {
            // Arrange
            StringEnricherSettings.EnableDebugLogs = true;
            var originalOut = Console.Out;
            var stringWriter = new StringWriter();
            Console.SetOut(stringWriter);

            try
            {
                // Act
                StringEnricherSettings.Extensions.StringBuilder.MaxPooledArrayLength =
                    6_000_000; // Above soft limit of 5_242_880

                // Assert
                var output = stringWriter.ToString();
                Assert.Contains("WARNING", output);
                Assert.Contains("high value", output);
            }
            finally
            {
                Console.SetOut(originalOut);
            }
        }
    }

    [Collection("NonParallelTests")]

    public class EdgeCaseTests
    {
        public EdgeCaseTests()
        {
            ResetSettings();
        }

        [Fact]
        public void Settings_MultipleChanges_ShouldMaintainConsistency()
        {
            // Act
            StringEnricherSettings.Extensions.StringBuilder.MaxStackAllocLength = 100;
            StringEnricherSettings.Extensions.StringBuilder.MaxPooledArrayLength = 200;
            StringEnricherSettings.Extensions.StringBuilder.MaxStackAllocLength = 150;
            StringEnricherSettings.Extensions.StringBuilder.MaxPooledArrayLength = 300;

            // Assert
            Assert.Equal(150, StringEnricherSettings.Extensions.StringBuilder.MaxStackAllocLength);
            Assert.Equal(300, StringEnricherSettings.Extensions.StringBuilder.MaxPooledArrayLength);
        }

        [Fact]
        public void Settings_AtBoundaryValues_ShouldWork()
        {
            // Act & Assert - Should not throw
            StringEnricherSettings.Extensions.StringBuilder.MaxStackAllocLength = 1; // Minimum
            StringEnricherSettings.Extensions.StringBuilder.MaxPooledArrayLength = 1; // Minimum

            StringEnricherSettings.Extensions.StringBuilder.MaxPooledArrayLength = 10_485_760; // Hard limit
            StringEnricherSettings.Extensions.StringBuilder.MaxStackAllocLength = 2048; // Hard limit

            Assert.Equal(2048, StringEnricherSettings.Extensions.StringBuilder.MaxStackAllocLength);
            Assert.Equal(10_485_760, StringEnricherSettings.Extensions.StringBuilder.MaxPooledArrayLength);
        }
    }
}