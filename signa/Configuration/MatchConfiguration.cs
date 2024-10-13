using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using signa.Entities;

namespace signa.Configuration;

public class MatchConfiguration : IEntityTypeConfiguration<MatchEntity>
{
    public void Configure(EntityTypeBuilder<MatchEntity> builder)
    {
        builder.HasKey(m => m.Id);

        builder.HasMany(m => m.Teams)
            .WithMany(t => t.Matches)
            .UsingEntity<MatchTeamEntity>();

        builder.HasOne(m => m.Tournament)
            .WithMany(t => t.Matches);
        
        //builder.HasOne(m => m.Group)
            //.WithMany(g => g.Matches);
    }
}