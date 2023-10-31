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
    public void GetAuthorByName_ShouldReturnCorrectAuthorDTO()
    {
        var authorRepository = new AuthorRepository(context);
        
        // Created to test for the correct author
        AuthorDTO theAuthor = null;
        
        // Create 3 authors
        for (int i = 0; i < 3; i++)
        {
            AuthorDTO authorDto = new AuthorDTO
            {
                AuthorId = Guid.NewGuid(),
                Name = "TestAuthor" + i,
                Email = "mock" + i + "@email.com"
            };
            
            // Set theAuthor to the middle author
            if (i == 1)
            {
                theAuthor = authorDto;
            }

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
        AuthorDTO expectedAuthor = theAuthor;
        AuthorDTO returnedAuthor = authorRepository.GetAuthorByName("TestAuthor5");

        //Assert
        Assert.Equal(expectedAuthor, returnedAuthor);

    }

    [Fact]
    public void GetCheepsByAuthor_ShouldReturnCorrectCheeps()
    {

        //Arrange
        var authorRepository = new AuthorRepository(context);

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
        ICollection<CheepDTO> expectedCheep = new List<CheepDTO>();
        expectedCheep.Add(cheepDto1);

        ICollection<CheepDTO> returnedCheep = authorRepository.GetCheepsByAuthor(authorDto1.Name, 0);

        //Assert
        Assert.Equal(expectedCheep, returnedCheep);
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

        // Act
        int initialAuthorCount = context.Authors.Count();

        AuthorDTO authorDto2 = new AuthorDTO
        {
            AuthorId = Guid.NewGuid(),
            Name = "TestAuthor2",
            Email = "TestEmail2@test.com"
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

    [Fact]
    public void GetAuthorById_ShouldReturnCorrectAuthor()
    {
        var authorRepository = new AuthorRepository(context);
        
        // Created to test for the correct author
        AuthorDTO theAuthor = null;
        Guid theAuthorId = new Guid();
        
        // Create 3 authors
        for (int i = 0; i < 3; i++)
        {
            AuthorDTO authorDto = new AuthorDTO
            {
                AuthorId = Guid.NewGuid(),
                Name = "TestAuthor" + i,
                Email = "mock" + i + "@email.com"
            };
            
            // Set theAuthor to the middle author
            if (i == 1)
            {
                theAuthor = authorDto;
            }

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
        AuthorDTO expectedAuthor = theAuthor;
        AuthorDTO returnedAuthor = authorRepository.GetAuthorById(theAuthorId);

        //Assert
        Assert.Equal(expectedAuthor, returnedAuthor);
    }

    [Fact]
    public void GetAuthorByEmail_ShouldReturnCorrectAuthor()
    {
        var authorRepository = new AuthorRepository(context);
        
        // Created to test for the correct author
        AuthorDTO theAuthor = null;
        String theAuthorEmail = null;
        
        // Create 3 authors
        for (int i = 0; i < 3; i++)
        {
            AuthorDTO authorDto = new AuthorDTO
            {
                AuthorId = Guid.NewGuid(),
                Name = "TestAuthor" + i,
                Email = "mock" + i + "@email.com"
            };
            
            // Set theAuthor to the middle author
            if (i == 1)
            {
                theAuthor = authorDto;
            }

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
        AuthorDTO expectedAuthor = theAuthor;
        AuthorDTO returnedAuthor = authorRepository.GetAuthorByEmail(theAuthorEmail);

        //Assert
        Assert.Equal(expectedAuthor, returnedAuthor);
    }
}