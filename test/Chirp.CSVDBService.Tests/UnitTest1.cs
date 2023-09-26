using CSVDBService;


namespace Chirp.CSVDBService.Tests;

public class UnitTest1
{
    
    private IDatabaseRepository<Cheep> database = CSVDatabase<Cheep>.Instance;
    
    [Fact]
    public void Test1()
    {
        database.SetFilePath("./data/cheep-test");
        
        database.Store(new Cheep("Hej", "hej", 12355431));
            
        File.Delete("./data/cheep-test");
    }
}