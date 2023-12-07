using Chirp.Core.Entities;

namespace Chirp.Core.Repository;

public interface IAuthorRepository
{
    public ICollection<Cheep> GetCheepsByAuthor(string name, int page);
    public Author GetAuthorById(Guid authorId);
    public Author GetAuthorByName(string name);
    public Author GetAuthorByEmail(string email);
    public void AddAuthor(Author authorDto);
    public ICollection<Author> GetFollowersByAuthor(string name);
    public ICollection<Author> GetFollowingByAuthor(string name);
    public Task AddFollowing(Author author, Author author2Follow);
    public Task RemoveFollowing(Author author, Author author2remove);
    public Task<Author> GetAuthorByIdAsync(Guid authorId);
    public int GetCheepCountByAuthor(Guid authorId);
    public int GetPageCountByAuthor(Guid authorId);

}