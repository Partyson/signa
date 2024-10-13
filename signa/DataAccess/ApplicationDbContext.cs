using Microsoft.EntityFrameworkCore;
using signa.Configuration;
using signa.Entities;

namespace signa.DataAccess;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<UserEntity> Users { get; set; }
    
    public DbSet<TournamentEntity> Tournaments { get; set; }
    
    public DbSet<TeamEntity> Teams { get; set; }
    
    public DbSet<GroupEntity> Groups { get; set; }
    
    public DbSet<MatchEntity> Matches { get; set; }
    
    public DbSet<SocialMediaLinkEntity> SocialMediaLinks { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new TournamentConfiguration());
        modelBuilder.ApplyConfiguration(new TeamConfiguration());
        modelBuilder.ApplyConfiguration(new MatchConfiguration());
    }
}