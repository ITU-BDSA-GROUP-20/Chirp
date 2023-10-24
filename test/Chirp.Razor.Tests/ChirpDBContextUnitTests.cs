using Chirp.Core.Entities;
using Chirp.Infrastructure;
using Chirp.Razor;
using Microsoft.EntityFrameworkCore;

namespace a;

public class ChirpDBContextUnitTests
{
    private ChirpDbContext Db;
        
    public ChirpDBContextUnitTests()
    {
        // Set up DbContextOptions for an in-memory SQLite database
        var options = new DbContextOptionsBuilder<ChirpDbContext>()
            .UseSqlite("Data Source=./data/ChirpDBContext.db")
            .Options;

        Db = new ChirpDbContext(options);
    }
    
    [Fact]
    public void DBContextContainsCheeps()
    {
        Assert.True(Db.Cheeps.Any());
    }

    [Fact]
    public void DBContextContainsAuthors()
    {
        Assert.True(Db.Authors.Any());
    }

    [Fact]
    public void QueryByCheepIdReturnsCheep()
    {
        CheepDTO cheepDto = Db.Cheeps.Find(1);
        
        Assert.NotNull(cheepDto);
        Assert.Equal(cheepDto.CheepId, 1);
        Assert.Equal(cheepDto.AuthorId, 10);
        Assert.Equal(cheepDto.Text, "They were married in Chicago, with old Smith, and was expected aboard every day; meantime, the two went past me.");
        Assert.Equal(cheepDto.TimeStamp, DateTime.Parse("2023-08-01 13:14:37"));
    }

    [Fact]
    public void QueryByAuthorIdReturnsAuthor()
    {
        AuthorDTO authorDto = Db.Authors
            .Include(a => a.Cheeps)
            .FirstOrDefault(a => a.AuthorId == 12);
    
        
        Assert.NotNull(authorDto);
        Assert.Equal(authorDto.AuthorId, 12);
        Assert.True(authorDto.Cheeps.Any(a => a.CheepId.Equals(657)));
    }
}