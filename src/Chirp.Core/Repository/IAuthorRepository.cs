using Chirp.Core.Entities;

namespace Chirp.Core.Repository;

public interface IAuthorRepository
{
    public ICollection<Cheep> GetCheepsByAuthor(Guid authorId, int page);
    public Author GetAuthorById(Guid authorId);
    public Author GetAuthorByName(string name);
    public Author GetAuthorByEmail(string email);
    public void AddAuthor(Author authorDto);
    public ICollection<Author?> GetFollowersById(Guid authorId);
    public ICollection<Author?> GetFollowingById(Guid authorId);
    public Task AddFollow(Author? followingAuthor, Author? followedAuthor);
    public Task RemoveFollow(Author? followingAuthor, Author? followedAuthor);
    public Task<Author?> GetAuthorByIdAsync(Guid authorId);
    public int GetCheepCountByAuthor(Guid authorId);
    public int GetPageCountByAuthor(Guid authorId);
    public Task DeleteCheepsByAuthorId(Guid authorId);
    public Task SaveContextAsync();
    public Task RemoveAllFollowersByAuthorId(Guid id);
    public Task RemoveReactionsByAuthorId(Guid id);
    public Task RemoveUserById(Guid id);
    public Task DeleteUserById(Guid authorId);
    public Task DeleteUserById1(Guid id);
}