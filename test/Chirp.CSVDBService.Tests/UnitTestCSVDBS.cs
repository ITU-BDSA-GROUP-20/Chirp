using CSVDBService;


namespace Chirp.CSVDBService.Tests;

public class UnitTestCSVDBS
{
    
    private IDatabaseRepository<Cheep> database = CSVDatabase<Cheep>.Instance;
    
    
    public void SetUp()
    {
        database.SetFilePath("./data/cheep-test");
        
    }
    public void TearDown()
    {
        File.Delete("./data/cheep-test");
    }
    
    //Methods to test:
    //read - 
    //store - 



    [Fact]
    public void Read_ShouldReadFirstCheepAuthorAndMessage()
    {
        SetUp();
        database.Store(new Cheep("TestAuthor", "Test, message", 12355431));

        List<Cheep> output = (List<Cheep>)database.Read(1);
        Assert.StartsWith("TestAuthor", output[0].Author);
        Assert.StartsWith("Test", output[0].Message);
        TearDown();
    }

    [Fact]
    public void Store_ShouldStoreExactNumberOfCheeps()
    {
        SetUp();

        database.Store(new Cheep("Auth1", "Message1", 1695728248));
        database.Store(new Cheep("Auth2", "Message2", 1695728260));

        List<Cheep> output =new(database.Read(2));
        Assert.Equal(2, output.Count);

        TearDown();
    }

   
    


    
    
}