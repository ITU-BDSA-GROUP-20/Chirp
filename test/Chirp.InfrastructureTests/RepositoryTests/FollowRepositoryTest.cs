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
        
        Author follower = new Author()
        {
            Id = Guid.NewGuid(),
            UserName = "Follower",
            Email = "Follower@mail.com",
        };
        Author following = new Author()
        {
            Id = Guid.NewGuid(),
            UserName = "Following",
            Email = "Following@mail.com"
        };
        
        Follow manuelFollow = new()
        {
            Follower = follower,
            Following = following
        };
        
        // Act
        Follow generatedFollow = followRepository.CreateFollow(follower, following);
        
        // Assert
        Assert.Equal(generatedFollow, manuelFollow);
    }
}