using System.Globalization;
using Chirp.Core.Entities;
using Chirp.Core.Repository;
using Microsoft.EntityFrameworkCore;

namespace Chirp.Infrastructure.Repository;

public class AuthorRepository : BaseRepository, IAuthorRepository
{
    public AuthorRepository(ChirpDbContext chirpDbContext) : base(chirpDbContext)
    {
        db.Users.Include(e => e.Following);
        db.Users.Include(e => e.Followers);
    }

    public void AddAuthor(Author author)
    {
        db.Users.Add(author);
        db.SaveChanges();
    }

    public int GetCheepCountByAuthor(Guid authorId)
    {
        Author author = GetAuthorById(authorId);
        //Check that author has cheeps
        if (author.Cheeps == null || !(author.Cheeps.Any()))
        {
            return 0;
        }

        return author.Cheeps.Count;
    }

    public int GetPageCountByAuthor(Guid authorId)
    {
        return GetCheepCountByAuthor(authorId) / PageSize + 1;
    }

    public Author GetAuthorById(Guid authorId)
    {
        Author author = db.Users
            .Include(e => e.Cheeps)
            .Include(e => e.Following)
            .Include(e => e.Followers)
            .Where(a => a.Id == authorId).FirstOrDefault()!;
            
        return author;
    }
    
    public async Task<Author> GetAuthorByIdAsync(Guid authorId)
    {
        Author author = await db.Users
            .Include( e => e.Cheeps)
            .Include(e => e.Following)
            .Include(e => e.Followers)
            .Where(a => a.Id == authorId).FirstOrDefaultAsync()!;
         
         
        return author!;
    }
    
    public Author GetAuthorByName(string name)
    {
        Author author = db.Users
            .Include(e => e.Cheeps)
            .Include(e => e.Following)
            .Include(e => e.Followers)
            .Where(a => a.UserName == name).FirstOrDefault()!;
            
        return author;
    }
    
    public Author GetAuthorByEmail(string email)
    {
        Author author = db.Users
            .Include(e => e.Cheeps)
            .Where(a => a.Email == email).FirstOrDefault()!;
            
        return author;
    }

    public ICollection<Cheep> GetCheepsByAuthor(Guid id, int page)
    {
        Author author = GetAuthorById(id);
        
        //Check that author has cheeps
        if (author.Cheeps == null || !(author.Cheeps.Any()))
        {
            throw new Exception("Author " + author.UserName + " has no cheeps");
        }

        if(page==0){
            page=1;
        }

        int pageSizeIndex = (page - 1) * PageSize;
        
        if(author.Cheeps.Count < pageSizeIndex + PageSize) return author.Cheeps.ToList<Cheep>().GetRange(pageSizeIndex,author.Cheeps.Count - pageSizeIndex);
        if(author.Cheeps.Count > 32) return author.Cheeps.ToList<Cheep>().GetRange(pageSizeIndex,PageSize);
        return author.Cheeps;
    }
    
    public ICollection<Cheep> GetCheepsByAuthorAndFollowing(Guid id, int page)
    {
        Author author = GetAuthorById(id);
        //Get cheeps from the author, and append cheeps from followers to that list
        ICollection<Author> following = GetFollowingByAuthor(id);
        ICollection<Cheep> cheeps = GetCheepsByAuthor(id, page);

        foreach (Author follower in following)
        {   
            //If follower has no cheeps, skip them
            if (follower.Cheeps == null || !(follower.Cheeps.Any()))
            {
               continue;
            }
     
            //Add each cheep from the follower to the list
            //TODO Try to find alternative to foreach
            foreach (var cheepDto in follower.Cheeps)
            {
                cheeps.Add(cheepDto);
            }
            
        }
        //Sort the cheeps according to timestamp, latest first
        cheeps = cheeps.OrderByDescending(c => c.TimeStamp).ToList();
        
        int pageSizeIndex = (page - 1) * PageSize;
        
        if(cheeps.Count < pageSizeIndex + PageSize) return cheeps.ToList<Cheep>().GetRange(pageSizeIndex,cheeps.Count - pageSizeIndex);
        if(cheeps.Count > 32) return cheeps.ToList<Cheep>().GetRange(pageSizeIndex,PageSize);
        return cheeps;
    }

    public ICollection<Author> GetFollowersByAuthor(Guid id)
    {
        Author author = GetAuthorById(id);

        //Check that author has followers
        if (author.Followers == null || !(author.Followers.Any()))
        {
            throw new Exception("Author " + author.UserName + " has no followers");
        }

        return author.Followers;
    }

    public ICollection<Author> GetFollowingByAuthor(Guid id)
    {
        Author author = GetAuthorById(id);

        //Check that author has following
        if (author.Following == null || !(author.Following.Any()))
        {
            throw new Exception("Author " + author.UserName + " has no following");
        }

        return author.Following;
    }

    public async Task AddFollowing(Author user, Author following) {
        
        user.Following.Add(following);
        await AddFollower(following, user);
        db.Users.Update(user);
        await db.SaveChangesAsync();
    }
    public async Task RemoveFollowing(Author user, Author following) {
        user.Following.Remove(following);
        await RemoveFollower(following, user);

        // Load the Followers collection explicitly
        await db.Entry(user).Collection(u => u.Followers).LoadAsync();

        db.Users.Update(user);
        await db.SaveChangesAsync();
    }

    private async Task AddFollower(Author user, Author follower)
    {
        user.Followers.Add(follower);
        db.Users.Update(user);
        await db.SaveChangesAsync();

    }

    private async Task RemoveFollower(Author user, Author follower)
    {
        // Load the Followers collection explicitly
        await db.Entry(user).Collection(u => u.Followers).LoadAsync();

        user.Followers.Remove(follower);
        db.Users.Update(user);
        await db.SaveChangesAsync();

    }
}