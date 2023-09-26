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
    [Theory]
    [InlineData("Hermione", "Ron sucks", 1695756366)]
    [InlineData("Ron", "Hermione sucks", 1695756333)]
    public void Store_Read_ToStringOutput(string author, string message, long timestamp)
    {
        SetUp();
        database.Store(new Cheep(author, message, timestamp));

        //DateTime time = DateTimeOffset.FromUnixTimeSeconds(timestamp).LocalDateTime;
        //string timeFormatted = time.ToString();
        
        Cheep cheep = database.Read(1).First();
        // "Cheep { Author = {author}, Message = {message}, Timestamp = {timestamp} }"
        
        Assert.Equal($"Cheep {{ Author = {author}, Message = {message}, Timestamp = {timestamp} }}", cheep.ToString());
        /*
        @TODO:
        Due to Cheep.ToString() not being implemented, it is not possible to use the correctly formatted string,
        for now the default toString() method will be the one that is tested on.
        Assert.Equal($"{author} @ {timeFormatted} {message}", cheep.ToString());
        */
        TearDown();

    }
    
    

   
    


    
    
}