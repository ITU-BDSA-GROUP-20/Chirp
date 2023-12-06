using System.Globalization;
using Chirp.Core.Entities;
using Chirp.Core.Repository;

namespace Chirp.Web;

public record CheepViewModel(string Author, string Message, string Timestamp, ICollection<ReactionDTO> Reactions);


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
            List<ReactionDTO> reactionTypeCounts = new List<ReactionDTO>();
                
            Console.WriteLine("VI PRINTER CHEEPs");
                
            foreach (ReactionType reactionType in Enum.GetValues(typeof(ReactionType)))
            {
                    
                Console.WriteLine(reactionType);
                    
                int count = cheepDto.Reactions
                    .Where(r => r.ReactionType == reactionType)
                    .Count();
                    
                reactionTypeCounts.Add(new ReactionDTO(reactionType, count));                
            }
            cheeps.Add(new CheepViewModel(cheepDto.Author.UserName, cheepDto.Text, cheepDto.TimeStamp.ToString(CultureInfo.InvariantCulture), reactionTypeCounts));
        }
        
        return cheeps;
    }
    
    public ICollection<CheepViewModel> GetCheepsFromAuthor(Guid id, int page)
    {
        ICollection<Cheep> cheepDtos = _authorRepository.GetCheepsByAuthor(id, page);
        ICollection<CheepViewModel> cheeps = new List<CheepViewModel>();

            foreach (Cheep cheepDto in cheepDtos)
            {
                List<ReactionDTO> reactionTypeCounts = new List<ReactionDTO>();
                
                Console.WriteLine("VI PRINTER CHEEPs");
                
                foreach (ReactionType reactionType in Enum.GetValues(typeof(ReactionType)))
                {
                    
                    Console.WriteLine(reactionType);

                    int count = 0;
                    
                    if (cheepDto.Reactions != null)
                    {
                        count = cheepDto.Reactions
                            .Where(r => r.ReactionType == reactionType)
                            .Count();
                        reactionTypeCounts.Add(new ReactionDTO(reactionType, count));       
                    }
                }
                cheeps.Add(new CheepViewModel(cheepDto.Author.UserName, cheepDto.Text, cheepDto.TimeStamp.ToString(CultureInfo.InvariantCulture), reactionTypeCounts));
            }
        
        return cheeps;
    }
    
    public ICollection<Author> GetFollowers(Guid id)
    {
        return _authorRepository.GetFollowersByAuthor(id);
    }
    
    public ICollection<Author> GetFollowing(Guid id)
    {
        return _authorRepository.GetFollowingByAuthor(id);
    }
}