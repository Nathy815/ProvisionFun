using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Persistence.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Property(b => b.Email)
                .IsRequired();

            builder.Property(b => b.Name)
                .IsRequired();

            builder.Property(b => b.Password)
                .IsRequired();

            builder.Property(b => b.IsMaster);
                //.HasDefaultValue(false);

            builder.HasIndex(b => b.Email)
                .IsUnique();

            // Seed
            builder.HasData(
                new User
                {
                    Id = new Guid("0f054b3e-aaf0-44ae-a2af-4d1f1fa69b02"),
                    Active = true,
                    CreatedDate = Convert.ToDateTime("04/07/2020 13:55:00"),
                    ModifiedDate = Convert.ToDateTime("04/07/2020 13:55:00"),
                    Name = "Master",
                    Email = "master@master.com",
                    Password = BCrypt.Net.BCrypt.HashPassword("master123"),
                    IsMaster = true,
                    RoleID = new Guid("66300219-e7f6-4f17-a859-d8cc11315796")
                },
                new User
                {
                    Id = new Guid("d06fc2b3-d60b-4ad2-8794-829daa444506"),
                    Active = true,
                    CreatedDate = Convert.ToDateTime("04/07/2020 13:55:00"),
                    ModifiedDate = Convert.ToDateTime("04/07/2020 13:55:00"),
                    Name = "Administrator",
                    Email = "adm@adm.com",
                    Password = BCrypt.Net.BCrypt.HashPassword("admin123"),
                    IsMaster = false,
                    RoleID = new Guid("66300219-e7f6-4f17-a859-d8cc11315796")
                }
            );
        }
    }
}
