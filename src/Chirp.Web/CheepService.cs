using System.Globalization;
using Chirp.Core.Entities;
using Chirp.Core.Repository;

namespace Chirp.Web;

public record CheepViewModel(string Author, string Message, string Timestamp);


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
            cheeps.Add(new CheepViewModel(cheepDto.Author.UserName, cheepDto.Text, cheepDto.TimeStamp.ToString(CultureInfo.InvariantCulture)));
        }
        
        return cheeps;
    }
    
    public ICollection<CheepViewModel> GetCheepsFromAuthor(string author, int page)
    {
        ICollection<Cheep> cheepDtos = _authorRepository.GetCheepsByAuthor(author, page);
        ICollection<CheepViewModel> cheeps = new List<CheepViewModel>();

        foreach (Cheep cheepDto in cheepDtos)
        {
            cheeps.Add(new CheepViewModel(cheepDto.Author.UserName, cheepDto.Text, cheepDto.TimeStamp.ToString(CultureInfo.InvariantCulture)));
        }
        
        return cheeps;
    }
}