using Chirp.Core.Entities;

namespace Chirp.Core.Repository;

public interface IFollowRepository
{
    public Task AddFollow(Guid id, Guid author2FollowId);
    public Task RemoveFollow(Guid id, Guid author2removeId);
    public Task<bool> IsFollowing(Guid authorId, Guid author2FollowId);
    
    public Task ICollection<Follow> GetFollowById(Guid authorId);
    
}