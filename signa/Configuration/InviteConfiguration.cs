using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using signa.Entities;

namespace signa.Configuration;

public class InviteConfiguration : IEntityTypeConfiguration<InviteEntity>
{
    public void Configure(EntityTypeBuilder<InviteEntity> builder)
    {
        builder.HasKey(x => x.Id);
        builder.HasOne(i => i.InvitedUser)
            .WithMany(u => u.Invites);
        builder.HasOne(i => i.InviteTeam)
            .WithMany(u => u.Invites);
    }
}