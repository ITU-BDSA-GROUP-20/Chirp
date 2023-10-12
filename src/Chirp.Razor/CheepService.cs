using System.Data.SqlTypes;
using Chirp.Razor;
using Chirp.Razor.Models;
using Chirp.Razor.Repository;
using Microsoft.VisualBasic;

public record CheepViewModel(string Author, string Message, string Timestamp);


public interface ICheepService
{
    public List<CheepViewModel> GetCheeps(int page);
    public List<CheepViewModel> GetCheepsFromAuthor(string author, int page);
}

public class CheepService : ICheepService
{
    
    private readonly IAuthorRepository _Author;
    private readonly ICheepRepository _Cheep;

    public CheepService(ChirpDBContext db)
    {
        _Author = new AuthorRepository(db);
        _Cheep = new CheepRepository(db);
    }
    
    public List<CheepViewModel> GetCheeps(int page)
    {
        return _Cheep.GetCheepsByPage(page);;
    }
    

    public List<CheepViewModel> GetCheepsFromAuthor(string author, int page)
    {
        var authorCheeps = _Author.GetCheepsByAuthor(author, page);
        return authorCheeps;
    }
}