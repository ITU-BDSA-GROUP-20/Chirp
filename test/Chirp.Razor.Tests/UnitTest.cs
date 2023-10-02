namespace Chirp.Razor.Tests;

public class UnitTest
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