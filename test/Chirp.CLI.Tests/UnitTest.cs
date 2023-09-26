using System.Diagnostics;
using Xunit.Abstractions;

namespace Chirp.CLI.Tests;
using Chirp.CLI;
using Xunit;

public class UnitTest
{
    private readonly ITestOutputHelper _testOutputHelper;

    public UnitTest(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    public void InitDatabase()
    {
        // clear database
        // add test data
    }

    [Fact]
    public void TestCheepToString()
    {
        // Arrange
        var cheep = new Cheep("test", "test", 1695108998);

        // Act
        var output = cheep.ToString();

        // Assert
        Assert.Equal("test" + " @ " +
                     DateTimeOffset.FromUnixTimeSeconds(1695108998).LocalDateTime + " "
                     + "test", output);
    }
}