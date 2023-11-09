using Chirp.Core.Entities;
using Chirp.Core.Repository;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using ValidationException = System.ComponentModel.DataAnnotations.ValidationException;


namespace Chirp.Infrastructure.Repository;

public class CheepRepository : BaseRepository, ICheepRepository
{
   
    private readonly ChirpDbContext chirpDbContext;

    public CheepRepository(ChirpDbContext DbContext) : base(DbContext)
    {
    
        chirpDbContext = DbContext;

    }
    public ICollection<Cheep> GetCheepsByPage(int page)
    {
        //Use EF to get the specified page of cheeps from the database
        ICollection<Cheep> cheeps = db.Cheeps.Include(e => e.Author)
            .OrderByDescending(c => c.CheepId)
            .Skip(PageSize * page)
            .Take(PageSize)
            .ToList();

        return cheeps;
    }

    public void DeleteCheepById(Guid cheepId)
    {
        //Delete the specified cheep from the database
        Cheep? cheep = db.Cheeps.Find(cheepId);
        if (cheep != null)
        {
            db.Cheeps.Remove(cheep);
        }
        else
        {
            throw new Exception("Cheep with id " + cheepId + " not found");
        }

        db.SaveChanges();
    }

    public void AddCheep(Cheep cheep)
    {
        //Check if author is in database, if not add them too
        if (!db.Authors.Any(a => a.AuthorId == cheep.AuthorId))
        {
            db.Authors.Add(cheep.Author);
        }

        db.Cheeps.Add(cheep);
        db.SaveChanges();
    }
    
    // TODO Should CheepRepo contain this method? If yes, why? If not, delete.
    private Author GetAuthorById(string authorId)
    {
        var author = (Author) db.Authors.Include(e => e.Cheeps)
            .Where(a => a.AuthorId == Guid.Parse(authorId));
        
        return author;
    }
    
    
}