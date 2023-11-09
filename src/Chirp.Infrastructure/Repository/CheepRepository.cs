using System.Globalization;
using Chirp.Core.Entities;
using Chirp.Core.Repository;
using Microsoft.EntityFrameworkCore;

namespace Chirp.Infrastructure.Repository;

public class CheepRepository : BaseRepository, ICheepRepository
{
    public CheepRepository(ChirpDbContext chirpDbContext) : base(chirpDbContext)
    {
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
        if (!db.Users.Any(a => a.Id == cheep.Author.Id))
        {
            db.Users.Add(cheep.Author);
        }
        db.Cheeps.Add(cheep);
        db.SaveChanges();
    }
    
    // TODO Should CheepRepo contain this method? If yes, why? If not, delete.
    private String GetAuthorById(string authorId)
    {
        String authorName = db.Users
            .Include(e => e.Cheeps)
            .Where(a => a.Id == Guid.Parse(authorId))
            .Select(a => a.UserName)
            .FirstOrDefault()!;
        
        return authorName;
    }
}