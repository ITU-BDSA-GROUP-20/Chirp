//Test of cheep repository methods using Test_Utilites in-memory database


using Chirp.Infrastructure.Repository;
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
        Assert.Equal(1, cheepRepository.GetCheepsByPage(1).Count)
    }
    
    
}

    