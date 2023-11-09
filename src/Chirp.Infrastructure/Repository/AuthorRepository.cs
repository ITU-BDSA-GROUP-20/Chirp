using System.Globalization;
using Chirp.Core.Entities;
using Chirp.Core.Repository;
using Microsoft.EntityFrameworkCore;

namespace Chirp.Infrastructure.Repository;

public class AuthorRepository : BaseRepository, IAuthorRepository
{
    public AuthorRepository(ChirpDbContext chirpDbContext) : base(chirpDbContext)
    {
    }

    public void AddAuthor(Author author)
    {
        db.Users.Add(author);
        db.SaveChanges();
    }

    public Author GetAuthorById(Guid authorId)
    {
        Author author = db.Users
            .Include(e => e.Cheeps)
            .Where(a => a.Id == authorId).FirstOrDefault()!;
            
        return author;
    }
    public Author GetAuthorByName(string name)
    {
        Author author = db.Users
            .Include(e => e.Cheeps)
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

    public ICollection<Cheep> GetCheepsByAuthor(string name, int page)
    {
        
        Author author = GetAuthorByName(name);
        
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
  
}