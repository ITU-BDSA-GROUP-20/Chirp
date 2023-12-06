using System.Globalization;
using Chirp.Core.Entities;
using Chirp.Core.Repository;

namespace Chirp.Web;

public record CheepViewModel(string Author, string Message, string Timestamp, ICollection<ReactionDTO> Reactions, Guid CheepId);


public interface ICheepService
{
    public ICollection<CheepViewModel> GetCheeps(int page);
    public ICollection<CheepViewModel> GetCheepsFromAuthor(string author, int page);
}

public class CheepService : ICheepService
{
    private readonly IAuthorRepository _authorRepository;
    private readonly ICheepRepository _cheepRepository;
    private readonly IReactionRepository _reactionRepository;
    
    public CheepService(ICheepRepository cheepRepository, IAuthorRepository authorRepository, IReactionRepository reactionRepository)
    {
        _cheepRepository = cheepRepository;
        _authorRepository = authorRepository;
        _reactionRepository = reactionRepository;
    }
    
    public ICollection<CheepViewModel> GetCheeps(int page)
    {
        ICollection<Cheep> cheepDtos = _cheepRepository.GetCheepsByPage(page);
        List<CheepViewModel> cheeps = new List<CheepViewModel>();

        foreach (Cheep cheepDto in cheepDtos)
        {
            List<ReactionDTO> reactionTypeCounts = new List<ReactionDTO>();
            
            foreach (ReactionType reactionType in Enum.GetValues(typeof(ReactionType)))
            {
                int count = cheepDto.Reactions
                    .Where(r => r.ReactionType == reactionType)
                    .Count();
                List<Guid> authorsThatReacted = cheepDto.Reactions
                    .Where(r => r.ReactionType == reactionType)
                    .Select(r => r.AuthorId)
                    .ToList();    
                reactionTypeCounts.Add(new ReactionDTO(reactionType, count, authorsThatReacted));                
            }
            cheeps.Add(new CheepViewModel(cheepDto.Author.UserName, cheepDto.Text, cheepDto.TimeStamp.ToString(CultureInfo.InvariantCulture), reactionTypeCounts, cheepDto.CheepId));
        }
        
        return cheeps;
    }
    
    public ICollection<CheepViewModel> GetCheepsFromAuthor(string author, int page)
    {
        ICollection<Cheep> cheepDtos = _authorRepository.GetCheepsByAuthor(author, page);
        ICollection<CheepViewModel> cheeps = new List<CheepViewModel>();

            foreach (Cheep cheepDto in cheepDtos)
            {
                List<ReactionDTO> reactionTypeCounts = new List<ReactionDTO>();
                
                
                foreach (ReactionType reactionType in Enum.GetValues(typeof(ReactionType)))
                {
                    int count = 0;
                    
                    if (cheepDto.Reactions != null)
                    {
                        count = cheepDto.Reactions
                            .Where(r => r.ReactionType == reactionType)
                            .Count();
                        
                            List<Guid> authorsThatReacted = cheepDto.Reactions
                            .Where(r => r.ReactionType == reactionType)
                            .Select(r => r.AuthorId)
                            .ToList();
                            
                        reactionTypeCounts.Add(new ReactionDTO(reactionType, count, authorsThatReacted));       
                    }
                }
                cheeps.Add(new CheepViewModel(cheepDto.Author.UserName, cheepDto.Text, cheepDto.TimeStamp.ToString(CultureInfo.InvariantCulture), reactionTypeCounts, cheepDto.CheepId));
            }
        
        return cheeps;
    }
}