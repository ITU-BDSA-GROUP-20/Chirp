namespace Chirp.Razor.Tests;

public class UnitTest1


{
    [Fact]
    public void Test1()
    {
        // Arrange

        // Act
        List<CheepViewModel> cheeps = new CheepService().GetCheeps();
        
        // Assert
        Assert.Equal(2, cheeps.Count);
        Assert.Equal("Helge", cheeps[0].Author);
    }
   
}