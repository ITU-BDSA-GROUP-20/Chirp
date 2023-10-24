using Chirp.Core.Entities;

//Co-authored by ChatGPT-4.0

namespace Chirp.CoreTests.Entities;

public class CheepDTOTests
{
    [Fact]
    public void CheepDTO_DefaultConstructor_SetsPropertiesToDefault()
    {
        // Arrange & Act
        var cheep = new CheepDTO();

        // Assert
        Assert.Equal(0, cheep.CheepId);
        Assert.Equal(0, cheep.AuthorId);
        Assert.Null(cheep.AuthorDto);
        Assert.Null(cheep.Text);
        Assert.Equal(default(DateTime), cheep.TimeStamp);
    }

    [Fact]
    public void CheepDTO_SetProperties_PropertiesAreSet()
    {
        // Arrange
        var cheep = new CheepDTO();
        var author = new AuthorDTO { AuthorId = 1, Name = "Test Author" };

        // Act
        cheep.CheepId = 1;
        cheep.AuthorId = 1;
        cheep.AuthorDto = author;
        cheep.Text = "Test Cheep";
        cheep.TimeStamp = new DateTime(2023, 10, 24);

        // Assert
        Assert.Equal(1, cheep.CheepId);
        Assert.Equal(1, cheep.AuthorId);
        Assert.Equal(author, cheep.AuthorDto);
        Assert.Equal("Test Cheep", cheep.Text);
        Assert.Equal(new DateTime(2023, 10, 24), cheep.TimeStamp);
    }
}