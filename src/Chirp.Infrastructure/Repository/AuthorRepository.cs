using System.Globalization;
using Chirp.Core.Entities;
using Chirp.Core.Repository;
using Microsoft.EntityFrameworkCore;

namespace Chirp.Infrastructure.Repository;

public class AuthorRepository : BaseRepository, IAuthorRepository
{
    private readonly IFollowRepository _followRepository;
    public AuthorRepository(ChirpDbContext chirpDbContext) : base(chirpDbContext)
    {
        db.Users.Include(e => e.Followers);
        _followRepository = new FollowRepository(chirpDbContext);
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
            .Include(e => e.Followers)
            .Where(a => a.Id == authorId).FirstOrDefault()!;
            
        return author;
    }
    
    public async Task<Author> GetAuthorByIdAsync(Guid authorId)
    {
        Author author = await db.Users
            .Include( e => e.Cheeps)
            .Include(e => e.Followers)
            .Where(a => a.Id == authorId).FirstOrDefaultAsync()!;
         
         
        return author!;
    }
    
    public Author GetAuthorByName(string name)
    {
        Author author = db.Users
            .Include(e => e.Cheeps)
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
        ICollection<Author> following = GetFollowingById(id);
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

    public ICollection<Author> GetFollowersById(Guid id)
    {
        Author author = GetAuthorById(id);

        //Check that author has followers
        if (author.Followers == null || !(author.Followers.Any()))
        {
            throw new Exception("Author " + author.UserName + " has no followers");
        }
        
        ICollection<Author> followers = new List<Author>();
        
        foreach (Follow follow in author.Followers)
        {
            if (follow.FollowingAuthorId != id) 
            {
                followers.Add(follow.FollowingAuthor);
            }
        }
        
        return followers;
    }

    public ICollection<Author> GetFollowingById(Guid id)
    {
        Author author = GetAuthorById(id);

        //Check that author has followers
        if (author.Followers == null || !(author.Followers.Any()))
        {
            throw new Exception("Author " + author.UserName + " has no followers");
        }
        
        ICollection<Author> followers = new List<Author>();
        
        foreach (Follow follow in author.Followers)
        {
            if (follow.FollowingAuthorId.Equals(id)) 
            {
                followers.Add(follow.FollowingAuthor);
            }
        }
        
        return followers;
    }

    public async Task AddFollowing(Author followingAuthor, Author followedAuthor)
    {
        Follow follow = _followRepository.CreateFollow(followingAuthor, followedAuthor);
        
        followingAuthor.Followers.Add(follow);
        followedAuthor.Followers.Add(follow);
        
        db.Users.Update(followingAuthor);
        db.Users.Update(followedAuthor);
        
        await db.SaveChangesAsync();
    }
    
    public async Task RemoveFollow(Author followingAuthor, Author followedAuthor) {
        // Load the Follow collections explicitly
        await db.Entry(followingAuthor).Collection(f => f.Followers).LoadAsync();
        await db.Entry(followedAuthor).Collection(u => u.Followers).LoadAsync();

        followingAuthor.Followers.Remove(followingAuthor.Followers.FirstOrDefault(f => f.FollowedAuthorId == followedAuthor.Id));
        followedAuthor.Followers.Remove(followedAuthor.Followers.FirstOrDefault(f => f.FollowingAuthorId == followingAuthor.Id));

        db.Users.Update(followingAuthor);
        db.Users.Update(followedAuthor);
        
        await SaveContext();
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
        
        var following = user.Followers.ToList();
        db.Follows.RemoveRange(following);

        db.Users.Remove(user);

        await db.SaveChangesAsync();
    }
    
    public async Task SaveContext()
    {
        await db.SaveChangesAsync();
    }
}