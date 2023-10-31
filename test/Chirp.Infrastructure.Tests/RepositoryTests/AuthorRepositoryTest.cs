using Chirp.Infrastructure.Repository;
using Chirp.Core.Entities; 
using Chirp.Infrastructure;
using Test_Utilities;

namespace Chirp.InfrastructureTests.RepositoryTests;

public class AuthorRepositoryTest
{

    private readonly ChirpDbContext context;

    public AuthorRepositoryTest()
    {
        context = SqliteInMemoryBuilder.GetContext();
    }

    [Fact]
    public void GetCheepsByPage_ShouldSkipFirst32Cheeps_ReturnXAmountOfCheeps()
    {
        var cheepRepository = new CheepRepository(context);

        for (int i = 0; i < 34; i++)
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
        

        //Assert
        
    }

    [Fact]
    public void DeleteCheepById_ShouldOnlyDeleteSpecifiedCheep()
    {

        // Arrange
        var cheepRepository = new CheepRepository(context);

        for (int i = 0; i < 34; i++)
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
        

        //Assert
        
    }

    [Fact]
    public void addAuthor_ShouldAddAuthorToDatabase()
    {
        //Arrange
        var authorRepository = new AuthorRepository(context);

        AuthorDTO authorDto1 = new AuthorDTO
        {
            AuthorId = Guid.NewGuid(),
            Name = "TestAuthor",
            Email = "Bob@Marley.com"
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

        // Act
        int initialAuthorCount = context.Authors.Count();
        
        AuthorDTO authorDto2 = new AuthorDTO
        {
            AuthorId = Guid.NewGuid(),
            Name = "TestAuthor2",
            Email = ""
        };
        CheepDTO cheepDto2 = new CheepDTO
        {
            CheepId = Guid.NewGuid(),
            AuthorId = authorDto2.AuthorId,
            Text = "TestCheep2",
            AuthorDto = authorDto2
        };
        
        authorRepository.AddAuthor(authorDto2);
        context.Cheeps.Add(cheepDto2);
        
        context.SaveChanges();
        
        int updatedAuthorCount = context.Authors.Count();
        
        // Assert
        Assert.Equal(initialAuthorCount + 1, updatedAuthorCount);
    }
}