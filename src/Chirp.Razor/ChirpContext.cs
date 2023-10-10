using Chirp.Razor.Models;
//This file holds the datamodel consisting of Author, Cheep, and ChirpDbContext
//EF Core will use the properties of these classes to create and control
//the database, without having the application directly interact with the database. 




public class ChirpDBContext : DbContext
{
    public ChirpDBContext(DbContextOptions<ChirpDBContext> options) : base(options)
    {
    }
    public DbSet<Author> Authors {get; set;}
    //Source for methods:
    //https://www.c-sharpcorner.com/article/get-started-with-entity-framework-core-using-sqlite/

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=./data/ChirpDBContext.db");
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Author>(Entity =>
        {
            Entity.HasKey(e => e.AuthorId);
            Entity.Property(e => e.Name).IsRequired();
            Entity.Property(e => e.Email).IsRequired();
            Entity.HasMany(e => e.Cheeps).WithOne(e => e.Author).HasForeignKey(e => e.AuthorId);
        });
        OnModelCreating(modelBuilder);
    }
    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

}