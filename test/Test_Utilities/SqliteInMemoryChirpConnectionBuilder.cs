using Microsoft.EntityFrameworkCore;
using Chirp.Core;
using Chirp.Infrastructure;
using Microsoft.VisualBasic;
using Microsoft.Data.Sqlite;

namespace Test_Utilities{
public class SqliteInMemoryChirpConnectionBuilder
{   
   
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