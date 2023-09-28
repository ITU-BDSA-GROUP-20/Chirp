namespace Chirp.Razor.Tests;

public class UnitTest1
{
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