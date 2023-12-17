using Chirp.Core.Entities;
using Chirp.Infrastructure;
using Chirp.Infrastructure.Repository;
using Test_Utilities;

namespace Chirp.InfrastructureTest.RepositoryTests;

public class FollowRepositoryTest
{
    private readonly ChirpDbContext context;

    public FollowRepositoryTest()
    {
        context = SqliteInMemoryBuilder.GetContext();
    }

    [Fact]
    public void CreateFollow_ShouldCreateFollow()
    {
        // Arrange
        FollowRepository followRepository = new(context);
        
        Author? authorThatFollows = new Author()
        {
            Id = Guid.NewGuid(),
            UserName = "authorThatFollows",
            Email = "Follower@mail.com",
        };
        Author? authorBeingFollowed = new Author()
        {
            Id = Guid.NewGuid(),
            UserName = "authorBeingFollowed",
            Email = "Following@mail.com"
        };
        
        Follow manuelFollow = new()
        {
            FollowingAuthor = authorThatFollows,
            FollowingAuthorId = authorThatFollows.Id,
            FollowedAuthor = authorBeingFollowed,
            FollowedAuthorId = authorBeingFollowed.Id
        };
        
        // Act
        Follow generatedFollow = followRepository.CreateFollow(authorThatFollows, authorBeingFollowed);
        
        // Assert
        Assert.Equal(generatedFollow.FollowingAuthor, manuelFollow.FollowingAuthor);
        Assert.Equal(generatedFollow.FollowingAuthorId, manuelFollow.FollowingAuthorId);
        Assert.Equal(generatedFollow.FollowedAuthor, manuelFollow.FollowedAuthor);
        Assert.Equal(generatedFollow.FollowedAuthorId, manuelFollow.FollowedAuthorId);
    }
}