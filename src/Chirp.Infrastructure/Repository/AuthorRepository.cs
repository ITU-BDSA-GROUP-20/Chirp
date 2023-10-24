using System.Globalization;
using Chirp.Core.Entities;
using Chirp.Core.Repository;

namespace Chirp.Infrastructure.Repository;

public class AuthorRepository : BaseRepository, IAuthorRepository
{
    public AuthorRepository(ChirpDbContext chirpDbContext) : base(chirpDbContext)
    {
    }

    public void AddAuthor(AuthorDTO author)
    {
        db.Authors.Add(author);
    }

    public String GetAuthorById(int authorId)
    {
        string authorName = db.Authors
            .Where(a => a.AuthorId == authorId)
            .Select(a => a.Name)
            .FirstOrDefault()!;
            
        return authorName;
    }
    public AuthorDTO GetAuthorByName(string name)
    {
        AuthorDTO author = db.Authors
            .Where(a => a.Name == name).FirstOrDefault()!;
            
        return author;
    }
    
    public AuthorDTO GetAuthorByEmail(string email)
    {
        AuthorDTO author = db.Authors
            .Where(a => a.Email == email).FirstOrDefault()!;
            
        return author;
    }

    public ICollection<CheepDTO> GetCheepsByAuthor(string name, int page)
    {
        
        AuthorDTO author = GetAuthorByName(name);
        
        //Check that author has cheeps
        if (author.Cheeps.Count == 0)
        {
            throw new Exception("Author " + author.Name + " has no cheeps");
        }

        if(page==0){
            page=1;
        }
        return author.Cheeps.ToList<CheepDTO>().GetRange((page-1)*32,32);
    }
  
}