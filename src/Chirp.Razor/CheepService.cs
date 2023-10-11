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
    
    private IAuthorRepository _Author = new AuthorRepository();
    private readonly ICheepRepository _Cheep = new CheepRepository();

    public CheepService()
    {
       
    }
    
    public List<CheepViewModel> GetCheeps(int page)
    {
        return _Cheep.GetCheepsByPage(page);;
    }
    

    public List<CheepViewModel> GetCheepsFromAuthor(string author, int page)
    {
        var _author = _Author.GetCheepsByAuthor(author, page);
        return _author;
    }
}