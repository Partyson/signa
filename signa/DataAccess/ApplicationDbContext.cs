using Microsoft.EntityFrameworkCore;
using signa.Entities;

namespace signa.DataAccess;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<UserEntity> Users { get; set; }
    
    public DbSet<TournamentEntity> Tournaments { get; set; }
}