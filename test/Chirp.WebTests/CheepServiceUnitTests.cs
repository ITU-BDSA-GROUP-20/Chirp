using Chirp.Core.Entities;
using Chirp.Core.Repository;
using Chirp.Infrastructure;
using Chirp.Infrastructure.Repository;
using Chirp.Web;
using Moq;
using Test_Utilities;

namespace Chirp.WebTests;

public class CheepServiceUnitTests
{
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
        var author1 = new Author { Id = Guid.NewGuid(), UserName = "Author1", Email = "email1" };
        var author2 = new Author { Id = Guid.NewGuid(), UserName = "Author2", Email = "email2" };
        
        CreateCheep cheep1 = new CreateCheep(author1, "Cheep 1");
        CreateCheep cheep2 = new CreateCheep(author2, "Cheep 2");

        var cheepDtos = new List<CreateCheep>();
        cheepDtos.Add(cheep1);
        cheepDtos.Add(cheep2);
        
        // Add authors to database
        _authorRepository.AddAuthor(author1);
        _authorRepository.AddAuthor(author2);
        
        // Add cheeps to database
        foreach (var cheep in cheepDtos)
        {
            _cheepRepository.AddCreateCheep(cheep);
        }

        // Act
        List<CheepViewModel> result = service.GetCheeps(0).ToList();

        result.Sort((a, b) => String.Compare(a.Author, b.Author, StringComparison.Ordinal));
            
        // Assert
        Assert.Equal(2, result.Count);
        Assert.Equal("Author1", result[0].Author);
        Assert.Equal("Cheep 1", result[0].Message);
        Assert.NotNull(result[0].Timestamp);
    }
    
    
    [Fact]
    public void GetCheepsFromAuthor_ReturnsCheepViewModels()
    {
        // Arrange
        var cheepRepositoryMock = new Mock<ICheepRepository>();
        var authorRepositoryMock = new Mock<IAuthorRepository>();
        var cheepService = new CheepService(cheepRepositoryMock.Object, authorRepositoryMock.Object);
        
        var author1 = new Author { Id = Guid.NewGuid(), UserName = "Author1", Email = "email1" };
        var author2 = new Author { Id = Guid.NewGuid(), UserName = "Author2", Email = "email2" };
        
        var cheeps = new List<Cheep>
        {
            new Cheep
            {
                Author = author1,
                AuthorId = author1.Id,
                Text = "Cheep 1",
                TimeStamp = DateTime.Now
            },
            new Cheep
            {
                Author = author2,
                AuthorId = author2.Id,
                Text = "Cheep 2",
                TimeStamp = DateTime.Now
            }
        };

        authorRepositoryMock.Setup(repo => repo.GetCheepsByAuthor(author1.UserName, It.IsAny<int>())).Returns(cheeps);

        // Act
        ICollection<CheepViewModel> result = cheepService.GetCheepsFromAuthor(author1.UserName, 1);

        CheepViewModel returnedCheep = result.ElementAt(0);
        
        // Assert
        Assert.Equal(2, result.Count);
        Assert.Equal(author1.UserName, returnedCheep.Author);
        Assert.Equal("Cheep 1", returnedCheep.Message);
        Assert.NotNull(returnedCheep.Timestamp);
    }
}