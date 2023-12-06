using System.Globalization;
using Chirp.Core.Entities;
using Chirp.Core.Repository;

namespace Chirp.Web;

public record CheepViewModel(string Author, Guid AuthorId, string Message, string Timestamp, ICollection<ReactionDTO> Reactions);


public interface ICheepService
{
    public ICollection<CheepViewModel> GetCheeps(int page);
    public ICollection<CheepViewModel> GetCheepsFromAuthor(string author, int page);
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
            List<ReactionDTO> reactionTypeCounts = cheepReactions(cheepDto);
            
            cheeps.Add(new CheepViewModel(cheepDto.Author.UserName, cheepDto.AuthorId, cheepDto.Text, cheepDto.TimeStamp.ToString(CultureInfo.InvariantCulture), reactionTypeCounts));
        }

        return cheeps;
    }
    
    public ICollection<CheepViewModel> GetCheepsFromAuthor(string author, int page)
    {
        ICollection<Cheep> cheepDtos = _authorRepository.GetCheepsByAuthor(author, page);
        ICollection<CheepViewModel> cheeps = new List<CheepViewModel>();

        foreach (Cheep cheepDto in cheepDtos)
        {
            List<ReactionDTO> reactionTypeCounts = cheepReactions(cheepDto);

            cheeps.Add(new CheepViewModel(cheepDto.Author.UserName, cheepDto.AuthorId, cheepDto.Text, cheepDto.TimeStamp.ToString(CultureInfo.InvariantCulture), reactionTypeCounts));
        }
        
        return cheeps;
    }

    private List<ReactionDTO> cheepReactions(Cheep cheepDto)
    {

        List<ReactionDTO> reactions = new List<ReactionDTO>();

        var reactionCounts = cheepDto.Reactions
            .GroupBy(r => r.ReactionType)
            .ToDictionary(g => g.Key, g => g.Count());

        // Iterate over each ReactionType in the enum
        foreach (ReactionType reactionType in Enum.GetValues(typeof(ReactionType)))
        {
            Console.WriteLine(reactionType);

            // Get the count for this reaction type, defaulting to 0 if it's not in the dictionary
            int count = reactionCounts.TryGetValue(reactionType, out var reactionCount) ? reactionCount : 0;

            reactions.Add(new ReactionDTO(reactionType, count));
        }

        return reactions;
    }
}