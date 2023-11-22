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
    private readonly CreateCheep Cheep1;
    private readonly CreateCheep Cheep2;
    private IAuthorRepository authorRepository;
    private ICheepRepository cheepRepository;

    public ChirpDBContextUnitTests()
    {
        Db = SqliteInMemoryBuilder.GetContext();
        authorRepository = new AuthorRepository(Db);
        cheepRepository = new CheepRepository(Db);

        // Mock data
        Author1 = new Author { Id = Guid.NewGuid(), UserName = "Author1", Email = "email1" };
        Author2 = new Author { Id = Guid.NewGuid(), UserName = "Author2", Email = "email2" };

        Cheep1 = new CreateCheep(Author1, "Cheep 1");
        Cheep2 = new CreateCheep(Author2, "Cheep 2");
        
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
        Cheep cheep = cheepRepository.CreateCheep2Cheep(Cheep1);
        Cheep returnedCheep = Db.Cheeps.Find(cheep.CheepId);
        
        Assert.NotNull(returnedCheep);
        Assert.Equal(cheep.CheepId, returnedCheep.CheepId);
        Assert.Equal(cheep.AuthorId, returnedCheep.AuthorId);
        Assert.Equal(cheep.Text, returnedCheep.Text);
        Assert.Equal(cheep.TimeStamp, returnedCheep.TimeStamp);
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