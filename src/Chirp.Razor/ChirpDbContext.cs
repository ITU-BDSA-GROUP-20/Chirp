using Chirp.Razor.Models;
//This file holds the datamodel consisting of Author, Cheep, and ChirpDbContext
//EF Core will use the properties of these classes to create and control
//the database, without having the application directly interact with the database. 




public class ChirpDbContext : DbContext
{
    public ChirpDbContext(DbContextOptions<ChirpDbContext> options) : base(options)
    {
    }
    public DbSet<Author> Authors {get; set;}

}