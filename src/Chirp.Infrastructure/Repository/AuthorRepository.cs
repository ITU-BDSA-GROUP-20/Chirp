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

    public async Task DeleteCheepsByAuthorId(Guid id)
    {
        Author author = await GetAuthorByIdAsync(id);
        foreach (var cheep in author.Cheeps)
        {
            db.Cheeps.Remove(cheep);
        }
    }
    
    public ICollection<Cheep> GetCheepsByAuthorAndFollowing(Guid id, int page)
    {
        Author author = GetAuthorById(id);
        //Get cheeps from the author, and append cheeps from followers to that list
        ICollection<Follow> following = GetFollowingByAuthor(id);
        ICollection<Cheep> cheeps = GetCheepsByAuthor(id, page);

        foreach (Follow follower in following)
        {   
            //If follower has no cheeps, skip them
            if (follower.Follower.Cheeps == null || !(follower.Follower.Cheeps.Any()))
            {
               continue;
            }
     
            //Add each cheep from the follower to the list
            //TODO Try to find alternative to foreach
            foreach (var cheepDto in follower.Follower.Cheeps)
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

    public ICollection<Follow> GetFollowersByAuthor(Guid id)
    {
        Author author = GetAuthorById(id);

        //Check that author has followers
        if (author.Followers == null || !(author.Followers.Any()))
        {
            throw new Exception("Author " + author.UserName + " has no followers");
        }

        return author.Followers;
    }

    public ICollection<Follow> GetFollowingByAuthor(Guid id)
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
        
        Follow follow = new Follow()
        {
            FollowingId = following.Id,
            Following = following,
            FollowerId = user.Id,
            Follower = user
        };
        
        user.Following.Add(follow);
        await AddFollower(following, user);
        db.Users.Update(user);
        db.Users.Update(following);
        await db.SaveChangesAsync();
    }
    public async Task RemoveFollowing(Author user, Author following) {
        // ------- THIS METHOD DOES NOT SAVE THE CHANGES TO THE DATABASE --------
        
        Follow follow = new Follow()
        {
            FollowingId = following.Id,
            Following = following,
            FollowerId = user.Id,
            Follower = user
        };
        
        user.Following.Remove(follow);
        await RemoveFollower(following, user);

        // Load the Followers collection explicitly
        await db.Entry(user).Collection(u => u.Followers).LoadAsync();

        db.Users.Update(user);
        await SaveContext();
    }

    private async Task AddFollower(Author user, Author follower)
    {
        Follow follow = new Follow()
        {
            FollowingId = user.Id,
            Following = user,
            FollowerId = follower.Id,
            Follower = follower
        };
        
        user.Followers.Add(follow);
        db.Users.Update(user);
        await db.SaveChangesAsync();
    }

    private async Task RemoveFollower(Author user, Author follower)
    {
        // ------- THIS METHOD DOES NOT SAVE THE CHANGES TO THE DATABASE --------
        
        // Load the Followers collection explicitly
        await db.Entry(user).Collection(u => u.Followers).LoadAsync();
        
        Follow follow = new Follow()
        {
            FollowingId = user.Id,
            Following = user,
            FollowerId = follower.Id,
            Follower = follower
        };

        user.Followers.Remove(follow);
        db.Users.Update(user);
        await SaveContext();
    }
    
    public async Task RemoveAllFollowRelationsById(Guid id)
    {
        Author author = await GetAuthorByIdAsync(id);
        
        List<Follow> followers = author.Followers.ToList();
        List<Follow> following = author.Following.ToList();
        
        // Remove all followers
        foreach (Follow follower in followers)
        {
            await RemoveFollower(author, follower.Follower);
        }
        
        // Remove all following
        foreach (var follower in following)
        {
            await RemoveFollowing(author, follower.Following);
        }

        await db.SaveChangesAsync();
    }
    
    public async Task DeleteUserById(Guid id)
    {
        Author user = await GetAuthorByIdAsync(id);
    
        if (user is null)
        {
            throw new Exception("User not found");
        }
        
        // Remove all followers
        var followers = user.Followers.ToList();
        db.Follows.RemoveRange(followers);
        
        var following = user.Following.ToList();
        db.Follows.RemoveRange(following);

        await db.SaveChangesAsync();
    }
    
    public async Task SaveContext()
    {
        await db.SaveChangesAsync();
    }
}