namespace Chirp.Core.Entities;

public class Reaction {
    public Guid ChirpId { get; set; }
    public Cheep Cheep { get; set; }
    public Author Author { get; set; }

    public Guid AuthorId { get; set; }
    
    public ReactionType ReactionType { get; set; }
}