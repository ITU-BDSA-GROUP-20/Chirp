using Chirp.Core.Entities;
using Chirp.Infrastructure;
using Chirp.Infrastructure.Repository;
using Microsoft.IdentityModel.Tokens;
using Test_Utilities;
using Xunit.Abstractions;
using Xunit.Sdk;
using Timer = System.Timers.Timer;

namespace Chirp.InfrastructureTest.RepositoryTests;

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
        // Arrange
        var authorRepository = new AuthorRepository(context);
        
        // Created to test for the correct author
        Author theAuthor = null;
        
        // Create 3 authors
        for (int i = 0; i < 3; i++)
        {
            Author authorDto = new Author
            {
                Id = Guid.NewGuid(),
                UserName = "TestAuthor" + i,
                Email = "mock" + i + "@email.com"
            };
            
            // Set theAuthor to the middle author
            if (i == 1)
            {
                theAuthor = authorDto;
            }
      
            context.Users.Add(authorDto);
        }

        context.SaveChanges();

        //Act
        string theAuthorName = theAuthor.UserName;
        Author expectedAuthor = theAuthor;
        Author returnedAuthor = authorRepository.GetAuthorByName(theAuthorName);

        //Assert
        Assert.Equal(expectedAuthor, returnedAuthor);

    }

    [Fact]
    public void GetCheepsByAuthor_ShouldReturnCorrectCheeps()
    {

        //Arrange
        var authorRepository = new AuthorRepository(context);

        Author author1 = new Author
        {
            Id = Guid.NewGuid(),
            UserName = "TestAuthor",
            Email = "mock@email.com",
            Cheeps = new List<Cheep>()
        };
        Cheep cheep1 = new Cheep
        {
            CheepId = Guid.NewGuid(),
            AuthorId = author1.Id,
            Text = "TestCheep",
            Author = author1
        };

        context.Users.Add(author1);
        context.Cheeps.Add(cheep1);

        context.SaveChanges();

        //Act
        ICollection<Cheep> expectedCheep = new List<Cheep>();
        expectedCheep.Add(cheep1);

        ICollection<Cheep> returnedCheep = authorRepository.GetCheepsByAuthor(author1.Id, 0);

        //Assert
        Assert.Equal(expectedCheep, returnedCheep);
    }

    [Fact]
    public void addAuthor_ShouldAddAuthorToDatabase()
    {
        //Arrange
        var authorRepository = new AuthorRepository(context);
        
        Author authorDto1 = new Author
        {
            Id = Guid.NewGuid(),
            UserName = "TestAuthor",
            Email = "mock@email.com"
        };
        
        context.Users.Add(authorDto1);
        context.SaveChanges();
        
        // Note: authorDto2 is not yet added to context
        Author authorDto2 = new Author
        {
            Id = Guid.NewGuid(),
            UserName = "TestAuthor2",
            Email = "TestEmail2@test.com"
        };

        // Act
        int initialAuthorCount = context.Users.Count();
        
        authorRepository.AddAuthor(authorDto2);

        context.SaveChanges();

        int updatedAuthorCount = context.Users.Count();

        // Assert
        Assert.Equal(initialAuthorCount + 1, updatedAuthorCount);
    }

    [Fact]
    public void GetAuthorById_ShouldReturnCorrectAuthor()
    {
        // Arrange
        var authorRepository = new AuthorRepository(context);
        
        // Created to test for the correct author
        Author theAuthor = null;
        
        // Create 3 authors
        for (int i = 0; i < 3; i++)
        {
            Author authorDto = new Author
            {
                Id = Guid.NewGuid(),
                UserName = "TestAuthor" + i,
                Email = "mock" + i + "@email.com"
            };
            
            // Set theAuthor to the middle author
            if (i == 1)
            {
                theAuthor = authorDto;
            }

            context.Users.Add(authorDto);
        }

        context.SaveChanges();

        //Act
        Author expectedAuthor = theAuthor;
        Author returnedAuthor = authorRepository.GetAuthorById(theAuthor.Id);

        //Assert
        Assert.Equal(expectedAuthor, returnedAuthor);
    }

    [Fact]
    public void GetAuthorByEmail_ShouldReturnCorrectAuthor()
    {
        // Arrange
        var authorRepository = new AuthorRepository(context);
        
        // Created to test for the correct author
        Author theAuthor = null;
        
        // Create 3 authors
        for (int i = 0; i < 3; i++)
        {
            Author authorDto = new Author
            {
                Id = Guid.NewGuid(),
                UserName = "TestAuthor" + i,
                Email = "mock" + i + "@email.com"
            };
            
            // Set theAuthor to the middle author
            if (i == 1)
            {
                theAuthor = authorDto;
            }

            context.Users.Add(authorDto);
        }

        context.SaveChanges();

        //Act
        Author expectedAuthor = theAuthor;
        Author returnedAuthor = authorRepository.GetAuthorByEmail(theAuthor.Email);

        //Assert
        Assert.Equal(expectedAuthor, returnedAuthor);
    }
    
    [Fact]
    public void GetAuthorByIdAsync_ShouldReturnCorrectAuthor()
    {
        // Arrange
        var authorRepository = new AuthorRepository(context);

        // Created to test for the correct author
        Author theAuthor = null;

        // Create 3 authors
        for (int i = 0; i < 3; i++)
        {
            Author authorDto = new Author
            {
                Id = Guid.NewGuid(),
                UserName = "TestAuthor" + i,
                Email = "mock" + i + "@test.com"
            };

            // Set theAuthor to the middle author
            if (i == 1)
            {
                theAuthor = authorDto;
            }

            context.Users.Add(authorDto);
        }

        context.SaveChanges();

        //Act
        Author expectedAuthor = theAuthor;
        Author returnedAuthor = authorRepository.GetAuthorByIdAsync(theAuthor.Id).Result;

        //Assert
        Assert.Equal(expectedAuthor, returnedAuthor);
    }
    
    [Fact]
    public void AddFollowing_ShouldAddFollowingToAuthor()
    {
        // Arrange
        var authorRepository = new AuthorRepository(context);

        // Create 3 authors
        for (int i = 0; i < 2; i++)
        {
            Author authorDto = new Author
            {
                Id = Guid.NewGuid(),
                UserName = "TestAuthor" + i,
                Email = "mock" + i + "@test.com"
            };

            context.Users.Add(authorDto);
        }

        context.SaveChanges();

        //Act
        Author author1 = context.Users.Where(a => a.UserName == "TestAuthor0").FirstOrDefault();
        Author author2 = context.Users.Where(a => a.UserName == "TestAuthor1").FirstOrDefault();

        authorRepository.AddFollowing(author1, author2);

        //Assert
        Assert.Contains(author2, author1.Following);
        Assert.Contains(author1, author2.Followers);
    }

    [Fact]
    public async void RemoveFollowing_ShouldRemoveFollowingFromAuthor()
    {
        // Arrange
        var authorRepository = new AuthorRepository(context);

        // Create 3 authors
        for (int i = 0; i < 2; i++)
        {
            Author authorDto = new Author
            {
                Id = Guid.NewGuid(),
                UserName = "TestAuthor" + i,
                Email = "mock" + i + "@test.com"
            };

            context.Users.Add(authorDto);
        }

        context.SaveChanges();

        //Act
        Author author1 = context.Users.Where(a => a.UserName == "TestAuthor0").FirstOrDefault();
        Author author2 = context.Users.Where(a => a.UserName == "TestAuthor1").FirstOrDefault();

        authorRepository.AddFollowing(author1, author2);

        Assert.Contains(author2, author1.Following);
        Assert.Contains(author1, author2.Followers);

        await authorRepository.RemoveFollowing(author1, author2);

        await context.SaveChangesAsync();

        //Assert
        Assert.DoesNotContain(author2, author1.Following);
        Assert.DoesNotContain(author1, author2.Followers);
    }

    [Fact]
    public void GetFollowersByAuthor_ShouldReturnCorrectFollowers()
    {
        // Arrange
        var authorRepository = new AuthorRepository(context);

        // Create 3 authors
        for (int i = 0; i < 3; i++)
        {
            Author authorDto = new Author
            {
                Id = Guid.NewGuid(),
                UserName = "TestAuthor" + i,
                Email = "mock" + i + "@test.com"
            };

            context.Users.Add(authorDto);
        }

        context.SaveChanges();

        //Act
        Author author1 = context.Users.Where(a => a.UserName == "TestAuthor0").FirstOrDefault();
        Author author2 = context.Users.Where(a => a.UserName == "TestAuthor1").FirstOrDefault();
        Author author3 = context.Users.Where(a => a.UserName == "TestAuthor2").FirstOrDefault();

        authorRepository.AddFollowing(author2, author1);
        authorRepository.AddFollowing(author3, author1);

        ICollection<Author> expectedFollowers = new List<Author>();
        expectedFollowers.Add(author2);
        expectedFollowers.Add(author3);

        ICollection<Author> returnedFollowers = authorRepository.GetFollowersByAuthor(author1.Id);

        //Assert
        Assert.Equal(expectedFollowers, returnedFollowers);
    }

    [Fact]
    public void GetFollowingByAuthor_ShouldReturnCorrectFollowing()
    {
        // Arrange
        var authorRepository = new AuthorRepository(context);

        // Create 3 authors
        for (int i = 0; i < 3; i++)
        {
            Author authorDto = new Author
            {
                Id = Guid.NewGuid(),
                UserName = "TestAuthor" + i,
                Email = "mock" + i + "@test.com"
            };

            context.Users.Add(authorDto);
        }

        context.SaveChanges();

        //Act
        Author author1 = context.Users.Where(a => a.UserName == "TestAuthor0").FirstOrDefault();
        Author author2 = context.Users.Where(a => a.UserName == "TestAuthor1").FirstOrDefault();
        Author author3 = context.Users.Where(a => a.UserName == "TestAuthor2").FirstOrDefault();

        authorRepository.AddFollowing(author1, author2);
        authorRepository.AddFollowing(author1, author3);

        ICollection<Author> expectedFollowing = new List<Author>();
        expectedFollowing.Add(author2);
        expectedFollowing.Add(author3);

        ICollection<Author> returnedFollowing = authorRepository.GetFollowingByAuthor(author1.Id);

        //Assert
        Assert.Equal(expectedFollowing, returnedFollowing);
    }

    [Fact]
    public async void RemoveAllFollowRelationsById_ShouldRemoveFollowersAndFollowing()
    {
        // Arrange
        var authorRepository = new AuthorRepository(context);

        // Create 3 authors
        for (int i = 0; i < 3; i++)
        {
            Author authorDto = new Author
            {
                Id = Guid.NewGuid(),
                UserName = "TestAuthor" + i,
                Email = "mock" + i + "@test.com"
            };

            context.Users.Add(authorDto);
        }

        context.SaveChanges();

        //Act 1
        Author author1 = context.Users.Where(a => a.UserName == "TestAuthor0").FirstOrDefault();
        Author author2 = context.Users.Where(a => a.UserName == "TestAuthor1").FirstOrDefault();
        Author author3 = context.Users.Where(a => a.UserName == "TestAuthor2").FirstOrDefault();
        
        // Author1 follows author2 and author3
        await authorRepository.AddFollowing(author1, author2);
        await authorRepository.AddFollowing(author1, author3);
        
        // Author2 follows author1
        await authorRepository.AddFollowing(author2, author1);

        ICollection<Follow> expectedFollowing = new List<Follow>();
        expectedFollowing.Add(author2);
        expectedFollowing.Add(author3);
        
        ICollection<Follow> expectedFollowers = new List<Follow>();
        expectedFollowers.Add(author2);

        ICollection<Follow> returnedFollowing = authorRepository.GetFollowingByAuthor(author1.Id);
        ICollection<Follow> returnedFollowers = authorRepository.GetFollowersByAuthor(author1.Id);

        //Assert 1
        Assert.Equal(expectedFollowing, returnedFollowing);
        Assert.Equal(expectedFollowers, returnedFollowers);
        
        //Act 2
        await authorRepository.RemoveAllFollowRelationsById(author1.Id);
        
        //Assert 2
        Assert.True(author1.Followers.Count == 0);
        Assert.True(author2.Following.Count == 0);
        Assert.True(author2.Followers.Count == 0);
        Assert.True(author3.Followers.Count == 0);
        
        Assert.True(author1.Following.Count == 0);
        
    }
}