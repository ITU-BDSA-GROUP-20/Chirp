//Test of cheep repository methods using Test_Utilites in-memory database

using Chirp.Infrastructure.Repository;
using Chirp.Core.Entities; 
using Chirp.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.Sqlite;
using Test_Utilities;

namespace Chirp.InfrastructureTests.RepositoryTests;
public class CheepRepositoryTest{

    private readonly ChirpDbContext context;

    public CheepRepositoryTest()
    {
        context = SqliteInMemoryBuilder.GetContext();
    }

    [Fact]
    public void GetCheepsByPage_ShouldSkipFirst32Cheeps_ReturnXAmountOfCheeps()
    {
        var cheepRepository = new CheepRepository(context);

        for(int i = 0; i < 34; i++)
        {

            AuthorDTO authorDto = new AuthorDTO
                { AuthorId = Guid.NewGuid(), Name = "TestAuthor" + i, Email = "mock@email.com" };
            CheepDTO cheepDto = new CheepDTO
            {
                CheepId = Guid.NewGuid(),
                AuthorId = authorDto.AuthorId,
                Text = "TestCheep" + i,
                AuthorDto = authorDto
            };
            
            context.Authors.Add(authorDto);
            context.Cheeps.Add(cheepDto);
        }

        context.SaveChanges();

        //Act
        ICollection<CheepDTO> cheeps = cheepRepository.GetCheepsByPage(1);

        //Assert
        Assert.Equal(2, cheeps.Count);
    }

    [Fact]
    public void DeleteCheepById_ShouldOnlyDeleteSpecifiedCheep(){
        
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

    