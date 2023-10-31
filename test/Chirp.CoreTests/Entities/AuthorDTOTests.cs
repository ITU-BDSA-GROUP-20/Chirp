using Chirp.Core.Entities;

//Co-Authored by ChatGPT-4.0

namespace Chirp.CoreTests.Entities;

public class AuthorDTOTests
{
    [Fact]
    public void AuthorDTO_DefaultConstructor_SetsPropertiesToDefault()
    {
        // Arrange & Act
        var author = new AuthorDTO();

        // Assert
        Assert.Equal(0, author.AuthorId);
        Assert.Null(author.Name);
        Assert.Null(author.Email);
        Assert.Null(author.Cheeps);
    }

    [Fact]
    public void AuthorDTO_SetProperties_PropertiesAreSet()
    {
        // Arrange
        var author = new AuthorDTO();
        var cheeps = new List<CheepDTO>();

        // Act
        author.AuthorId = 1;
        author.Name = "Test Author";
        author.Email = "test@email.com";
        author.Cheeps = cheeps;

        // Assert
        Assert.Equal(1, author.AuthorId);
        Assert.Equal("Test Author", author.Name);
        Assert.Equal("test@email.com", author.Email);
        Assert.Equal(cheeps, author.Cheeps);
    }
}