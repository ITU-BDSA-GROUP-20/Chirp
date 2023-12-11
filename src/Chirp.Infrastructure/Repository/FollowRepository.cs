using Chirp.Core.Entities;
using Chirp.Core.Repository;
using Chirp.Infrastructure.Repository;

namespace Chirp.Infrastructure.Repository;

public class FollowRepository : BaseRepository, IFollowRepository
{
    public FollowRepository(ChirpDbContext chirpDbContext) : base(chirpDbContext)
    {
        
    }
    public Follow CreateFollow(Author follower, Author following)
    {
        Follow follow = new()
        {
            Follower = follower,
            Following = following
        };
        return follow;
    }
}