using Chirp.Razor;
using Chirp.Razor.Models;
using Microsoft.EntityFrameworkCore;

namespace a;

public class ChirpDBContextUnitTests
{
    private ChirpDBContext Db;
    
    private void setUp()
    {
        Db = new ChirpDBContext();
    }
    
    [Fact]
    public void DBContextContainsCheeps()
    {
        setUp();
        
        Assert.True(Db.Cheeps.Any());
    }

    [Fact]
    public void DBContextContainsAuthors()
    {
        setUp();
        
        Assert.True(Db.Authors.Any());
    }

    [Fact]
    public void QueryByCheepIdReturnsCheep()
    {
        setUp();

        Cheep cheep = Db.Cheeps.Find(1);
        
        Assert.NotNull(cheep);
        Assert.Equal(cheep.CheepId, 1);
        Assert.Equal(cheep.AuthorId, 10);
        Assert.Equal(cheep.Text, "They were married in Chicago, with old Smith, and was expected aboard every day; meantime, the two went past me.");
        Assert.Equal(cheep.TimeStamp, DateTime.Parse("2023-08-01 13:14:37"));
    }

    [Fact]
    public void QueryByAuthorIdReturnsAuthor()
    {
        
        setUp();
    
        Author author = Db.Authors
            .Include(a => a.Cheeps)
            .FirstOrDefault(a => a.AuthorId == 12);
    
        
        Assert.NotNull(author);
        Assert.Equal(author.AuthorId, 12);
        Assert.True(author.Cheeps.Any(a => a.CheepId.Equals(657)));
    }
}