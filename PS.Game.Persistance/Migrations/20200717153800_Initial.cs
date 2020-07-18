using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PS.Game.Persistance.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Condominiums",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Active = table.Column<bool>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    ZipCode = table.Column<string>(nullable: false),
                    Address = table.Column<string>(nullable: true),
                    Number = table.Column<string>(nullable: false),
                    City = table.Column<string>(nullable: true),
                    State = table.Column<string>(nullable: true),
                    Validated = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Condominiums", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Games",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Active = table.Column<bool>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Games", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Players",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Active = table.Column<bool>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Email = table.Column<string>(nullable: true),
                    BirthDate = table.Column<DateTime>(nullable: false),
                    CPF = table.Column<string>(nullable: false),
                    Document = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Players", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Active = table.Column<bool>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Setups",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Active = table.Column<bool>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: false),
                    Key = table.Column<string>(nullable: false),
                    Value = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Setups", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tournaments",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Active = table.Column<bool>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    StartSubscryption = table.Column<DateTime>(nullable: false),
                    EndSubscryption = table.Column<DateTime>(nullable: false),
                    SubscryptionLimit = table.Column<int>(nullable: false),
                    PlayerLimit = table.Column<int>(nullable: false),
                    Mode = table.Column<int>(nullable: false),
                    Plataform = table.Column<string>(nullable: true),
                    Type = table.Column<int>(nullable: false),
                    GameID = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tournaments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tournaments_Games_GameID",
                        column: x => x.GameID,
                        principalTable: "Games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Active = table.Column<bool>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Email = table.Column<string>(nullable: false),
                    Password = table.Column<string>(nullable: false),
                    IsMaster = table.Column<bool>(nullable: false),
                    RoleID = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Roles_RoleID",
                        column: x => x.RoleID,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Teams",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Active = table.Column<bool>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Icon = table.Column<int>(nullable: false),
                    Color = table.Column<int>(nullable: false),
                    Mode = table.Column<string>(nullable: false),
                    Status = table.Column<string>(nullable: false),
                    Price = table.Column<double>(nullable: false),
                    SubscryptionSent = table.Column<bool>(nullable: false),
                    PaymentSent = table.Column<bool>(nullable: false),
                    FinishedSent = table.Column<bool>(nullable: false),
                    CancellationSent = table.Column<bool>(nullable: false),
                    ValidatedDate = table.Column<DateTime>(nullable: true),
                    PaymentDate = table.Column<DateTime>(nullable: true),
                    CancellationComments = table.Column<string>(nullable: true),
                    CondominiumID = table.Column<Guid>(nullable: false),
                    TournamentID = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teams", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Teams_Condominiums_CondominiumID",
                        column: x => x.CondominiumID,
                        principalTable: "Condominiums",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Teams_Tournaments_TournamentID",
                        column: x => x.TournamentID,
                        principalTable: "Tournaments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Matches",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Active = table.Column<bool>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: false),
                    Sequence = table.Column<int>(nullable: false),
                    Date = table.Column<DateTime>(nullable: true),
                    Comments = table.Column<string>(nullable: true),
                    Player1Score = table.Column<double>(nullable: false),
                    Player2Score = table.Column<double>(nullable: false),
                    Winner = table.Column<Guid>(nullable: true),
                    Type = table.Column<int>(nullable: false),
                    Player1ID = table.Column<Guid>(nullable: false),
                    Player2ID = table.Column<Guid>(nullable: false),
                    AuditorID = table.Column<Guid>(nullable: true),
                    TournamentID = table.Column<Guid>(nullable: false),
                    PlayerId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Matches", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Matches_Users_AuditorID",
                        column: x => x.AuditorID,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Matches_Teams_Player1ID",
                        column: x => x.Player1ID,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Matches_Teams_Player2ID",
                        column: x => x.Player2ID,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Matches_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Matches_Tournaments_TournamentID",
                        column: x => x.TournamentID,
                        principalTable: "Tournaments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Payments",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Active = table.Column<bool>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: false),
                    DocumentNumber = table.Column<string>(nullable: true),
                    Number = table.Column<string>(nullable: true),
                    NumberVD = table.Column<string>(nullable: true),
                    FormatedNumber = table.Column<string>(nullable: true),
                    IssueDate = table.Column<DateTime>(nullable: false),
                    DueDate = table.Column<DateTime>(nullable: false),
                    Price = table.Column<double>(nullable: false),
                    Validated = table.Column<bool>(nullable: false),
                    TeamID = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Payments_Teams_TeamID",
                        column: x => x.TeamID,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TeamPlayers",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Active = table.Column<bool>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: false),
                    PlayerID = table.Column<Guid>(nullable: false),
                    TeamID = table.Column<Guid>(nullable: false),
                    IsPrincipal = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeamPlayers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TeamPlayers_Players_PlayerID",
                        column: x => x.PlayerID,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TeamPlayers_Teams_TeamID",
                        column: x => x.TeamID,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "Active", "CreatedDate", "ModifiedDate", "Name" },
                values: new object[,]
                {
                    { new Guid("66300219-e7f6-4f17-a859-d8cc11315796"), true, new DateTime(2020, 7, 4, 13, 55, 0, 0, DateTimeKind.Unspecified), new DateTime(2020, 7, 4, 13, 55, 0, 0, DateTimeKind.Unspecified), "Administrador" },
                    { new Guid("2f743547-6ab3-4f99-93a0-457eab81fecf"), true, new DateTime(2020, 7, 4, 13, 55, 0, 0, DateTimeKind.Unspecified), new DateTime(2020, 7, 4, 13, 55, 0, 0, DateTimeKind.Unspecified), "Auditor" }
                });

            migrationBuilder.InsertData(
                table: "Setups",
                columns: new[] { "Id", "Active", "CreatedDate", "Key", "ModifiedDate", "Value" },
                values: new object[,]
                {
                    { new Guid("0707aff8-0901-4f67-bfa4-52a19bbe4237"), true, new DateTime(2020, 7, 4, 13, 55, 0, 0, DateTimeKind.Unspecified), "BannerHome", new DateTime(2020, 7, 4, 13, 55, 0, 0, DateTimeKind.Unspecified), "" },
                    { new Guid("7e5b0a5b-80e8-43ea-8cb1-0b6d8a302a6d"), true, new DateTime(2020, 7, 4, 13, 55, 0, 0, DateTimeKind.Unspecified), "HomeTitle", new DateTime(2020, 7, 4, 13, 55, 0, 0, DateTimeKind.Unspecified), "" },
                    { new Guid("8e0ef355-af58-46ca-80a3-d76c4b68927c"), true, new DateTime(2020, 7, 4, 13, 55, 0, 0, DateTimeKind.Unspecified), "ResponsibilityTerm", new DateTime(2020, 7, 4, 13, 55, 0, 0, DateTimeKind.Unspecified), "" },
                    { new Guid("c8578be5-ead7-48cc-aac0-cf33a97fca04"), true, new DateTime(2020, 7, 4, 13, 55, 0, 0, DateTimeKind.Unspecified), "Regulation", new DateTime(2020, 7, 4, 13, 55, 0, 0, DateTimeKind.Unspecified), "Regras" },
                    { new Guid("f970ccaa-5a0d-41dd-a302-eab3072f8c09"), true, new DateTime(2020, 7, 4, 13, 55, 0, 0, DateTimeKind.Unspecified), "Logo", new DateTime(2020, 7, 4, 13, 55, 0, 0, DateTimeKind.Unspecified), "" },
                    { new Guid("8aae9ff9-48ae-4222-a144-0abf5070d798"), true, new DateTime(2020, 7, 4, 13, 55, 0, 0, DateTimeKind.Unspecified), "NossoNumero", new DateTime(2020, 7, 4, 13, 55, 0, 0, DateTimeKind.Unspecified), "" },
                    { new Guid("e8989e76-346f-457d-ba42-5e669e1eec84"), true, new DateTime(2020, 7, 4, 13, 55, 0, 0, DateTimeKind.Unspecified), "ShippingFile", new DateTime(2020, 7, 4, 13, 55, 0, 0, DateTimeKind.Unspecified), "" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Active", "CreatedDate", "Email", "IsMaster", "ModifiedDate", "Name", "Password", "RoleID" },
                values: new object[] { new Guid("0f054b3e-aaf0-44ae-a2af-4d1f1fa69b02"), true, new DateTime(2020, 7, 4, 13, 55, 0, 0, DateTimeKind.Unspecified), "master@master.com", true, new DateTime(2020, 7, 4, 13, 55, 0, 0, DateTimeKind.Unspecified), "Master", "$2b$10$CyWSzLpMjcCTOk8UHeiR8.ooyxWRYgf0GmwSDecx0I71M4h95QWHa", new Guid("66300219-e7f6-4f17-a859-d8cc11315796") });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Active", "CreatedDate", "Email", "IsMaster", "ModifiedDate", "Name", "Password", "RoleID" },
                values: new object[] { new Guid("d06fc2b3-d60b-4ad2-8794-829daa444506"), true, new DateTime(2020, 7, 4, 13, 55, 0, 0, DateTimeKind.Unspecified), "adm@adm.com", false, new DateTime(2020, 7, 4, 13, 55, 0, 0, DateTimeKind.Unspecified), "Administrator", "$2b$10$aYRoeIG4CST.BCseIAJ3PO1oLw1ThAIi9hpMOIoSyKmqQukRRmQbi", new Guid("66300219-e7f6-4f17-a859-d8cc11315796") });

            migrationBuilder.CreateIndex(
                name: "IX_Condominiums_Number",
                table: "Condominiums",
                column: "Number");

            migrationBuilder.CreateIndex(
                name: "IX_Condominiums_ZipCode",
                table: "Condominiums",
                column: "ZipCode");

            migrationBuilder.CreateIndex(
                name: "IX_Matches_AuditorID",
                table: "Matches",
                column: "AuditorID");

            migrationBuilder.CreateIndex(
                name: "IX_Matches_Player1ID",
                table: "Matches",
                column: "Player1ID");

            migrationBuilder.CreateIndex(
                name: "IX_Matches_Player2ID",
                table: "Matches",
                column: "Player2ID");

            migrationBuilder.CreateIndex(
                name: "IX_Matches_PlayerId",
                table: "Matches",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_Matches_TournamentID",
                table: "Matches",
                column: "TournamentID");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_TeamID",
                table: "Payments",
                column: "TeamID");

            migrationBuilder.CreateIndex(
                name: "IX_Players_CPF",
                table: "Players",
                column: "CPF",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Roles_Name",
                table: "Roles",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Setups_Key",
                table: "Setups",
                column: "Key",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TeamPlayers_PlayerID",
                table: "TeamPlayers",
                column: "PlayerID");

            migrationBuilder.CreateIndex(
                name: "IX_TeamPlayers_TeamID",
                table: "TeamPlayers",
                column: "TeamID");

            migrationBuilder.CreateIndex(
                name: "IX_Teams_CondominiumID",
                table: "Teams",
                column: "CondominiumID");

            migrationBuilder.CreateIndex(
                name: "IX_Teams_TournamentID",
                table: "Teams",
                column: "TournamentID");

            migrationBuilder.CreateIndex(
                name: "IX_Tournaments_GameID",
                table: "Tournaments",
                column: "GameID");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_RoleID",
                table: "Users",
                column: "RoleID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Matches");

            migrationBuilder.DropTable(
                name: "Payments");

            migrationBuilder.DropTable(
                name: "Setups");

            migrationBuilder.DropTable(
                name: "TeamPlayers");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Players");

            migrationBuilder.DropTable(
                name: "Teams");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Condominiums");

            migrationBuilder.DropTable(
                name: "Tournaments");

            migrationBuilder.DropTable(
                name: "Games");
        }
    }
}
