using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Persistence.Configurations
{
    public class CondominiumConfiguration : IEntityTypeConfiguration<Condominium>
    {
        public void Configure(EntityTypeBuilder<Condominium> builder)
        {
            builder.Property(b => b.Validated);
                //.HasDefaultValue(false);

            builder.Property(b => b.ZipCode)
                .IsRequired();

            builder.Property(b => b.Number)
                .IsRequired();

            builder.HasIndex(b => b.ZipCode);

            builder.HasIndex(b => b.Number);
        }
    }
}
