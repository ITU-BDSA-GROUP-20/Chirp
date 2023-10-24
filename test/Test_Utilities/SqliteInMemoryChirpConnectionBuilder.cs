using Microsoft.EntityFrameworkCore;

namespace Test_Utilities;


public class SqliteInMemoryChirpConnectionBuilder
{
    var _contextOptions = new DbContextOptionsBuilder<ChirpDbContext>()
        .UseInMemoryDatabase(databaseName: "Chirp")
        .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
        .Options;

    using var context = new ChirpDbContext(_contextOptions);

	public SqliteInMemoryChirpConnectionBuilder()
    {
		context.Database.EnsureDeleted();
        context.Database.EnsureCreated();
    }
    
    public void AddAuthor(AuthorDTO author)
    {
        context.Authors.Add(author);
        context.SaveChanges();
    }
    
    public void AddCheep(CheepDTO cheep)
    {
        context.Cheeps.Add(cheep);
        context.SaveChanges();
	}
    
    public ChirpDbContext GetContext()
    {
		context.Database.EnsureDeleted();
        context.Database.EnsureCreated();
        return context;
    }
}
