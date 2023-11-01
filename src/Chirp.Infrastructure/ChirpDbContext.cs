using Chirp.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

//This file holds the datamodel consisting of Author, Cheep, and ChirpDbContext
//EF Core will use the properties of these classes to create and control
//the database, without having the application directly interact with the database. 

namespace Chirp.Infrastructure;

public class ChirpDbContext : IdentityDbContext<IdentityUser>
{
    public DbSet<AuthorDTO> Authors {get; set;} = null!;

    public DbSet<CheepDTO> Cheeps {get; set;} = null!;
    //Source for methods:
    //https://www.c-sharpcorner.com/article/get-started-with-entity-framework-core-using-sqlite/

    public ChirpDbContext(DbContextOptions<ChirpDbContext> dbContextOptions) : base(dbContextOptions)
    {
        Cheeps.Include(c => c.AuthorDto).ToList();
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AuthorDTO>(Entity =>
        {
            Entity.HasKey(e => e.AuthorId);
            Entity.Property(e => e.Name).IsRequired();
            
            // Email should be required and unique
            Entity.Property(e => e.Email).IsRequired();
            Entity.HasIndex(e => e.Email).IsUnique();
            
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

        modelBuilder.Entity<IdentityUserLogin<string>>().HasKey(e => e.UserId);
        modelBuilder.Entity<IdentityUserRole<string>>().HasKey(e => e.RoleId);
        modelBuilder.Entity<IdentityUserToken<string>>().HasKey(e => e.UserId);
    }
}