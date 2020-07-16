using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Persistence.Configurations
{
    public class TeamConfiguration : IEntityTypeConfiguration<Team>
    {
        public void Configure(EntityTypeBuilder<Team> builder)
        {
            builder.Property(b => b.FinishedSent);
                //.HasDefaultValue(false);

            builder.Property(b => b.Mode)
                .HasConversion(
                    e => e.ToString(),
                    e => (Domain.Enums.eMode)Enum.Parse(typeof(Domain.Enums.eMode), e)
                );

            builder.Property(b => b.Status)
                //.HasDefaultValue(Domain.Enums.eStatus.Validation)
                .HasConversion(
                    e => e.ToString(),
                    e => (Domain.Enums.eStatus)Enum.Parse(typeof(Domain.Enums.eStatus), e)
                );

            builder.Property(b => b.Name)
                .IsRequired();

            builder.Property(b => b.SubscryptionSent);
                //.HasDefaultValue(false);

            builder.HasIndex(b => b.TournamentID);

            builder.HasIndex(b => b.CondominiumID);
        }
    }
}
