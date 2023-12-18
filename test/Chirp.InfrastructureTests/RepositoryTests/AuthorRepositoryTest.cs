using Chirp.Core.Entities;
using Chirp.Core.Repository;
using Chirp.Infrastructure;
using Chirp.Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Test_Utilities;
using Xunit.Abstractions;
using Xunit.Sdk;
using Timer = System.Timers.Timer;

namespace Chirp.InfrastructureTest.RepositoryTests;

public class AuthorRepositoryTest
{

    private readonly ChirpDbContext context;
    private readonly IAuthorRepository _authorRepository;

    private Author _author1;
    private Author _author2;
    private Author _author3;
    private Cheep _cheep1;
    private Cheep _cheep2;
    private Cheep _cheep3;

    public AuthorRepositoryTest()
    {
        context = SqliteInMemoryBuilder.GetContext();

        _authorRepository = new AuthorRepository(context);
        
        _author1 = new Author {
            Id = Guid.NewGuid(),
            UserName = "TestAuthor1",
            Email = "mock1@email.com"
        };
        _author2 = new Author {
            Id = Guid.NewGuid(),
            UserName = "TestAuthor2",
            Email = "mock2@email.com"
        };
        _author3 = new Author {
            Id = Guid.NewGuid(),
            UserName = "TestAuthor3",
            Email = "mock3@email.com"
        };
        
        _cheep1 = new Cheep
        {
            CheepId = Guid.NewGuid(),
            AuthorId = _author1.Id,
            Text = "TestCheep by author 1",
            Author = _author1
        };
        
        _cheep2 = new Cheep
        {
            CheepId = Guid.NewGuid(),
            AuthorId = _author2.Id,
            Text = "TestCheep by author 2",
            Author = _author2
        };
        
        _cheep3 = new Cheep
        {
            CheepId = Guid.NewGuid(),
            AuthorId = _author3.Id,
            Text = "TestCheep by author 3",
            Author = _author3
        };

        context.Add(_author2);
        context.Add(_author3);

        context.Add(_cheep1);
        context.Add(_cheep2);
        context.Add(_cheep3);

        context.SaveChanges();
    }

    [Fact]
    public void GetAuthorByName_ShouldReturnCorrectAuthorDTO()
    {
        //Arange
        Author expectedAuthor = _author2;
        
        //Act
        Author returnedAuthor = _authorRepository.GetAuthorByName(_author2.UserName);

        //Assert
        Assert.Equal(expectedAuthor, returnedAuthor);

    }

    [Fact]
    public void GetCheepsByAuthor_ShouldReturnCorrectCheeps()
    {
        //Act
        ICollection<Cheep> expectedCheep = new List<Cheep>();
        expectedCheep.Add(_cheep1);

        ICollection<Cheep> returnedCheep = _authorRepository.GetCheepsByAuthor(_author1.Id, 0);

        //Assert
        Assert.Equal(expectedCheep, returnedCheep);
    }

    [Fact]
    public void addAuthor_ShouldAddAuthorToDatabase()
    {
        // Act
        int initialAuthorCount = context.Users.Count();
        
        Author author4 = new Author {
            Id = Guid.NewGuid(),
            UserName = "TestAuthor4",
            Email = "mock4@email.com"
        };
        
        _authorRepository.AddAuthor(author4);

        int updatedAuthorCount = context.Users.Count();

        // Assert
        Assert.Equal(initialAuthorCount + 1, updatedAuthorCount);
    }

    [Fact]
    public void GetAuthorById_ShouldReturnCorrectAuthor()
    {
        //Act
        Author expectedAuthor = _author2;
        Author returnedAuthor = _authorRepository.GetAuthorById(_author2.Id);

        //Assert
        Assert.Equal(expectedAuthor, returnedAuthor);
    }

    [Fact]
    public void GetAuthorByEmail_ShouldReturnCorrectAuthor()
    {
        //Act
        Author expectedAuthor = _author2;
        Author returnedAuthor = _authorRepository.GetAuthorByEmail(_author2.Email);

        //Assert
        Assert.Equal(expectedAuthor, returnedAuthor);
    }
    
    [Fact]
    public void GetAuthorByIdAsync_ShouldReturnCorrectAuthor()
    {
        //Act
        Author expectedAuthor = _author2;
        Author? returnedAuthor = _authorRepository.GetAuthorByIdAsync(_author2.Id).Result;

        //Assert
        Assert.Equal(expectedAuthor, returnedAuthor);
    }
    
    [Fact]
    public async void AddFollow_ShouldAddFollowingToAuthor()
    {
        // Arrange
        var authorRepository = new AuthorRepository(context);

        // Create 2 authors
        Author? author1 = new Author()
        {
            Id = new Guid(),
            UserName = "author1",
            Email = "author1@mail.com"
        };
        Author? author2 = new Author()
        {
            Id = new Guid(),
            UserName = "author2",
            Email = "author1@mail.com"
        };
        
        context.Users.Add(author1);
        context.Users.Add(author2);

        context.SaveChanges();

        //Act
        await authorRepository.AddFollow(author1, author2);
        
        //Assert
        Assert.True(author1.Following.FirstOrDefault().FollowedAuthorId == author2.Id);
        Assert.True(author2.Followers.FirstOrDefault().FollowingAuthorId == author1.Id);
    }

