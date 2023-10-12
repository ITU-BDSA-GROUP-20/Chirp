using Chirp.Razor.Models;

namespace Chirp.Razor.Repository;

public interface IAuthorRepository
{
    public List<CheepViewModel> GetCheepsByAuthor(string name, int page);
    public String GetAuthorById(int authorId);
    public Author GetAuthorByName(string name);
    public Author GetAuthorByEmail(string email);
    public void AddAuthor(Author author);
    
}