using System.Globalization;
using Chirp.Core.Entities;
using Chirp.Core.Repository;
using Chirp.Infrastructure;
using Chirp.Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;

namespace Chirp.Web;

public record CheepViewModel(string Author, string Message, string Timestamp);

public interface ICheepService
{
    public ICollection<CheepViewModel> GetCheeps(int page);
    public ICollection<CheepViewModel> GetCheepsFromAuthor(string author, int page);
}

public class CheepService : ICheepService
{
    
    private readonly IAuthorRepository _Author;
    private readonly ICheepRepository _Cheep;
    private const int PageSize = 32;

    public CheepService(ChirpDbContext db)
    {
        DbInitializer.SeedDatabase(db);
        db.Cheeps.Include(c => c.AuthorDto).ToList();
        _Author = new AuthorRepository(db, PageSize);
        _Cheep = new CheepRepository(db, PageSize);
    }
    
    public ICollection<CheepViewModel> GetCheeps(int page)
    {
        ICollection<CheepDTO> cheepDtos = _Cheep.GetCheepsByPage(page);
        List<CheepViewModel> cheeps = new List<CheepViewModel>();

        foreach (CheepDTO cheepDto in cheepDtos)
        {
            cheeps.Add(new CheepViewModel(cheepDto.AuthorDto.Name, cheepDto.Text, cheepDto.TimeStamp.ToString(CultureInfo.InvariantCulture)));
        }
        
        return cheeps;
    }
    

    public ICollection<CheepViewModel> GetCheepsFromAuthor(string author, int page)
    {
        ICollection<CheepDTO> cheepDtos = _Author.GetCheepsByAuthor(author, page);
        ICollection<CheepViewModel> cheeps = new List<CheepViewModel>();

        foreach (CheepDTO cheepDto in cheepDtos)
        {
            cheeps.Add(new CheepViewModel(cheepDto.AuthorDto.Name, cheepDto.Text, cheepDto.TimeStamp.ToString(CultureInfo.InvariantCulture)));
        }
        
        return cheeps;
    }
}