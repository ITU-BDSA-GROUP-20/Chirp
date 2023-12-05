using Chirp.Core.Entities;
using Chirp.Core.Repository;

namespace Chirp.Infrastructure.Repository;

public class ReactionRepository: BaseRepository, IReactionRepository
{
    public ReactionRepository(ChirpDbContext dbContext) : base(dbContext)
    {
    }
    
    public async Task AddReaction(ReactionType reaction, Guid cheepId, Guid authorId)
    {
        Cheep? cheep = await db.Cheeps.FindAsync(cheepId);
        Author? author = await db.Users.FindAsync(authorId);
        if (cheep != null)
        {
            Reaction entity = new Reaction()
            {
                ChirpId = cheepId,
                Cheep = cheep,
                AuthorId = authorId,
                Author = author,
                ReactionType = reaction
            };
            db.Reactions.Add(entity);
            await db.SaveChangesAsync();
        }
        else
        {
            //
            throw new Exception("Cheep with id " + cheepId + " not found");
        }
    }
    public async Task RemoveReaction(ReactionType reaction, Guid cheepId, Guid authorId)
    {
        try
        {
            Reaction? entity = await db.Reactions.FindAsync(cheepId, reaction, authorId);
            if (entity != null) db.Reactions.Remove(entity);
            await db.SaveChangesAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine("ReactionRepository: "+ e);
            throw;
        }
       
        
    }
}