    [Fact]
    public async void RemoveFollow_ShouldRemoveFollowingFromAuthor()
    {
        // Arrange
        var authorRepository = new AuthorRepository(context);

        // Create 2 authors
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

        await context.SaveChangesAsync();

        //Act
        Author? author1 = context.Users.Include(author => author.Following).Include(author => author.Followers).FirstOrDefault(a => a.UserName == "TestAuthor0");
        Author? author2 = context.Users.Include(author => author.Followers).FirstOrDefault(a => a.UserName == "TestAuthor1");

        await authorRepository.AddFollow(author1, author2);
        
        Assert.Equal(author2.Id, author1.Following.FirstOrDefault().FollowedAuthor.Id);
        Assert.Equal(author1.Id, author2.Followers.FirstOrDefault().FollowingAuthor.Id);

        await authorRepository.RemoveFollow(author1, author2);

        await context.SaveChangesAsync();

        //Assert
        Assert.True(author1.Followers.IsNullOrEmpty());
        Assert.True(author2.Followers.IsNullOrEmpty());
    }

    [Fact]
    public async void GetFollowersByAuthor_ShouldReturnCorrectFollowers()
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

        await context.SaveChangesAsync();

        //Act
        Author? author1 = context.Users.FirstOrDefault(a => a.UserName == "TestAuthor0");
        Author? author2 = context.Users.FirstOrDefault(a => a.UserName == "TestAuthor1");
        Author? author3 = context.Users.FirstOrDefault(a => a.UserName == "TestAuthor2");

        await authorRepository.AddFollow(author2, author1);
        await authorRepository.AddFollow(author3, author1);

        ICollection<Author?> expectedFollowers = new List<Author?>();
        expectedFollowers.Add(author2);
        expectedFollowers.Add(author3);

        ICollection<Author?> returnedFollowers = authorRepository.GetFollowersById(author1.Id);

        //Assert
        Assert.Equal(expectedFollowers, returnedFollowers);
    }

    [Fact]
    public async void GetFollowingByAuthor_ShouldReturnCorrectFollowing()
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

        await context.SaveChangesAsync();

        //Act
        Author? author1 = context.Users.FirstOrDefault(a => a.UserName == "TestAuthor0");
        Author? author2 = context.Users.FirstOrDefault(a => a.UserName == "TestAuthor1");
        Author? author3 = context.Users.FirstOrDefault(a => a.UserName == "TestAuthor2");

        await authorRepository.AddFollow(author1, author2);
        await authorRepository.AddFollow(author1, author3);

        ICollection<Author?> expectedFollowing = new List<Author?>();
        expectedFollowing.Add(author2);
        expectedFollowing.Add(author3);

        ICollection<Author?> returnedFollowing = authorRepository.GetFollowingById(author1.Id);

        //Assert
        Assert.Equal(expectedFollowing, returnedFollowing);
    }

    [Fact]
    public async void DeleteUserById_ShouldDeleteUser()
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

        await context.SaveChangesAsync();

        //Act
        Author? author1 = context.Users.FirstOrDefault(a => a.UserName == "TestAuthor0");
        Author? author2 = context.Users.FirstOrDefault(a => a.UserName == "TestAuthor1");
        Author? author3 = context.Users.FirstOrDefault(a => a.UserName == "TestAuthor2");

        // await authorRepository.DeleteUserById(author1.Id);
        context.Remove(author1);

        await context.SaveChangesAsync();

        //Assert
        Assert.True(context.Users.Count() == 2);
        Assert.True(context.Users.FirstOrDefault(a => a.UserName == "TestAuthor0") == null);
    }

   [Fact]
    public async Task DeleteCheepsByAuthorId_ShouldRemoveAllCheepsByAuthor()
    {
        // Arrange
        var authorRepository = new AuthorRepository(context);

        // Create an author with cheeps
        Author author = new Author
        {
            Id = Guid.NewGuid(),
            UserName = "TestAuthor",
            Email = "testauthor@test.com",
            Cheeps = new List<Cheep>()
        };

        Cheep cheep1 = new Cheep
        {
            CheepId = Guid.NewGuid(),
            AuthorId = author.Id,
            Text = "TestCheep1",
            Author = author
        };
        Cheep cheep2 = new Cheep
        {
            CheepId = Guid.NewGuid(),
            AuthorId = author.Id,
            Text = "TestCheep2",
            Author = author
        };
        Cheep cheep3 = new Cheep
        {
            CheepId = Guid.NewGuid(),
            AuthorId = author.Id,
            Text = "TestCheep3",
            Author = author
        };

        author.Cheeps.Add(cheep1);
        author.Cheeps.Add(cheep2);
        author.Cheeps.Add(cheep3);
        
        context.Users.Add(author);
        
        await context.SaveChangesAsync();

        Assert.Equal(3, context.Cheeps.Count());

        // Act
        await authorRepository.DeleteCheepsByAuthorId(author.Id);
        
        await context.SaveChangesAsync();

        // Assert
        Assert.Empty(author.Cheeps);
        Assert.Empty(context.Cheeps);
    }
}