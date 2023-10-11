using Chirp.Razor.Models;

namespace Chirp.Razor.Repository;

public class AuthorRepository : BaseRepository, IAuthorRepository
{
    public String GetAuthorById(int authorId)
    {
        return db.Authors
            .Where(a => a.AuthorId == authorId)
            .Select(a => a.Name)
            .FirstOrDefault()!;
    }

    public ICollection<Cheep> GetCheepsByAuthor(Author author)
    {
        //Check that author has cheeps
        if (!db.Cheeps.Any(c => c.AuthorId == author.AuthorId))
        {
            throw new Exception("Author " + author.Name + " has no cheeps");
        }
        
        // select Authors entire iCollection of Cheeps
        return db.Authors
            .Where(a => a.AuthorId == author.AuthorId)
            .Select(a => a.Cheeps).FirstOrDefault()!;
    }
}