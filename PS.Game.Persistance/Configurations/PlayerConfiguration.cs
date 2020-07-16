using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Persistence.Configurations
{
    public class PlayerConfiguration : IEntityTypeConfiguration<Player>
    {
        public void Configure(EntityTypeBuilder<Player> builder)
        {
            builder.Property(b => b.CPF)
                .IsRequired();

            builder.Property(b => b.BirthDate)
                .IsRequired();

            builder.Property(b => b.Name)
                .IsRequired();

            builder.Property(b => b.Document)
                .IsRequired();

            builder.HasIndex(b => b.CPF)
                .IsUnique();
        }
    }
}
