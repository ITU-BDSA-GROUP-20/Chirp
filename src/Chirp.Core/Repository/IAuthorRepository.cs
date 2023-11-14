using Chirp.Core.Entities;

namespace Chirp.Core.Repository;

public interface IAuthorRepository
{
    public ICollection<Cheep> GetCheepsByAuthor(string name, int page);
    public Author GetAuthorById(Guid authorId);
    public Author GetAuthorByName(string name);
    public Author GetAuthorByEmail(string email);
    public void AddAuthor(Author authorDto);

    
}