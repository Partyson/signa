using Microsoft.EntityFrameworkCore;
using signa.Entities;

namespace signa.DataAccess;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<UserEntity> Users { get; set; }
    
    public DbSet<TournamentEntity> Tournaments { get; set; }
    
    public DbSet<TeamEntity> Teams { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TeamEntity>()
            .HasOne(t => t.Tournament)
            .WithMany(t => t.Teams)
            .HasForeignKey("tournamentId")
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<TeamEntity>()
            .HasMany(t => t.Members)
            .WithMany(t => t.Teams)
            .UsingEntity("TeamsToMembers");

        modelBuilder.Entity<TeamEntity>()
            .HasOne(t => t.Captain)
            .WithMany(t => t.Teams)
            .HasForeignKey("captainId");

        modelBuilder.Entity<TeamEntity>()
            .HasOne(t => t.Group)
            .WithMany(g => g.Teams)
            .HasForeignKey("groupId");
        
        
    }
}