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
        if (followingUserId == Guid.Empty || followedUserId == Guid.Empty)
            return false;

        Author author = db.Users
            .Include(e => e.Following)
            .ThenInclude(f => f.FollowedAuthor)
            .FirstOrDefault(a => a.Id == followingUserId);

        if (author == null || author.Following == null)
            return false;

        // Ensure FollowedAuthor is loaded for each Following entry
        foreach (var following in author.Following)
        {
            if (following.FollowedAuthor == null)
                db.Entry(following).Reference(f => f.FollowedAuthor).Load();
        }

        return author.Following.Any(f => f.FollowedAuthor?.Id == followedUserId);
    }
}