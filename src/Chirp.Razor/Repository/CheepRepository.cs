using Chirp.Razor.Models;

namespace Chirp.Razor.Repository;

public class CheepRepository : BaseRepository, ICheepRepository
{

    public IEnumerable<CheepViewModel> GetCheepsByPage(int page)
    {
        //Use EF to get the specified page of cheeps from the database
        List<CheepViewModel> cheeps = db.Cheeps
            .OrderByDescending(c => c.CheepId)
            .Skip(PageSize * page)
            .Take(PageSize)
            .Select(c => new CheepViewModel(GetAuthorById(c.AuthorId), c.Text, c.TimeStamp.ToString()))
            .ToList();
        return cheeps;
    }

    public void DeleteCheepById(int cheepId)
    {
        //Delete the specified cheep from the database
        Cheep cheep = db.Cheeps.Find(cheepId);
        if (cheep != null)
        {
            db.Cheeps.Remove(cheep);
        }
        else
        {
            throw new Exception("Cheep with id " + cheepId + " not found");
        }
    }

    public void AddCheep(Cheep cheep)
    {
        db.Cheeps.Add(cheep);
    }

    private String GetAuthorById(int authorId)
    {
        AuthorRepository authorRepository = new();
        return authorRepository.GetAuthorById(authorId);
    }
    
    
}