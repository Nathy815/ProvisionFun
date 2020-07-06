using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Persistence.Configurations
{
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.Property(b => b.Name)
                .IsRequired();

            builder.HasIndex(b => b.Name);

            // Seed

            builder.HasData(
                new Role
                {
                    Id = new Guid("66300219-e7f6-4f17-a859-d8cc11315796"),
                    Active = true,
                    CreatedDate = Convert.ToDateTime("04/07/2020 13:55:00"),
                    ModifiedDate = Convert.ToDateTime("04/07/2020 13:55:00"),
                    Name = "Administrador"
                },
                new Role
                {
                    Id = new Guid("2f743547-6ab3-4f99-93a0-457eab81fecf"),
                    Active = true,
                    CreatedDate = Convert.ToDateTime("04/07/2020 13:55:00"),
                    ModifiedDate = Convert.ToDateTime("04/07/2020 13:55:00"),
                    Name = "Auditor"
                }
            );
        }
    }
}
