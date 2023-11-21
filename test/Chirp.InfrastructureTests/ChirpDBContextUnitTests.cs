using Chirp.Core.Entities;
using Chirp.Core.Repository;
using Chirp.Infrastructure;
using Chirp.Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;
using Test_Utilities;

namespace Chirp.Razor.Tests;

public class ChirpDBContextUnitTests
{

    private readonly ChirpDbContext Db;
    private readonly Author Author1;
    private readonly Author Author2;
    private readonly Cheep Cheep1;
    private readonly Cheep Cheep2;

    public ChirpDBContextUnitTests()
    {
        Db = SqliteInMemoryBuilder.GetContext();
        IAuthorRepository authorRepository = new AuthorRepository(Db);
        ICheepRepository cheepRepository = new CheepRepository(Db);

        // Mock data
        Author1 = new Author { Id = Guid.NewGuid(), UserName = "Author1", Email = "email1" };
        Author2 = new Author { Id = Guid.NewGuid(), UserName = "Author2", Email = "email2" };

        Cheep1 = new Cheep
        {
            CheepId = Guid.NewGuid(),
            Author = Author1,
            AuthorId = Author1.Id,
            Text = "Cheep 1",
            TimeStamp = DateTime.Now
        };
        Cheep2 = new Cheep
        {
            CheepId = Guid.NewGuid(), Author = Author2, AuthorId = Author2.Id, Text = "Cheep 2",
            TimeStamp = DateTime.Now
        };
        
        authorRepository.AddAuthor(Author1);
        authorRepository.AddAuthor(Author2);
        cheepRepository.AddCheep(Cheep1);
        cheepRepository.AddCheep(Cheep2);
    }
    
    
    [Fact]
    public void DBContextContainsCheeps()
    {
        Assert.True(Db.Cheeps.Any());
    }

    [Fact]
    public void DBContextContainsAuthors()
    {
        Assert.True(Db.Users.Any());
    }

    [Fact]
    public void QueryByCheepIdReturnsCheep()
    {
        
        Cheep returnedCheep = Db.Cheeps.Find(Cheep1.CheepId);
        
        Assert.NotNull(returnedCheep);
        Assert.Equal(Cheep1.CheepId, returnedCheep.CheepId);
        Assert.Equal(Cheep1.AuthorId, returnedCheep.AuthorId);
        Assert.Equal(Cheep1.Text, returnedCheep.Text);
        Assert.Equal(Cheep1.TimeStamp, returnedCheep.TimeStamp);
    }

    [Fact]
    public void QueryByAuthorIdReturnsAuthor()
    {

        Db.Cheeps.Include(e => e.Author);
        
        Author returnedAuthor = Db.Users.Find(Author1.Id);
        
        Assert.NotNull(returnedAuthor);
        Assert.Equal(returnedAuthor.Id, Author1.Id);
        Assert.True(returnedAuthor.Cheeps.Any());
    }
}