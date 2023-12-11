using Chirp.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

//This file holds the datamodel consisting of Author, Cheep, and ChirpDbContext
//EF Core will use the properties of these classes to create and control
//the database, without having the application directly interact with the database. 

namespace Chirp.Infrastructure;

public class ChirpDbContext : IdentityDbContext<Author, IdentityRole<Guid>, Guid>
{
    public DbSet<Cheep> Cheeps {get; set;} = null!;
    
    public DbSet<Follow> Follows { get; set; } = null!;
    public DbSet<Reaction> Reactions { get; set; }
    //Source for methods:
    //https://www.c-sharpcorner.com/article/get-started-with-entity-framework-core-using-sqlite/

    public ChirpDbContext(DbContextOptions<ChirpDbContext> dbContextOptions) : base(dbContextOptions)
    {
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {   

        // Author entity
        modelBuilder.Entity<Author>(entity =>
        {
            modelBuilder.Entity<IdentityUserLogin<Guid>>().HasKey(p => new { p.LoginProvider, p.ProviderKey });
            modelBuilder.Entity<IdentityUserRole<Guid>>().HasKey(p => new { p.UserId, p.RoleId });
            modelBuilder.Entity<IdentityUserToken<Guid>>().HasKey(p => new { p.UserId, p.LoginProvider, p.Name });

            modelBuilder.Entity<Author>(entity =>
            {
                entity.Property(e => e.Id);

                entity.HasMany(e => e.Cheeps)
                    .WithOne(c => c.Author)
                    .HasForeignKey(c => c.AuthorId)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Cascade); // Cascade delete for Cheeps

                entity.HasMany(e => e.Followers)
                    .WithOne(f => f.Following)
                    .HasForeignKey(f => f.FollowingId)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Cascade); // Cascade delete for Followers
            });
        });

        // Follow entity
        modelBuilder.Entity<Follow>(entity =>
        {
            entity.HasKey(e => new { e.FollowerId, e.FollowingId });
            
            entity.HasOne(f => f.Following)
                .WithMany(a => a.Followers)
                .HasForeignKey(f => f.FollowingId)
                .OnDelete(DeleteBehavior.Restrict);
            
            entity.HasOne(f => f.Follower)
                .WithMany(a => a.Following)
                .HasForeignKey(f => f.FollowerId)
                .OnDelete(DeleteBehavior.Cascade);
        });
        
        // Cheep entity
        modelBuilder.Entity<Cheep>(entity =>
        {
            entity.HasKey(e => e.CheepId);
            entity.Property(e => e.Text).IsRequired();
            entity.Property(e => e.TimeStamp).IsRequired();

            entity.HasOne(c => c.Author)
                .WithMany(a => a.Cheeps)
                .HasForeignKey(c => c.AuthorId)
                .OnDelete(DeleteBehavior.Cascade); // Cascade delete for Cheeps

            entity.HasMany(c => c.Reactions)
                .WithOne(r => r.Cheep)
                .HasForeignKey(r => r.CheepId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade); // Cascade delete for Reactions
        });

        modelBuilder.Entity<Reaction>().Property(m => m.ReactionType)
            .HasConversion<string>();

        modelBuilder.Entity<Reaction>().HasKey(r => new { r.CheepId, r.AuthorId });

        modelBuilder.Entity<IdentityUserLogin<Guid>>().HasKey(e => e.UserId);
        modelBuilder.Entity<IdentityUserRole<Guid>>().HasKey(e => e.RoleId);
        modelBuilder.Entity<IdentityUserToken<Guid>>().HasKey(e => e.UserId);
    }
}