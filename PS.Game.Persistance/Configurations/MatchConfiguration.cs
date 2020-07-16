using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace PS.Game.Persistance.Configurations
{
    public class MatchConfiguration : IEntityTypeConfiguration<Match>
    {
        public void Configure(EntityTypeBuilder<Match> builder)
        {
            builder.HasOne(b => b.Player1)
                .WithMany(p => p.MatchesAsPlayer1)
                .HasForeignKey(p => p.Player1ID)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(b => b.Player2)
                .WithMany(p => p.MatchesAsPlayer2)
                .HasForeignKey(p => p.Player2ID)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
