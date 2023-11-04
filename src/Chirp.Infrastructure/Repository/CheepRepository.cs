using System.Globalization;
using Chirp.Core.Entities;
using Chirp.Core.Repository;
using Microsoft.EntityFrameworkCore;

namespace Chirp.Infrastructure.Repository;

public class CheepRepository : BaseRepository, ICheepRepository
{
    
    public CheepRepository(ChirpDbContext chirpDbContext, CheepCreateValidator cheepCreateValidator) : base(chirpDbContext)
    {
      
    }
    
    public ICollection<CheepDTO> GetCheepsByPage(int page)
    {
        //Use EF to get the specified page of cheeps from the database
        ICollection<CheepDTO> cheeps = db.Cheeps.Include(e => e.AuthorDto)
            .OrderByDescending(c => c.CheepId)
            .Skip(PageSize * page)
            .Take(PageSize)
            .ToList();

        return cheeps;
    }

    public void DeleteCheepById(Guid cheepId)
    {
        //Delete the specified cheep from the database
        CheepDTO? cheep = db.Cheeps.Find(cheepId);
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

    public void AddCheep(CheepDTO cheep)
    {
        //Check if author is in database, if not add them too
        if (!db.Authors.Any(a => a.AuthorId == cheep.AuthorId))
        {
            db.Authors.Add(cheep.AuthorDto);
        }
        db.Cheeps.Add(cheep);
        db.SaveChanges();
    }
    
    // TODO Should CheepRepo contain this method? If yes, why? If not, delete.
    private string GetAuthorById(string authorId)
    {
        string authorName = db.Authors
            .Include(e => e.Cheeps)
            .Where(a => a.AuthorId == Guid.Parse(authorId))
            .Select(a => a.Name)
            .FirstOrDefault()!;
        
        return authorName;
    }
    public async void CreateCheep(CreateCheepDTO cheepDto)
    {
       
    }
}