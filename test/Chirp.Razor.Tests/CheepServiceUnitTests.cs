namespace Chirp.Razor.Tests;

public class CheepServiceUnitTests
{
    [Fact]
    public void GetCheepsTest()
    {
        // Arrange

        // Act
        List<CheepViewModel> cheeps = new CheepService().GetCheeps(1);
        
        // Assert
        Assert.Equal(32, cheeps.Count);
    }

    [Fact]
    public void GetCheepsContainsAnExpectedCheep()
    {
        List<CheepViewModel> cheeps = new CheepService().GetCheeps(1);
        CheepViewModel cheep = cheeps[0];
        
        Assert.Equal(cheep.Author, "Quintin Sitts");
        Assert.Equal(cheep.Message, "For then, more whales the less to her, as you very much.");
        Assert.Equal(cheep.Timestamp, "2023-08-01 13:14:07");
    }
    
    [Fact]
    public void GetCheepsFromAuthorTest()
    {
        
        //TODO Fix test
        /*
        // Arrange
        CheepService cheepService = new CheepService();

        // Act
        List<CheepViewModel> cheeps = cheepService.GetCheepsFromAuthor("Helge", 1);
        
        // Assert
        Assert.Equal("Helge", cheeps[0].Author);
        Assert.Equal("Hello, BDSA students!", cheeps[0].Message);*/
    }
}