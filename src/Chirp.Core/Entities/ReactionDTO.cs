namespace Chirp.Core.Entities;

public record ReactionDTO
{
    public ReactionType ReactionType;
    public int ReactionCount;

    public ReactionDTO(ReactionType reactionType, int reactionCount)
    {
        ReactionType = reactionType;
        ReactionCount = reactionCount;
    }
}