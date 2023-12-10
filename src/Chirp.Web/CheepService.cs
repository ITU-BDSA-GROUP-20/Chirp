using System.Globalization;
using Chirp.Core.Entities;
using Chirp.Core.Repository;
using Chirp.Web.Models;

namespace Chirp.Web;

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
            List<ReactionDTO> reactionTypeCounts = CheepReactions(cheepDto);
            
            cheeps.Add(new CheepViewModel(cheepDto.CheepId, new UserModel(cheepDto.Author), cheepDto.Text, cheepDto.TimeStamp.ToString(CultureInfo.InvariantCulture), reactionTypeCounts));
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

            cheeps.Add(new CheepViewModel(cheepDto.CheepId, new UserModel(cheepDto.Author), cheepDto.Text, cheepDto.TimeStamp.ToString(CultureInfo.InvariantCulture), reactionTypeCounts));
        }
        
        return cheeps;
    }
    
    private List<ReactionDTO> CheepReactions(Cheep cheepDto)
    {
        // Initialize reactions with all reaction types set to count 0.
        var reactions = Enum.GetValues(typeof(ReactionType))
            .Cast<ReactionType>()
            .ToDictionary(rt => rt, rt => new ReactionDTO(rt, 0));

        // If cheepDto.Reactions is not null and has elements, process them.
        if (cheepDto.Reactions?.Any() == true)
        {
            foreach (Reaction reaction in cheepDto.Reactions)
            {
                reactions[reaction.ReactionType].ReactionCount++;
            }
        }

        return reactions.Values.ToList();
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