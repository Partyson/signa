﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using signa.Entities;

namespace signa.Configuration;

public class TournamentConfiguration : IEntityTypeConfiguration<TournamentEntity>
{
    public void Configure(EntityTypeBuilder<TournamentEntity> builder)
    {
        builder.HasKey(x => x.Id);
        
        builder.HasMany(t => t.Teams)
            .WithOne(t => t.Tournament);
        
        //builder.HasMany(t => t.Groups)
         //   .WithOne(t => t.Tournament);

        builder.HasMany(t => t.Organizers)
            .WithMany(u => u.OrganizedTournaments);
        
    }
}