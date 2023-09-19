namespace Chirp.CLI.Tests;

public class UnitTest1
{
    [Fact]
    public void TestCheepToString()
    {
        // Arrange
        Cheep cheep = new Cheep("test", "test", 1695108998);

        // Act
        var output = cheep.ToString();

        // Assert
        Assert.Equal("test @ 19/09/2023 09:36:38 test", output);
    }
}