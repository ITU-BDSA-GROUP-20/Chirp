namespace Chirp.Razor.Tests;

public class UnitTest
{
    [Fact]
    public void GetCheepsTest()
    {
        // Arrange

        // Act
        List<CheepViewModel> cheeps = new CheepService().GetCheeps(2);
        
        // Assert
        Assert.Equal(2, cheeps.Count);
        Assert.Equal("Helge", cheeps[0].Author);
        Assert.Equal("Rasmus", cheeps[1].Author);
    }
    
    [Fact]
    public void GetCheepsFromAuthorTest()
    {
        // Arrange

        // Act
        List<CheepViewModel> cheeps = new CheepService().GetCheepsFromAuthor("Helge", 1);
        
        // Assert
        Assert.Equal("Helge", cheeps[0].Author);
        Assert.Equal("Hello, BDSA students!", cheeps[0].Message);
    }
   
}