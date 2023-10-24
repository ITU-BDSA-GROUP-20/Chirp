using System.Globalization;
using Chirp.Core.Entities;
using Chirp.Core.Repository;

namespace Chirp.Infrastructure.Repository;

public class CheepRepository : BaseRepository, ICheepRepository
{
    public CheepRepository(ChirpDbContext chirpDbContext) : base(chirpDbContext)
    {
    }
    
    public ICollection<CheepDTO> GetCheepsByPage(int page)
    {
        //Use EF to get the specified page of cheeps from the database
        ICollection<CheepDTO> cheeps = db.Cheeps
            .OrderByDescending(c => c.CheepId)
            .Skip(PageSize * page)
            .Take(PageSize)
            .ToList();

        return cheeps;
    }

    public void DeleteCheepById(int cheepId)
    {
        //Delete the specified cheep from the database
        CheepDTO cheep = db.Cheeps.Find(cheepId);
        if (cheep != null)
        {
            db.Cheeps.Remove(cheep);
        }
        else
        {
            throw new Exception("Cheep with id " + cheepId + " not found");
        }
    }

    public void AddCheep(CheepDTO cheep)
    {
        //Check if author is in database, if not add them too
        if (!db.Authors.Any(a => a.AuthorId == cheep.AuthorId))
        {
            db.Authors.Add(cheep.AuthorDto);
        }
        db.Cheeps.Add(cheep);
    }

    private String GetAuthorById(int authorId)
    {
        String authorName = db.Authors
            .Where(a => a.AuthorId == authorId)
            .Select(a => a.Name)
            .FirstOrDefault()!;
        
        return authorName;
    }
}