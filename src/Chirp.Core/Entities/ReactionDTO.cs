namespace Chirp.Core.Entities;

public record ReactionDTO(ReactionType ReactionType, int Count, List<Guid> AuthorIds);