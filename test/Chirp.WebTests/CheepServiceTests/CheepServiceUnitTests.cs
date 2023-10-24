using Chirp.Core.Entities;
using Chirp.Infrastructure;
using Chirp.Web;
using Microsoft.EntityFrameworkCore;

namespace Chirp.WebTests.CheepServiceTests;

public class CheepServiceUnitTests
{
    //TODO integrate the databasefactory to provide a database;
    private readonly ICheepService _service = new CheepService(new ChirpDbContext(new DbContextOptions<ChirpDbContext>()), 32);
    
    [Fact]
    public void GetCheepsReturnsSomething()
    {
        //Arrange & Act
        ICollection<CheepViewModel> cheeps = _service.GetCheeps(1);
        
        //Assert
        Assert.NotNull(cheeps);
    }
    
    [Fact]
    public void GetCheepsReturnsProperPageSize()
    {
        ICollection<CheepViewModel> cheeps = _service.GetCheeps(1);
        
        Assert.Equal(32, cheeps.Count);
    }
    
    [Fact]
    public void GetCheepsReturnCorrectlyFormattedCheep()
    {
        /*ICollection<CheepViewModel> cheeps = _service.GetCheeps(1);
        
        CheepViewModel cheep = cheeps[0];

        foreach (CheepViewModel cheep in cheeps)
        {
            
            break;
        }
        
        Assert.Equal(cheep.Author, "Quintin Sitts");
        Assert.Equal(cheep.Message, "For then, more whales the less to her, as you very much.");
        Assert.Equal(cheep.Timestamp, "2023-08-01 13:14:07");*/
    }
    
    [Fact]
    public void GetCheepsReturnsAllValidCheeps()
    {
        ICollection<CheepViewModel> cheeps = _service.GetCheeps(1);

        foreach (CheepViewModel cheep in cheeps)
        {
            Assert.NotNull(cheep.Author);
            Assert.NotNull(cheep.Message);
            Assert.NotNull(cheep.Timestamp);
            Assert.NotEmpty(cheep.Author);
            Assert.NotEmpty(cheep.Message);
            Assert.NotEmpty(cheep.Timestamp);
        }
    }
    
    [Fact]
    public void GetCheepsFromAuthorReturnsSomething()
    {
        Assert.Fail("WIP");
    }
    
    [Fact]
    public void GetCheepsFromAuthorReturnsCheepsFromCorrectAuthor()
    {
        Assert.Fail("WIP");
    }
    
    [Fact]
    public void GetCheepsFromAuthorIsPaginated()
    {
        Assert.Fail("WIP");
    }
    
    [Fact]
    public void GetCheepsFromAuthorReturnsAllValidCheeps()
    {
        Assert.Fail("WIP");
    }
}