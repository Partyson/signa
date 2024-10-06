using Microsoft.EntityFrameworkCore;
using signa.Entities;

namespace signa.DataAccess;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }
    
    public DbSet<UserEntity> Users { get; set; }
}