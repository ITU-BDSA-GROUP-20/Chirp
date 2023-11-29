//Test of cheep repository methods using Test_Utilites in-memory database

using Chirp.Core.Entities;
using Chirp.Infrastructure;
using Chirp.Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;
using Test_Utilities;

namespace Chirp.InfrastructureTest.RepositoryTests;
public class CheepRepositoryTest{

    private readonly CheepRepository CheepRepository;
    private readonly ChirpDbContext db;

    public CheepRepositoryTest()
    {
        db = SqliteInMemoryBuilder.GetContext();
        CheepRepository = new CheepRepository(db);
        
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
            
            db.Users.Add(authorDto);
            db.Cheeps.Add(cheepDto);
        }

        db.SaveChanges();
    }

    [Fact]
    public void GetCheepsByPage_ShouldSkipFirst32Cheeps_ReturnXAmountOfCheeps()
    {
        //Act
        ICollection<Cheep> cheeps = CheepRepository.GetCheepsByPage(1);

        //Assert
        Assert.Equal(2, cheeps.Count);
    }

    [Fact]
    public void DeleteCheepById_ShouldOnlyDeleteSpecifiedCheep()
    {
        ICollection<Cheep> initialCheeps = CheepRepository.GetCheepsByPage(0);
        Cheep cheep = initialCheeps.First();
        Guid cheepId = cheep.CheepId;
        
        CheepRepository.DeleteCheepById(cheepId);

        ICollection<Cheep> updatedCheeps = CheepRepository.GetCheepsByPage(0);
        
        //Assert
        Assert.True(initialCheeps.Contains(cheep));
        Assert.False(updatedCheeps.Contains(cheep));

    }

    [Fact]
    public void addCheep_ShouldAddACheep()
    {
        Author authorDto1 = new Author
        { 
            UserName = "TestAuthor", 
            Email = "mock@email.com" 
        };
        
        Cheep cheepDto = new Cheep
        {
            CheepId = Guid.NewGuid(),
            AuthorId = authorDto1.Id,
            Text = "TestCheep",
            TimeStamp = DateTime.Now,
            Author = authorDto1
        };
            
        db.Users.Add(authorDto1);
        db.SaveChanges();

        CheepRepository.AddCheep(cheepDto).Wait();

        ICollection<Cheep> updatedCheeps = CheepRepository.GetCheepsByPage(0);
        
        //Assert
        Assert.True(updatedCheeps.Contains(cheepDto));
    }
}