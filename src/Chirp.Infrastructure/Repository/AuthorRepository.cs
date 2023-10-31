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

    public AuthorDTO GetAuthorById(Guid authorId)
    {
        AuthorDTO author = db.Authors
            .Where(a => a.AuthorId == authorId).FirstOrDefault()!;
            
        return author;
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

        int pageSizeIndex = (page - 1) * PageSize;
        
        if(author.Cheeps.Count < pageSizeIndex + PageSize) return author.Cheeps.ToList<CheepDTO>().GetRange(pageSizeIndex,author.Cheeps.Count - pageSizeIndex);
        if(author.Cheeps.Count > 32) return author.Cheeps.ToList<CheepDTO>().GetRange(pageSizeIndex,PageSize);
        return author.Cheeps;
    }
  
}