namespace Chirp.Razor;

public interface IAuthorRepository
{
    public IEnumerable<CheepViewModel> GetCheepsByAuthor(Author author);
    
}