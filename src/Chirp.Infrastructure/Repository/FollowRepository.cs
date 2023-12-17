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
    public Follow CreateFollow(Author? followingAuthor, Author? followedAuthor)
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
    
    public bool IsFollowing(Guid followingUserId, Guid followedUserId)
    {
        Author author = db.Users
            .Include(e => e.Following).FirstOrDefault(a => a.Id == followingUserId)!;
        
        return author.Following.Any(f => f.FollowedAuthor.Id == followedUserId);
    }
        // TODO implement equals method
}