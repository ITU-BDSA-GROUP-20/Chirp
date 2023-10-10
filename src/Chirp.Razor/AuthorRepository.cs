using Chirp.Razor.Models;

namespace Chirp.Razor;

public class AuthorRepository : BaseRepository, IAuthorRepository
{
    
    public IEnumerable<CheepViewModel> GetCheepsByAuthor(Author author)
    {
        //Get cheeps by Author object
        
    }
    
    public String GetAuthorById(int authorId)
    {
        return db.Authors
            .Where(a => a.AuthorId == authorId)
            .Select(a => a.Name)
            .FirstOrDefault()!;
    }
}