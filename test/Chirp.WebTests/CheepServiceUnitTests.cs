using Chirp.Core.Entities;
using Chirp.Core.Repository;
using Chirp.Infrastructure;
using Chirp.Infrastructure.Repository;
using Chirp.Web;
using Microsoft.EntityFrameworkCore;
using Test_Utilities;

namespace Chirp.WebTests.CheepServiceTests;

public class CheepServiceUnitTests
{
    /*
    private readonly ChirpDbContext context;
    private ICheepRepository _cheepRepository;
    private IAuthorRepository _authorRepository;

    public CheepServiceUnitTests()
    {
        context = SqliteInMemoryBuilder.GetContext();
    }
    
    [Fact]
    public void GetCheeps_ReturnsCheepViewModels()
    {
        // Arrange
        _cheepRepository = new CheepRepository(context);
        _authorRepository = new AuthorRepository(context);
        ICheepService service = new CheepService(_cheepRepository, _authorRepository);
        
        // Mock data
        var author1 = new AuthorDTO { AuthorId = new Guid(), Name = "Author1", Email = "email1" };
        var author2 = new AuthorDTO { AuthorId = new Guid(), Name = "Author2", Email = "email2" };
        
        var cheepDtos = new List<CheepDTO>
        {
            new CheepDTO
            {
                AuthorDto = author1 ,
                Text = "Cheep 1",
                TimeStamp = DateTime.Now
            },
            new CheepDTO
            {
                AuthorDto = author2,
                Text = "Cheep 2",
                TimeStamp = DateTime.Now
            }
        };
        
        // Add authors to database
        _authorRepository.AddAuthor(author1);
        _authorRepository.AddAuthor(author2);
        
        // Add cheeps to database
        foreach (var cheep in cheepDtos)
        {
            _cheepRepository.AddCheep(cheep);
        }

        // Act
        List<CheepViewModel> result = service.GetCheeps(0).ToList();

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Equal("Author1", result[0].Author);
        Assert.Equal("Cheep 1", result[0].Message);
        Assert.NotNull(result[0].Timestamp);
    }
    
    /*
    [Fact]
    public void GetCheepsFromAuthor_ReturnsCheepViewModels()
    {
        // Arrange
        var cheepRepositoryMock = new Mock<ICheepRepository>();
        var authorRepositoryMock = new Mock<IAuthorRepository>();
        var cheepService = new CheepService(cheepRepositoryMock.Object, authorRepositoryMock.Object);

        // Mock data
        var authorName = "Author1";
        var cheepDtos = new List<CheepDTO>
        {
            new CheepDTO
            {
                AuthorDto = new AuthorDTO { Name = authorName },
                Text = "Cheep 1",
                TimeStamp = DateTime.Now
            },
            new CheepDTO
            {
                AuthorDto = new AuthorDTO { Name = authorName },
                Text = "Cheep 2",
                TimeStamp = DateTime.Now
            }
        };

        authorRepositoryMock.Setup(repo => repo.GetCheepsByAuthor(authorName, It.IsAny<int>())).Returns(cheepDtos);

        // Act
        var result = cheepService.GetCheepsFromAuthor(authorName, 1);

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Equal(authorName, result[0].Author);
        Assert.Equal("Cheep 1", result[0].Message);
        Assert.NotNull(result[0].Timestamp);
    }
    */
}