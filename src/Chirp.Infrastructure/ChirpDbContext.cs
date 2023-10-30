using Chirp.Core.Entities;
using Microsoft.EntityFrameworkCore;

//This file holds the datamodel consisting of Author, Cheep, and ChirpDbContext
//EF Core will use the properties of these classes to create and control
//the database, without having the application directly interact with the database. 

namespace Chirp.Infrastructure;

public class ChirpDbContext : DbContext
{
    public DbSet<AuthorDTO> Authors {get; set;} = null!;

    public DbSet<CheepDTO> Cheeps {get; set;} = null!;
    //Source for methods:
    //https://www.c-sharpcorner.com/article/get-started-with-entity-framework-core-using-sqlite/

    public ChirpDbContext(DbContextOptions<ChirpDbContext> dbContextOptions) : base(dbContextOptions)
    {
        DbInitializer.SeedDatabase(this);
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AuthorDTO>(Entity =>
        {
            Entity.HasKey(e => e.AuthorId);
            Entity.Property(e => e.Name).IsRequired();
            Entity.Property(e => e.Email).IsRequired();
            Entity.HasMany(e => e.Cheeps);
        });

        modelBuilder.Entity<CheepDTO>(Entity =>
        {
            Entity.HasKey(e => e.CheepId);
            Entity.Property(e => e.Text).IsRequired();
            Entity.Property(e => e.TimeStamp).IsRequired();
            Entity.HasOne(e => e.AuthorDto)
                .WithMany(author => author.Cheeps)
                .HasForeignKey(e => e.AuthorId);
        });
    }

    public void initializeDb() 
    {
        Database.EnsureCreated();
        DbInitializer.SeedDatabase(this);
    }
}