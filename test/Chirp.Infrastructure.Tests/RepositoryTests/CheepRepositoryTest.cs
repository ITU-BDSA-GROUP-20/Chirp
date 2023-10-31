//Test of cheep repository methods using Test_Utilites in-memory database

using Chirp.Infrastructure.Repository;
using Chirp.Core.Entities; 
using Chirp.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.Sqlite;
using Test_Utilities;

namespace Chirp.InfrastructureTests.RepositoryTests;
public class CheepRepositoryTest{

    private ChirpDbContext context;

    [Fact]
    public void GetCheepsByPage_ShouldSkipFirst32Cheeps_ReturnXAmountOfCheeps()
    {
        //Arrange
        using var connection = new SqliteConnection("Filename=:memory:");
        connection.Open();
        var builder = new DbContextOptionsBuilder<ChirpDbContext>().UseSqlite(connection);
        using var context = new ChirpDbContext(builder.Options);
        context.Database.EnsureCreatedAsync(); // Applies the schema to the database

        var cheepRepository = new CheepRepository(context, 32);

        for(int i = 0; i < 34; i++){
            context.Authors.Add(new AuthorDTO { AuthorId = i, Name = "TestAuthor" + i });
        }
        for(int i = 0; i < 34; i++){
            context.Cheeps.AddCheep(new CheepDTO { CheepId = i, AuthorId = i, Text = "TestCheep" + i });
        }

        context.SaveChanges();

        //Act
        ICollection<CheepDTO> cheeps = cheepRepository.GetCheepsByPage(2);

        //Assert
        Assert.Equal(3, cheeps.Count);
    }

    [Fact]
    public void DeleteCheepById_ShouldOnlyDeleteSpecifiedCheep(){
        
        //Arrange
        Db = SqliteInMemoryChirpConnectionBuilder.GetContext();

        // seed database with authors
        for(int i = 0; i < 34; i++){
            Db.Authors.Add(new AuthorDTO { AuthorId = i, Name = "TestAuthor" + i, Email = "bob@bob.dk" + 1, Cheeps = new List<CheepDTO>()});
        }

        for(int i = 0; i < 34; i++){
            Db.Cheeps.Add(new CheepDTO { CheepId = i, AuthorId = i, Text = "TestCheep" + i });
        }
        
        Db.SaveChanges();
        
        //Act
        int initialDbCount = Db.Cheeps.Count();
        int someId = Db.Cheeps.First().CheepId;

        var cheepRepository = new CheepRepository(Db, 32);
        cheepRepository.DeleteCheepById(someId);
        
        //Assert
        Assert.Equal(initialDbCount - 1, Db.Cheeps.Count());
        //Assert.Equal(1, cheepRepository.GetCheepsByPage(1).Count);
        //Assert.Equal(2, cheepRepository.GetCheepsByPage(1).First().CheepId);
    }

    [Fact]
    public void addCheep_ShouldAddACheep()
    {
        /*
        //Arrange
        var Db = SqliteInMemoryChirpConnectionBuilder.GetContext();

        var cheepRepository = new CheepRepository(Db, 32);

        //Act
        cheepRepository.AddCheep(new CheepDTO { CheepId = 1, AuthorId = 2, Text = "TestCheep" });
        
        //Assert
        
        Assert.Equal(1, cheepRepository.GetCheepsByPage(1).Count);
        Assert.Equal(1, cheepRepository.GetCheepsByPage(1).First().CheepId);
        Assert.Equal("TestCheep", cheepRepository.GetCheepsByPage(1).First().Text);
        Assert.Equal(2, cheepRepository.GetCheepsByPage(2).First().AuthorId);
        */
    }
}

    