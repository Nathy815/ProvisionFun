using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Persistence.Configurations
{
    public class SetupConfiguration : IEntityTypeConfiguration<Setup>
    {
        public void Configure(EntityTypeBuilder<Setup> builder)
        {
            builder.Property(b => b.Key)
                .IsRequired();

            builder.Property(b => b.Value)
                .IsRequired();

            builder.HasIndex(b => b.Key)
                .IsUnique();

            // Seed
            builder.HasData(
                new Setup
                {
                    Id = new Guid("0707aff8-0901-4f67-bfa4-52a19bbe4237"),
                    Active = true,
                    CreatedDate = Convert.ToDateTime("04/07/2020 13:55:00"),
                    ModifiedDate = Convert.ToDateTime("04/07/2020 13:55:00"),
                    Key = "HomeBanner",
                    Value = ""
                },
                new Setup
                {
                    Id = new Guid("7e5b0a5b-80e8-43ea-8cb1-0b6d8a302a6d"),
                    Active = true,
                    CreatedDate = Convert.ToDateTime("04/07/2020 13:55:00"),
                    ModifiedDate = Convert.ToDateTime("04/07/2020 13:55:00"),
                    Key = "HomeTitle",
                    Value = ""
                },
                new Setup
                {
                    Id = new Guid("8e0ef355-af58-46ca-80a3-d76c4b68927c"),
                    Active = true,
                    CreatedDate = Convert.ToDateTime("04/07/2020 13:55:00"),
                    ModifiedDate = Convert.ToDateTime("04/07/2020 13:55:00"),
                    Key = "ResponsibilityTerm",
                    Value = ""
                },
                new Setup
                {
                    Id = new Guid("c8578be5-ead7-48cc-aac0-cf33a97fca04"),
                    Active = true,
                    CreatedDate = Convert.ToDateTime("04/07/2020 13:55:00"),
                    ModifiedDate = Convert.ToDateTime("04/07/2020 13:55:00"),
                    Key = "Regulation",
                    Value = "Regras"
                },
                new Setup
                {
                    Id = new Guid("f970ccaa-5a0d-41dd-a302-eab3072f8c09"),
                    Active = true,
                    CreatedDate = Convert.ToDateTime("04/07/2020 13:55:00"),
                    ModifiedDate = Convert.ToDateTime("04/07/2020 13:55:00"),
                    Key = "Logo",
                    Value = ""
                },
                new Setup
                {
                    Id = new Guid("e067f067-6ff3-48d2-813b-d4373b68bc54"),
                    Active = true,
                    CreatedDate = Convert.ToDateTime("04/07/2020 13:55:00"),
                    ModifiedDate = Convert.ToDateTime("04/07/2020 13:55:00"),
                    Key = "RegistryBanner",
                    Value = ""
                },
                new Setup
                {
                    Id = new Guid("8aae9ff9-48ae-4222-a144-0abf5070d798"),
                    Active = true,
                    CreatedDate = Convert.ToDateTime("04/07/2020 13:55:00"),
                    ModifiedDate = Convert.ToDateTime("04/07/2020 13:55:00"),
                    Key = "NossoNumero",
                    Value = "5504"
                },
                new Setup
                {
                    Id = new Guid("e8989e76-346f-457d-ba42-5e669e1eec84"),
                    Active = true,
                    CreatedDate = Convert.ToDateTime("04/07/2020 13:55:00"),
                    ModifiedDate = Convert.ToDateTime("04/07/2020 13:55:00"),
                    Key = "ShippingFile",
                    Value = "1"
                },
                new Setup
                {
                    Id = new Guid("b3a92942-b10c-4a82-bc2e-e58627ee7010"),
                    Active = true,
                    CreatedDate = Convert.ToDateTime("04/07/2020 13:55:00"),
                    ModifiedDate = Convert.ToDateTime("04/07/2020 13:55:00"),
                    Key = "HomeBanner2",
                    Value = ""
                },
                new Setup
                {
                    Id = new Guid("e180712c-a3a9-489b-ac61-2a64defa8c34"),
                    Active = true,
                    CreatedDate = Convert.ToDateTime("04/07/2020 13:55:00"),
                    ModifiedDate = Convert.ToDateTime("04/07/2020 13:55:00"),
                    Key = "HomeBanner3",
                    Value = ""
                }
            );
        }
    }
}
