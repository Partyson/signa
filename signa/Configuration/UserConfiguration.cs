using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using signa.Entities;

namespace signa.Configuration;

public class UserConfiguration : IEntityTypeConfiguration<UserEntity>
{
    public void Configure(EntityTypeBuilder<UserEntity> builder)
    {
        builder.HasKey(x => x.Id);
        builder.HasMany(u => u.Teams)
            .WithMany(t => t.Members);
        
        builder.HasMany(u => u.CaptainsTeams)
            .WithOne(t => t.Captain);
        
        builder.HasMany(u => u.OrganizedTournaments)
            .WithMany(t => t.Organizers);

        builder.HasMany(u => u.Invites)
            .WithOne(i => i.InvitedUser);
        
        builder.Property(x => x.FirstName)
            .IsRequired();
        
        builder.Property(x => x.LastName)
            .IsRequired();
        
        builder.Property(x => x.Patronymic)
            .IsRequired();
        
        builder.Property(x => x.Gender)
            .HasConversion<string>()
            .IsRequired();
        
        builder.Property(x => x.Email)
            .IsRequired();
        
        builder.Property(x => x.Role)
            .HasConversion<string>()
            .IsRequired();
        
        builder.Property(x => x.FullName)
            .HasComputedColumnSql("CONCAT(FirstName, \" \", LastName, \" \", Patronymic)", stored: true);
    }
}