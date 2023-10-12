using Chirp.Razor.Models;
using Microsoft.EntityFrameworkCore;

//This file holds the datamodel consisting of Author, Cheep, and ChirpDbContext
//EF Core will use the properties of these classes to create and control
//the database, without having the application directly interact with the database. 

namespace Chirp.Razor;

public class ChirpDBContext : DbContext
{
    public DbSet<Author> Authors {get; set;}
    public DbSet<Cheep> Cheeps {get; set;}
    //Source for methods:
    //https://www.c-sharpcorner.com/article/get-started-with-entity-framework-core-using-sqlite/

    public ChirpDBContext(DbContextOptions<ChirpDBContext> dbContextOptions) : base(dbContextOptions)
    {
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Author>(Entity =>
        {
            Entity.HasKey(e => e.AuthorId);
            Entity.Property(e => e.Name).IsRequired();
            Entity.Property(e => e.Email).IsRequired();
            Entity.HasMany(e => e.Cheeps);
        });

        modelBuilder.Entity<Cheep>(Entity =>
        {
            Entity.HasKey(e => e.CheepId);
            Entity.Property(e => e.Text).IsRequired();
            Entity.Property(e => e.TimeStamp).IsRequired();
            Entity.HasOne(e => e.Author)
                .WithMany(author => author.Cheeps)
                .HasForeignKey(e => e.AuthorId);
        });
    }
}