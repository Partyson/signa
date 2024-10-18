using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using signa.Entities;

namespace signa.Configuration;

public class GroupConfiguration : IEntityTypeConfiguration<GroupEntity>
{
    public void Configure(EntityTypeBuilder<GroupEntity> builder)
    {
        builder.HasKey(x => x.Id);

        builder.HasMany(g => g.Teams)
            .WithOne(t => t.Group);

        builder.HasOne(g => g.Tournament)
            .WithMany(t => t.Groups);
        
        builder.HasMany(g => g.Matches)
            .WithOne(m => m.Group);
    }
}