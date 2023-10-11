using Chirp.Razor.Models;

namespace Chirp.Razor.Repository;

public interface IAuthorRepository
{
    public ICollection<Cheep> GetCheepsByAuthor(Author author);
    public String GetAuthorById(int authorId);
    
}