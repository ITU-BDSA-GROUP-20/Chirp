//Test of cheep repository methods using Test_Utilites in-memory database


using Chirp.Infrastructure.Repository;
using Chirp.Core; 
using Test_Utilities;

namespace Chirp.InfrastructureTests.RepositoryTests;
public class CheepRepositoryTest{

    [Fact]
    public void GetCheepsByPage_ShouldSkipFirst32Cheeps_ReturnXAmountOfCheeps()
    {
        //Arrange
        var context = SqliteInMemoryChirpConnectionBuilder.GetContext();
        var cheepRepository = new CheepRepository(context, 32);
         for(int i = 0; i < 34; i++){
            cheepRepository.AddCheep(new CheepDTO { CheepId = i, AuthorId = 2, Message = "TestCheep" + i });
        }
        //Act
        List<CheepDTO> cheeps = cheepRepository.GetCheepsByPage(2);

        //Assert
        Assert.Equal(3, cheeps.Count);

    }

    [Fact]
    public void DeleteCheepById_ShouldOnlyDeleteSpecifiedCheep(){
        //Arrange
        var context =SqliteInMemoryChirpConnectionBuilder.GetContext();
        var cheepRepository = new CheepRepository(context, 32);
        cheepRepository.AddCheep(new CheepDTO { CheepId = 1, AuthorId = 2, Message = "TestCheep" })
        cheepRepository.AddCheep(new CheepDTO { CheepId = 2, AuthorId = 2, Message = "TestCheep2" })
        //Act
        cheepRepository.DeleteCheepById(1);
        
        //Assert
        Assert.Equal(1, cheepRepository.GetCheepsByPage(1).Count);
        Assert.Equal(2, cheepRepository.GetCheepsByPage(1).CheepId);
    }

    [Fact]
    public void addCheep_ShouldAddACheep()
    {
        //Arrange
        var context =SqliteInMemoryChirpConnectionBuilder.GetContext();
        var cheepRepository = new CheepRepository(context, 32);
        //Act
        cheepRepository.addCheep(new CheepDTO { CheepId = 1, AuthorId = 2, Message = "TestCheep" });
        
        //Assert
        Assert.Equal(1, cheepRepository.GetCheepsByPage(1).Count);
        Assert.Equal(1, cheepRepository.GetCheepsByPage(1).CheepId);
        Assert.Equal("TestCheep", cheepRepository.GetCheepsByPage(1).Text);
        Assert.Equal(2, cheepRepository.GetCheepsByPage(2).AuthorId);
    }
    
    
    
}

    