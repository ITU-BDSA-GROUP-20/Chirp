using Chirp.Razor.Models;

namespace Chirp.Razor.Repository;

public interface IAuthorRepository
{
    public List<CheepViewModel> GetCheepsByAuthor(Author author);
    public String GetAuthorById(int authorId);
    
}