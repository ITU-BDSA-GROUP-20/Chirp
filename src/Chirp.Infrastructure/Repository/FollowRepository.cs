using Chirp.Core.Entities;
using Chirp.Core.Repository;
using Chirp.Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;

namespace Chirp.Infrastructure.Repository;

public class FollowRepository : BaseRepository, IFollowRepository
{
    public FollowRepository(ChirpDbContext chirpDbContext) : base(chirpDbContext)
    {
    }
    public Follow CreateFollow(Author followingAuthor, Author followedAuthor)
    {
        Follow follow = new()
        {
            FollowingAuthor = followingAuthor,
            FollowingAuthorId = followingAuthor.Id,
            FollowedAuthor = followedAuthor,
            FollowedAuthorId = followedAuthor.Id
        };
        return follow;
    }
    
    // TODO implement equals method
}