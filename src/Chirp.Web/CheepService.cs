using System.Globalization;
using Chirp.Core.Entities;
using Chirp.Core.Repository;

namespace Chirp.Web;

public record CheepViewModel(string Author, Guid AuthorId, string Message, string Timestamp, ICollection<ReactionDTO> Reactions);


public interface ICheepService
{
    public ICollection<CheepViewModel> GetCheeps(int page);
    public ICollection<CheepViewModel> GetCheepsFromAuthor(Guid authorId, int page);
    public ICollection<Author> GetFollowers(Guid id);
    public ICollection<Author> GetFollowing(Guid id);
}

public class CheepService : ICheepService
{
    private readonly IAuthorRepository _authorRepository;
    private readonly ICheepRepository _cheepRepository;

    public CheepService(ICheepRepository cheepRepository, IAuthorRepository authorRepository)
    {
        _cheepRepository = cheepRepository;
        _authorRepository = authorRepository;
    }
    
    public ICollection<CheepViewModel> GetCheeps(int page)
    {
        ICollection<Cheep> cheepDtos = _cheepRepository.GetCheepsByPage(page);
        List<CheepViewModel> cheeps = new List<CheepViewModel>();

        foreach (Cheep cheepDto in cheepDtos)
        {
            List<ReactionDTO> reactionTypeCounts = CheepReactions(cheepDto);
            
            cheeps.Add(new CheepViewModel(cheepDto.Author.UserName, cheepDto.AuthorId, cheepDto.Text, cheepDto.TimeStamp.ToString(CultureInfo.InvariantCulture), reactionTypeCounts));
        }

        return cheeps;
    }
    
    public ICollection<CheepViewModel> GetCheepsFromAuthor(Guid id, int page)
    {
        ICollection<Cheep> cheepDtos = _authorRepository.GetCheepsByAuthor(id, page);
        ICollection<CheepViewModel> cheeps = new List<CheepViewModel>();

        foreach (Cheep cheepDto in cheepDtos)
        {
            List<ReactionDTO> reactionTypeCounts = CheepReactions(cheepDto);

            cheeps.Add(new CheepViewModel(cheepDto.Author.UserName, cheepDto.AuthorId, cheepDto.Text, cheepDto.TimeStamp.ToString(CultureInfo.InvariantCulture), reactionTypeCounts));
        }
        
        return cheeps;
    }
    
    private List<ReactionDTO> CheepReactions(Cheep cheepDto)
    {
        var reactions = new List<ReactionDTO>();

        // Check if cheepDto.Reactions is null
        if (cheepDto.Reactions == null || !cheepDto.Reactions.Any())
        {
            // If cheepDto.Reactions is null or empty, initialize reaction counts to 0 for each ReactionType
            foreach (ReactionType reactionType in Enum.GetValues(typeof(ReactionType)))
            {
                reactions.Add(new ReactionDTO(reactionType, 0));
            }
        }
        else
        {
            var reactionCounts = cheepDto.Reactions
                .GroupBy(r => r.ReactionType)
                .ToDictionary(g => g.Key, g => g.Count());

            foreach (ReactionType reactionType in Enum.GetValues(typeof(ReactionType)))
            {
                int count = reactionCounts.TryGetValue(reactionType, out var reactionCount) ? reactionCount : 0;
                reactions.Add(new ReactionDTO(reactionType, count));
            }
        }

        return reactions;
    }

    public ICollection<Author> GetFollowers(Guid id)
    {
        return _authorRepository.GetAuthorById(id).Followers;
    }
    
    public ICollection<Author> GetFollowing(Guid id)
    {
        return _authorRepository.GetAuthorById(id).Following;
    }
}