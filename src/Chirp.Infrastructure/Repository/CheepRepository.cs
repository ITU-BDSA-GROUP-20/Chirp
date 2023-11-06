using System.ComponentModel.DataAnnotations;
using Chirp.Core.Entities;
using Chirp.Core.Repository;
using Microsoft.EntityFrameworkCore;


namespace Chirp.Infrastructure.Repository;

public class CheepRepository : BaseRepository, ICheepRepository
{
    private CheepCreateValidator _validator;
    private ChirpDbContext chirpDbContext;

    public CheepRepository(ChirpDbContext DbContext, CheepCreateValidator validator) : base(DbContext)
    {
        _validator = validator;
        chirpDbContext = DbContext;

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
    private AuthorDTO GetAuthorById(string authorId)
    {
        AuthorDTO author = (AuthorDTO) db.Authors.Include(e => e.Cheeps)
            .Where(a => a.AuthorId == Guid.Parse(authorId));
            
        // string authorName = db.Authors
        //     .Include(e => e.Cheeps)
        //     .Where(a => a.AuthorId == Guid.Parse(authorId))
        //     .Select(a => a.Name)
        //     .FirstOrDefault()!;
        
        return author;
    }
    public async Task CreateCheep(CreateCheepDTO cheepDto)
    {
        var validationResult = await _validator.ValidateAsync(cheepDto);
        if (!validationResult.IsValid)
        {
            throw new ValidationException();
        }

        var user = await chirpDbContext.Users.SingleAsync(u => u.UserName == cheepDto.Author);

        var entity = new CheepDTO()
        {
            Text = cheepDto.Text,
            TimeStamp = DateTime.UtcNow,
            //Not sure what to do here, as our CheepDTO takes a guid
            AuthorDto = GetAuthorById(user.Id)
        };


        chirpDbContext.Cheeps.Add(entity);
        
       
        db.SaveChanges();
    }
    
}