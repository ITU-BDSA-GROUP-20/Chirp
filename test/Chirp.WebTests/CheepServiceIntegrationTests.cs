using Chirp.Core.Entities;
using Chirp.Core.Repository;
using Chirp.Infrastructure;
using Chirp.Infrastructure.Repository;
using Chirp.Web;
using Chirp.Web.Models;
using Moq;
using Test_Utilities;

namespace Chirp.WebTests;

public class CheepServiceIntegrationTests
{
    private CheepService _service;

    private Author _author1;
    private Author _author2;
    private Cheep _cheep1;
    private Cheep _cheep2;
    private Cheep _cheep3;

    public CheepServiceIntegrationTests()
    {
        ChirpDbContext context = SqliteInMemoryBuilder.GetContext();
        IFollowRepository followRepository = new FollowRepository(context);
        ICheepRepository cheepRepository = new CheepRepository(context);
        IAuthorRepository authorRepository = new AuthorRepository(context);
        IReactionRepository reactionRepository = new ReactionRepository(context);
        _service = new CheepService(cheepRepository, authorRepository, reactionRepository);

        _author1 = new Author { Id = Guid.NewGuid(), UserName = "Author1", Email = "email1" };
        _author2 = new Author { Id = Guid.NewGuid(), UserName = "Author2", Email = "email2" };

        _cheep1 = new Cheep
        {
            Author = _author1,
            AuthorId = _author1.Id,
            Text = "Cheep 1",
            TimeStamp = DateTime.Now,
            Reactions = new List<Reaction>(),
        };
        
        _cheep2 = new Cheep
        {
            Author = _author2,
            AuthorId = _author2.Id,
            Text = "Cheep 2",
            TimeStamp = DateTime.Now,
            Reactions = new List<Reaction>()
        };

        _cheep3 = new Cheep()
        {
            Author = _author2,
            AuthorId = _author2.Id,
            Text = "Cheep 3",
            TimeStamp = DateTime.Now,
            Reactions = new List<Reaction>()
        };

        followRepository.CreateFollow(_author1, _author2);

        context.Add(_author1);
        context.Add(_author2);
        context.Add(_cheep1);
        context.Add(_cheep2);
        context.Add(_cheep3);

        context.SaveChanges();
    }

    [Fact]
        public void GetCheeps_ReturnsCheepViewModels()
        {
            // Act
            List<CheepViewModel> result = _service.GetCheeps(0).ToList();

            result.Sort((a, b) => String.Compare(a.User.Username, b.User.Username, StringComparison.Ordinal));
            
            // Assert
            Assert.Equal(3, result.Count);
            Assert.Equal("Author1", result[0].User.Username);
            Assert.Equal("Cheep 1", result[0].Message);
            Assert.NotNull(result[0].Timestamp);
        }
    
    
        [Fact]
        public void GetCheepsFromAuthor_ReturnsCheepViewModels()
        {
            // Act
            ICollection<CheepViewModel> result = _service.GetCheepsFromAuthor(_author2.Id, 1);

            CheepViewModel returnedCheep = result.ElementAt(0);
        
            // Assert
            Assert.Equal(2, result.Count);
            Assert.Equal(_author2.UserName, returnedCheep.User.Username);
            Assert.Equal("Cheep 3", returnedCheep.Message);
            Assert.NotNull(returnedCheep.Timestamp);
        }
    }