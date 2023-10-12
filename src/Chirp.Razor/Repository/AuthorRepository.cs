using Chirp.Razor.Models;
using Microsoft.EntityFrameworkCore.Migrations.Operations;

namespace Chirp.Razor.Repository;

public class AuthorRepository : BaseRepository, IAuthorRepository
{
    public String GetAuthorById(int authorId)
    {
        string authorName = db.Authors
            .Where(a => a.AuthorId == authorId)
            .Select(a => a.Name)
            .FirstOrDefault()!;
            
        return authorName;
    }
    public Author GetAuthorByName(string name)
    {
        Author author = db.Authors
            .Where(a => a.Name == name).FirstOrDefault()!;
            
        return author;
    }

    public List<CheepViewModel> GetCheepsByAuthor(string name, int page)
    {
        Author author = GetAuthorByName(name);
        //Check that author has cheeps
        if (!db.Cheeps.Any(c => c.AuthorId == author.AuthorId))
        {
            throw new Exception("Author " + author.Name + " has no cheeps");
        }
        var cheepList = new List<CheepViewModel>();
        
        var cheepCollection = db.Authors
            .Where(a => a.AuthorId == author.AuthorId)
            .Select(a => a.Cheeps)
            .Skip(PageSize * page)
            .Take(PageSize)
            .FirstOrDefault()!;
        
        foreach(var Cheep in cheepCollection)
        {
            cheepList.Add(new CheepViewModel(Cheep.Author.Name, Cheep.Text, Cheep.TimeStamp.ToString()));
        }
        
        // select Authors entire iCollection of Cheeps
        return cheepList;
    }
}