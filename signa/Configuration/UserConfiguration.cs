using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using signa.Entities;
using signa.Models;

namespace signa.Configuration;

public class UserConfiguration : IEntityTypeConfiguration<UserEntity>
{
    public void Configure(EntityTypeBuilder<UserEntity> builder)
    {
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.FirstName)
            .HasMaxLength(User.MAX_FIRST_NAME_LENGTH)
            .IsRequired();
        
        builder.Property(x => x.LastName)
            .HasMaxLength(User.MAX_LAST_NAME_LENGTH)
            .IsRequired();
        
        builder.Property(x => x.Patronymic)
            .HasMaxLength(User.MAX_PATRONYMIC_LENGTH)
            .IsRequired();
        
        builder.Property(x => x.Gender)
            .IsRequired();
        
        builder.Property(x => x.Email)
            .HasMaxLength(User.VARCHAR_LIMIT)
            .IsRequired();
    }
}