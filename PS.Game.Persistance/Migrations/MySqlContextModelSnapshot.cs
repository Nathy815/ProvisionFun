﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Persistence.Contexts;

namespace PS.Game.Persistance.Migrations
{
    [DbContext(typeof(MySqlContext))]
    partial class MySqlContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.11-servicing-32099")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("Domain.Entities.Condominium", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Active");

                    b.Property<string>("Address");

                    b.Property<string>("City");

                    b.Property<DateTime>("CreatedDate");

                    b.Property<DateTime>("ModifiedDate");

                    b.Property<string>("Name");

                    b.Property<string>("Number")
                        .IsRequired();

                    b.Property<string>("State");

                    b.Property<bool>("Validated");

                    b.Property<string>("ZipCode")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("Number");

                    b.HasIndex("ZipCode");

                    b.ToTable("Condominiums");
                });

            modelBuilder.Entity("Domain.Entities.Game", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Active");

                    b.Property<DateTime>("CreatedDate");

                    b.Property<DateTime>("ModifiedDate");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Games");
                });

            modelBuilder.Entity("Domain.Entities.Match", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Active");

                    b.Property<Guid?>("AuditorID");

                    b.Property<string>("Comments");

                    b.Property<DateTime>("CreatedDate");

                    b.Property<DateTime?>("Date");

                    b.Property<DateTime>("ModifiedDate");

                    b.Property<Guid>("Player1ID");

                    b.Property<double>("Player1Score");

                    b.Property<Guid>("Player2ID");

                    b.Property<double>("Player2Score");

                    b.Property<Guid?>("PlayerId");

                    b.Property<int>("Round");

                    b.Property<int>("Sequence");

                    b.Property<Guid>("TournamentID");

                    b.Property<Guid?>("Winner");

                    b.HasKey("Id");

                    b.HasIndex("AuditorID");

                    b.HasIndex("Player1ID");

                    b.HasIndex("Player2ID");

                    b.HasIndex("PlayerId");

                    b.HasIndex("TournamentID");

                    b.ToTable("Matches");
                });

            modelBuilder.Entity("Domain.Entities.Payment", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Active");

                    b.Property<DateTime>("CreatedDate");

                    b.Property<string>("DocumentNumber");

                    b.Property<DateTime>("DueDate");

                    b.Property<string>("FormatedNumber");

                    b.Property<DateTime>("IssueDate");

                    b.Property<DateTime>("ModifiedDate");

                    b.Property<string>("Number");

                    b.Property<string>("NumberVD");

                    b.Property<double>("Price");

                    b.Property<Guid>("TeamID");

                    b.Property<bool>("Validated");

                    b.HasKey("Id");

                    b.HasIndex("TeamID");

                    b.ToTable("Payments");
                });

            modelBuilder.Entity("Domain.Entities.Player", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Active");

                    b.Property<DateTime>("BirthDate");

                    b.Property<string>("CPF")
                        .IsRequired();

                    b.Property<DateTime>("CreatedDate");

                    b.Property<string>("Document")
                        .IsRequired();

                    b.Property<string>("Email");

                    b.Property<DateTime>("ModifiedDate");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("CPF")
                        .IsUnique();

                    b.ToTable("Players");
                });

            modelBuilder.Entity("Domain.Entities.Role", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Active");

                    b.Property<DateTime>("CreatedDate");

                    b.Property<DateTime>("ModifiedDate");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("Name");

                    b.ToTable("Roles");

                    b.HasData(
                        new { Id = new Guid("66300219-e7f6-4f17-a859-d8cc11315796"), Active = true, CreatedDate = new DateTime(2020, 7, 4, 13, 55, 0, 0, DateTimeKind.Unspecified), ModifiedDate = new DateTime(2020, 7, 4, 13, 55, 0, 0, DateTimeKind.Unspecified), Name = "Administrador" },
                        new { Id = new Guid("2f743547-6ab3-4f99-93a0-457eab81fecf"), Active = true, CreatedDate = new DateTime(2020, 7, 4, 13, 55, 0, 0, DateTimeKind.Unspecified), ModifiedDate = new DateTime(2020, 7, 4, 13, 55, 0, 0, DateTimeKind.Unspecified), Name = "Auditor" }
                    );
                });

            modelBuilder.Entity("Domain.Entities.Setup", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Active");

                    b.Property<DateTime>("CreatedDate");

                    b.Property<string>("Key")
                        .IsRequired();

                    b.Property<DateTime>("ModifiedDate");

                    b.Property<string>("Value")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("Key")
                        .IsUnique();

                    b.ToTable("Setups");

                    b.HasData(
                        new { Id = new Guid("0707aff8-0901-4f67-bfa4-52a19bbe4237"), Active = true, CreatedDate = new DateTime(2020, 7, 4, 13, 55, 0, 0, DateTimeKind.Unspecified), Key = "BannerHome", ModifiedDate = new DateTime(2020, 7, 4, 13, 55, 0, 0, DateTimeKind.Unspecified), Value = "" },
                        new { Id = new Guid("7e5b0a5b-80e8-43ea-8cb1-0b6d8a302a6d"), Active = true, CreatedDate = new DateTime(2020, 7, 4, 13, 55, 0, 0, DateTimeKind.Unspecified), Key = "HomeTitle", ModifiedDate = new DateTime(2020, 7, 4, 13, 55, 0, 0, DateTimeKind.Unspecified), Value = "" },
                        new { Id = new Guid("8e0ef355-af58-46ca-80a3-d76c4b68927c"), Active = true, CreatedDate = new DateTime(2020, 7, 4, 13, 55, 0, 0, DateTimeKind.Unspecified), Key = "ResponsibilityTerm", ModifiedDate = new DateTime(2020, 7, 4, 13, 55, 0, 0, DateTimeKind.Unspecified), Value = "" },
                        new { Id = new Guid("c8578be5-ead7-48cc-aac0-cf33a97fca04"), Active = true, CreatedDate = new DateTime(2020, 7, 4, 13, 55, 0, 0, DateTimeKind.Unspecified), Key = "Regulation", ModifiedDate = new DateTime(2020, 7, 4, 13, 55, 0, 0, DateTimeKind.Unspecified), Value = "Regras" },
                        new { Id = new Guid("f970ccaa-5a0d-41dd-a302-eab3072f8c09"), Active = true, CreatedDate = new DateTime(2020, 7, 4, 13, 55, 0, 0, DateTimeKind.Unspecified), Key = "Logo", ModifiedDate = new DateTime(2020, 7, 4, 13, 55, 0, 0, DateTimeKind.Unspecified), Value = "" },
                        new { Id = new Guid("8aae9ff9-48ae-4222-a144-0abf5070d798"), Active = true, CreatedDate = new DateTime(2020, 7, 4, 13, 55, 0, 0, DateTimeKind.Unspecified), Key = "NossoNumero", ModifiedDate = new DateTime(2020, 7, 4, 13, 55, 0, 0, DateTimeKind.Unspecified), Value = "" },
                        new { Id = new Guid("e8989e76-346f-457d-ba42-5e669e1eec84"), Active = true, CreatedDate = new DateTime(2020, 7, 4, 13, 55, 0, 0, DateTimeKind.Unspecified), Key = "ShippingFile", ModifiedDate = new DateTime(2020, 7, 4, 13, 55, 0, 0, DateTimeKind.Unspecified), Value = "" }
                    );
                });

            modelBuilder.Entity("Domain.Entities.Team", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Active");

                    b.Property<string>("CancellationComments");

                    b.Property<bool>("CancellationSent");

                    b.Property<int>("Color");

                    b.Property<Guid>("CondominiumID");

                    b.Property<DateTime>("CreatedDate");

                    b.Property<bool>("FinishedSent");

                    b.Property<int>("Icon");

                    b.Property<string>("Mode")
                        .IsRequired();

                    b.Property<DateTime>("ModifiedDate");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<DateTime?>("PaymentDate");

                    b.Property<bool>("PaymentSent");

                    b.Property<double>("Price");

                    b.Property<string>("Status")
                        .IsRequired();

                    b.Property<bool>("SubscryptionSent");

                    b.Property<Guid>("TournamentID");

                    b.Property<DateTime?>("ValidatedDate");

                    b.HasKey("Id");

                    b.HasIndex("CondominiumID");

                    b.HasIndex("TournamentID");

                    b.ToTable("Teams");
                });

            modelBuilder.Entity("Domain.Entities.TeamPlayer", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Active");

                    b.Property<DateTime>("CreatedDate");

                    b.Property<bool>("IsPrincipal");

                    b.Property<DateTime>("ModifiedDate");

                    b.Property<Guid>("PlayerID");

                    b.Property<Guid>("TeamID");

                    b.HasKey("Id");

                    b.HasIndex("PlayerID");

                    b.HasIndex("TeamID");

                    b.ToTable("TeamPlayers");
                });

            modelBuilder.Entity("Domain.Entities.Tournament", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Active");

                    b.Property<DateTime>("CreatedDate");

                    b.Property<DateTime>("EndSubscryption");

                    b.Property<Guid>("GameID");

                    b.Property<int>("Mode");

                    b.Property<DateTime>("ModifiedDate");

                    b.Property<string>("Name");

                    b.Property<string>("Plataform");

                    b.Property<int>("PlayerLimit");

                    b.Property<int>("RoundSolo");

                    b.Property<int>("RoundTeam");

                    b.Property<DateTime>("StartSubscryption");

                    b.Property<int>("SubscryptionLimit");

                    b.HasKey("Id");

                    b.HasIndex("GameID");

                    b.ToTable("Tournaments");
                });

            modelBuilder.Entity("Domain.Entities.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Active");

                    b.Property<DateTime>("CreatedDate");

                    b.Property<string>("Email")
                        .IsRequired();

                    b.Property<bool>("IsMaster");

                    b.Property<DateTime>("ModifiedDate");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<string>("Password")
                        .IsRequired();

                    b.Property<Guid>("RoleID");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.HasIndex("RoleID");

                    b.ToTable("Users");

                    b.HasData(
                        new { Id = new Guid("0f054b3e-aaf0-44ae-a2af-4d1f1fa69b02"), Active = true, CreatedDate = new DateTime(2020, 7, 4, 13, 55, 0, 0, DateTimeKind.Unspecified), Email = "master@master.com", IsMaster = true, ModifiedDate = new DateTime(2020, 7, 4, 13, 55, 0, 0, DateTimeKind.Unspecified), Name = "Master", Password = "$2b$10$pULpx4mnTZwloXK5uW5JuugokJih/WzBZkABdRO/e/S8n3a1HfYUC", RoleID = new Guid("66300219-e7f6-4f17-a859-d8cc11315796") },
                        new { Id = new Guid("d06fc2b3-d60b-4ad2-8794-829daa444506"), Active = true, CreatedDate = new DateTime(2020, 7, 4, 13, 55, 0, 0, DateTimeKind.Unspecified), Email = "adm@adm.com", IsMaster = false, ModifiedDate = new DateTime(2020, 7, 4, 13, 55, 0, 0, DateTimeKind.Unspecified), Name = "Administrator", Password = "$2b$10$.AM41bg9SFHPqGWCw6398.9BlfKsibsdNUQVqYlkW/6LrDrJIEYn.", RoleID = new Guid("66300219-e7f6-4f17-a859-d8cc11315796") }
                    );
                });

            modelBuilder.Entity("Domain.Entities.Match", b =>
                {
                    b.HasOne("Domain.Entities.User", "Auditor")
                        .WithMany()
                        .HasForeignKey("AuditorID");

                    b.HasOne("Domain.Entities.Team", "Player1")
                        .WithMany("MatchesAsPlayer1")
                        .HasForeignKey("Player1ID")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Domain.Entities.Team", "Player2")
                        .WithMany("MatchesAsPlayer2")
                        .HasForeignKey("Player2ID")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Domain.Entities.Player")
                        .WithMany("Matches")
                        .HasForeignKey("PlayerId");

                    b.HasOne("Domain.Entities.Tournament", "Tournament")
                        .WithMany("Matches")
                        .HasForeignKey("TournamentID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Domain.Entities.Payment", b =>
                {
                    b.HasOne("Domain.Entities.Team", "Team")
                        .WithMany("Payments")
                        .HasForeignKey("TeamID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Domain.Entities.Team", b =>
                {
                    b.HasOne("Domain.Entities.Condominium", "Condominium")
                        .WithMany("Teams")
                        .HasForeignKey("CondominiumID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Domain.Entities.Tournament", "Tournament")
                        .WithMany("Teams")
                        .HasForeignKey("TournamentID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Domain.Entities.TeamPlayer", b =>
                {
                    b.HasOne("Domain.Entities.Player", "Player")
                        .WithMany("Teams")
                        .HasForeignKey("PlayerID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Domain.Entities.Team", "Team")
                        .WithMany("Players")
                        .HasForeignKey("TeamID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Domain.Entities.Tournament", b =>
                {
                    b.HasOne("Domain.Entities.Game", "Game")
                        .WithMany("Tournaments")
                        .HasForeignKey("GameID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Domain.Entities.User", b =>
                {
                    b.HasOne("Domain.Entities.Role", "Role")
                        .WithMany("Users")
                        .HasForeignKey("RoleID")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
