using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using signa.Entities;

namespace signa.Configuration;

public class TeamConfiguration : IEntityTypeConfiguration<TeamEntity>
{
    public void Configure(EntityTypeBuilder<TeamEntity> builder)
    {
        builder.HasKey(x => x.Id);

        builder.HasMany(t => t.Members)
            .WithMany(u => u.Teams);
        
        builder.HasOne(t => t.Tournament)
            .WithMany(t => t.Teams);
        
        builder.HasOne(t => t.Captain)
            .WithMany(u => u.CaptainsTeams);
        
        builder.HasMany(t => t.Matches)
            .WithMany(m => m.Teams)
            .UsingEntity<MatchTeamEntity>();
        
        builder.Property(x => x.Title).IsRequired();
    }
}