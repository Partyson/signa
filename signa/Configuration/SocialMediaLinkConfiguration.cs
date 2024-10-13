using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using signa.Entities;

namespace signa.Configuration;

public class SocialMediaLinkConfiguration : IEntityTypeConfiguration<SocialMediaLinkEntity>
{
    public void Configure(EntityTypeBuilder<SocialMediaLinkEntity> builder)
    {
        throw new NotImplementedException();
    }
}