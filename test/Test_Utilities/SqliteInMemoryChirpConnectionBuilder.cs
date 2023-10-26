using Microsoft.EntityFrameworkCore;
using Chirp.Core;
using Chirp.Infrastructure;
using Microsoft.VisualBasic;
using Microsoft.Data.Sqlite;

namespace Test_Utilities{
public class SqliteInMemoryChirpConnectionBuilder
{   
    //This class is used to create an in memory database for testing purposes
    //Create a new instance of this class in your test class,
    //then call the GetContext() method to get a new instance of the ChirpDbContext
   
    public SqliteInMemoryChirpConnectionBuilder()
    {   var context = GetContext();
		context.Database.EnsureDeleted();
        context.Database.EnsureCreated();
    }
    public static ChirpDbContext GetContext()
    {
        var connection = new SqliteConnection("Filename=:memory:");
        connection.Open();
        var builder = new DbContextOptionsBuilder<ChirpDbContext>().UseSqlite(connection);
        var context = new ChirpDbContext(builder.Options);
        context.Database.EnsureCreated();
        return context;
    }
	
    
}
}