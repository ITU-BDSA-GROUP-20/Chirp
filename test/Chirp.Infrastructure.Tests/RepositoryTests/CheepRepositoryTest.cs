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
        var cheepRepository = new CheepRepository(context);

        for(int i = 0; i < 34; i++)
        {

            AuthorDTO authorDto = new AuthorDTO
            { 
                AuthorId = Guid.NewGuid(), 
                Name = "TestAuthor" + i, 
                Email = "mock@email.com" 
            };
            
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
        
        var cheepRepository = new CheepRepository(context);
        
        for(int i = 0; i < 34; i++)
        {

            AuthorDTO authorDto = new AuthorDTO
            { 
                AuthorId = Guid.NewGuid(), 
                Name = "TestAuthor" + i, 
                Email = "mock@email.com" 
            };
            
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
        int initialCheepCount = context.Cheeps.Count();
        CheepDTO bob = context.Cheeps.FirstOrDefault();
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
        
        AuthorDTO authorDto1 = new AuthorDTO
        { 
            AuthorId = Guid.NewGuid(), 
            Name = "TestAuthor", 
            Email = "mock@email.com" 
        };
            
        CheepDTO cheepDto1 = new CheepDTO
        {
            CheepId = Guid.NewGuid(),
            AuthorId = authorDto1.AuthorId,
            Text = "TestCheep",
            AuthorDto = authorDto1
        };
            
        context.Authors.Add(authorDto1);
        context.Cheeps.Add(cheepDto1);
        
        context.SaveChanges();

        //Act
        int initialCheepCount = context.Cheeps.Count();
        
        AuthorDTO authorDto2 = new AuthorDTO
        { 
            AuthorId = Guid.NewGuid(), 
            Name = "TestAuthor", 
            Email = "mock@email.com" 
        };
            
        CheepDTO cheepDto2 = new CheepDTO
        {
            CheepId = Guid.NewGuid(),
            AuthorId = authorDto2.AuthorId,
            Text = "TestCheep",
            AuthorDto = authorDto2
        };
            
        context.Authors.Add(authorDto2);
        context.Cheeps.Add(cheepDto2);
        
        context.SaveChanges();

        int updatedCheepCount = context.Cheeps.Count();
        
        //Assert
        Assert.Equal(initialCheepCount + 1, updatedCheepCount);

    }
}