using Chirp.Core.Entities;
using Chirp.Infrastructure;
using Chirp.Infrastructure.Repository;
using Test_Utilities;

namespace Chirp.InfrastructureTest.RepositoryTests;

public class ReactionRepositoryTest
{
    private ReactionRepository _ReactionRepository;
    private readonly ChirpDbContext db;

    public ReactionRepositoryTest()
    {
        db = SqliteInMemoryBuilder.GetContext();
        
    }
    //public  Task AddReaction(ReactionType reaction, Guid cheepId, Guid authorId);
    [Theory]
    [InlineData(ReactionType.Like)]
    [InlineData(ReactionType.Dislike)]
    [InlineData(ReactionType.Love)]
    public async Task AddReaction_ShouldAddReactionToCheep(ReactionType reactionType)
    {
        //Arrange
        _ReactionRepository = new ReactionRepository(db);


        Author authorDto = new Author
        {
            UserName = "TestAuthor1", 
            Email = "mock1@email.com"
        };
        Author authorDto1 = new Author
        { 
                UserName = "TestAuthor2", 
                Email = "mock2@email.com" 
            };
        Cheep cheepDto = new Cheep
        {
            CheepId = Guid.NewGuid(),
            AuthorId = authorDto1.Id, 
            Text = "TestCheep1", 
            Author = authorDto
        };
            
        db.Users.Add(authorDto);
        db.Cheeps.Add(cheepDto);
        

        db.SaveChanges(); 
        
        //Act
        await _ReactionRepository.AddReaction(
            reactionType,
            cheepDto.CheepId,
            authorDto.Id
        );

        //Assert
        Assert.Equal(1, cheepDto.Reactions.Count);
        Assert.Equal(cheepDto.CheepId, db.Reactions.First().CheepId);
        Assert.Equal(authorDto.Id, cheepDto.Reactions.First().AuthorId);
        Assert.Equal(reactionType, cheepDto.Reactions.First().ReactionType);
        
    }
    
    //public Task RemoveReaction(ReactionType reaction, Guid cheepId, Guid authorId);
    [Theory]
    [InlineData(ReactionType.Like)]
    [InlineData(ReactionType.Dislike)]
    [InlineData(ReactionType.Love)]
    public async Task RemoveReaction_ShouldAddRemoveReactionFromCheep(ReactionType reactionType)
    {
        //Arrange
        _ReactionRepository = new ReactionRepository(db);


        Author authorDto = new Author
        {
            UserName = "TestAuthor1", 
            Email = "mock1@email.com"
        };
        Author authorDto1 = new Author
        { 
            UserName = "TestAuthor2", 
            Email = "mock2@email.com" 
        };
        Cheep cheepDto = new Cheep
        {
            CheepId = Guid.Parse("6e579f4c-c2da-420d-adad-40797a71d217"),
            AuthorId = authorDto1.Id, 
            Text = "TestCheep1", 
            Author = authorDto,
            Reactions = new List<Reaction>
            {
                new Reaction
                {
                    CheepId = Guid.Parse("6e579f4c-c2da-420d-adad-40797a71d217"),
                    AuthorId = authorDto.Id,
                    Author = authorDto,
                    ReactionType = reactionType
                }
            }
        };
            
        db.Users.Add(authorDto);
        db.Cheeps.Add(cheepDto);
        

        db.SaveChanges(); 
        
        //Act
        await _ReactionRepository.RemoveReaction(
            reactionType,
            cheepDto.CheepId,
            authorDto.Id
        );

        //Assert
        Assert.Equal(1, cheepDto.Reactions.Count);
        Assert.Equal(cheepDto.CheepId, db.Reactions.First().CheepId);
        Assert.Equal(authorDto.Id, cheepDto.Reactions.First().AuthorId);
        Assert.Equal(reactionType, cheepDto.Reactions.First().ReactionType);
        
    }
        
    
    
    //public Task<int> GetReactionCount(Guid cheepId, ReactionType reactionType);
    [Fact]
    public void GetReactionCount_ShouldReturnTheCorrectAmountOfReactions()
    {
        //Arrange
        
        //Act
        
        //Assert
        
    }
    
    
    //public Task<bool> HasUserReacted(Guid cheepId, Guid authorId);
    [Fact]
    public void HasUserReacted_ShouldReturnTrueWhenUserHasReacted()
    {
        //Arrange
        
        //Act
        
        //Assert
        
    }
}