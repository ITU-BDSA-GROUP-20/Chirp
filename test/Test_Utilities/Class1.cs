namespace Test_Utilities;


public class SqliteInMemoryChirpConnectionBuilder
{
    var _contextOptions = new DbContextOptionsBuilder<ChirpDbContext>()
        .UseInMemoryDatabase(databaseName: "Chirp")
        .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
        .Options;
    using var context = new ChirpDbContext(_contextOptions);
    
    public ChirpDbContext GetContext()
    {
        return context;
    }
}