using CSVDBService;


namespace Chirp.CSVDBService.Tests;

public class IntegrationTestCSVDBS
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
    
    //Stored cheep should be outputted correctly.. eh
    [Fact]
    public void Store_Read_ToStringOutput()
    {
        SetUp();

        Cheep cheep1 = new Cheep("Hermione", "Ron sucks", 1695756366);
        Cheep cheep2 = new Cheep("Ron", "Hermione sucks", 1695756333);
        
        database.Store(cheep1);
        database.Store(cheep2);
        
        List<Cheep> cheeps = new List<Cheep>(database.Read(2));
        
        Assert.Equal(cheeps[0].ToString(), cheep1.ToString());
        Assert.Equal(cheeps[1].ToString(), cheep2.ToString());
        /*
        @TODO:
        Due to Cheep.ToString() not being implemented, it is not possible to use the correctly formatted string,
        for now the default toString() method will be the one that is tested on.
        Assert.Equal($"{author} @ {timeFormatted} {message}", cheep.ToString());
        */
        TearDown();

    }
    
    

   
    


    
    
}