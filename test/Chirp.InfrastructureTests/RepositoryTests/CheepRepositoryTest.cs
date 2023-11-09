//Test of cheep repository methods using Test_Utilites in-memory database

using Chirp.Infrastructure.Repository;
using Chirp.Core.Entities; 
using Chirp.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.Sqlite;
using Test_Utilities;
using Microsoft.EntityFrameworkCore.Infrastructure;

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
        //Arrange
        var cheepRepository = new CheepRepository(context);

        for(int i = 0; i < 34; i++)
        {

            Author authorDto = new Author
            { 
                UserName = "TestAuthor" + i, 
                Email = "mock" + i + "@email.com" 
            };
            
            Cheep cheepDto = new Cheep
            {
                CheepId = Guid.NewGuid(),
                AuthorId = authorDto.Id,
                Text = "TestCheep" + i,
                Author = authorDto
            };
            
            context.Users.Add(authorDto);
            context.Cheeps.Add(cheepDto);
        }

        context.SaveChanges();

        //Act
        ICollection<Cheep> cheeps = cheepRepository.GetCheepsByPage(1);

        //Assert
        Assert.Equal(2, cheeps.Count);
    }

    [Fact]
    public void DeleteCheepById_ShouldOnlyDeleteSpecifiedCheep(){
        
        //Arrange
        var cheepRepository = new CheepRepository(context);
        
        for(int i = 0; i < 3; i++)
        {
            Author authorDto = new Author
            { 
                UserName = "TestAuthor" + i, 
                Email = "mock" + i + "@email.com" 
            };
            
            Cheep cheepDto = new Cheep
            {
                CheepId = Guid.NewGuid(),
                AuthorId = authorDto.Id,
                Text = "TestCheep" + i,
                Author = authorDto
            };
            
            context.Users.Add(authorDto);
            context.Cheeps.Add(cheepDto);
        }

        context.SaveChanges();
        
        //Act
        int initialCheepCount = context.Cheeps.Count();
        Cheep bob = context.Cheeps.FirstOrDefault();
        Guid cheepId = bob.CheepId;
        

        cheepRepository.DeleteCheepById(cheepId);

        int updatedCheepCount = context.Cheeps.Count();
        
        //Assert
        Assert.Equal(initialCheepCount - 1, updatedCheepCount);

    }

    [Fact]
    public void addCheep_ShouldAddACheep()
    {
        //Arrange
        var cheepRepository = new CheepRepository(context);
        
        Author authorDto1 = new Author
        { 
            UserName = "TestAuthor", 
            Email = "mock@email.com" 
        };
            
        Cheep cheepDto1 = new Cheep
        {
            CheepId = Guid.NewGuid(),
            AuthorId = authorDto1.Id,
            Text = "TestCheep",
            Author = authorDto1
        };
            
        context.Users.Add(authorDto1);
        context.Cheeps.Add(cheepDto1);
        
        context.SaveChanges();

        //Act
        int initialCheepCount = context.Cheeps.Count();
        
        Author authorDto2 = new Author
        { 
            UserName = "TestAuthor", 
            Email = "mock1@email.com" 
        };
            
        Cheep cheepDto2 = new Cheep
        {
            CheepId = Guid.NewGuid(),
            AuthorId = authorDto2.Id,
            Text = "TestCheep",
            Author = authorDto2
        };
            
        context.Users.Add(authorDto2);
        context.Cheeps.Add(cheepDto2);
        
        context.SaveChanges();

        int updatedCheepCount = context.Cheeps.Count();
        
        //Assert
        Assert.Equal(initialCheepCount + 1, updatedCheepCount);
    }

   
    
}