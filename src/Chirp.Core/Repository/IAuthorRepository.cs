using Chirp.Core.Entities;

namespace Chirp.Core.Repository;

public interface IAuthorRepository
{
    public ICollection<CheepDTO> GetCheepsByAuthor(string name, int page);
    public AuthorDTO GetAuthorById(Guid authorId);
    public AuthorDTO GetAuthorByName(string name);
    public AuthorDTO GetAuthorByEmail(string email);
    public void AddAuthor(AuthorDTO authorDto);
    
}