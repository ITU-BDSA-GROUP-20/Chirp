using Chirp.Core.Entities;

namespace Chirp.Core.Repository;

public interface IReactionRepository
{
    public  Task AddReaction(ReactionType reaction, Guid cheepId, Guid authorId);
    public Task RemoveReaction(ReactionType reaction, Guid cheepId, Guid authorId);
}