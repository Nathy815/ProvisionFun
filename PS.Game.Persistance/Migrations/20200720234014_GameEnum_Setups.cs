using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PS.Game.Persistance.Migrations
{
    public partial class GameEnum_Setups : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tournaments_Games_GameID",
                table: "Tournaments");

            migrationBuilder.DropIndex(
                name: "IX_Tournaments_GameID",
                table: "Tournaments");

            migrationBuilder.DropColumn(
                name: "GameID",
                table: "Tournaments");

            migrationBuilder.AddColumn<int>(
                name: "Game",
                table: "Tournaments",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Setups",
                keyColumn: "Id",
                keyValue: new Guid("0707aff8-0901-4f67-bfa4-52a19bbe4237"),
                column: "Key",
                value: "HomeBanner");

            migrationBuilder.UpdateData(
                table: "Setups",
                keyColumn: "Id",
                keyValue: new Guid("8aae9ff9-48ae-4222-a144-0abf5070d798"),
                column: "Value",
                value: "5504");

            migrationBuilder.UpdateData(
                table: "Setups",
                keyColumn: "Id",
                keyValue: new Guid("e8989e76-346f-457d-ba42-5e669e1eec84"),
                column: "Value",
                value: "1");

            migrationBuilder.InsertData(
                table: "Setups",
                columns: new[] { "Id", "Active", "CreatedDate", "Key", "ModifiedDate", "Value" },
                values: new object[] { new Guid("e067f067-6ff3-48d2-813b-d4373b68bc54"), true, new DateTime(2020, 7, 4, 13, 55, 0, 0, DateTimeKind.Unspecified), "RegistryBanner", new DateTime(2020, 7, 4, 13, 55, 0, 0, DateTimeKind.Unspecified), "" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("0f054b3e-aaf0-44ae-a2af-4d1f1fa69b02"),
                column: "Password",
                value: "$2b$10$wdpKfBmlHOkmtj6t5JNk9OyM43mgJIMFSKqiFXh.8O3tI5eEe/VUG");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("d06fc2b3-d60b-4ad2-8794-829daa444506"),
                column: "Password",
                value: "$2b$10$kjIV5DUbdy11gRE5pgjsu.je6j53/7l1/4KZ1hH1OLUybNHD4R5.W");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Setups",
                keyColumn: "Id",
                keyValue: new Guid("e067f067-6ff3-48d2-813b-d4373b68bc54"));

            migrationBuilder.DropColumn(
                name: "Game",
                table: "Tournaments");

            migrationBuilder.AddColumn<Guid>(
                name: "GameID",
                table: "Tournaments",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.UpdateData(
                table: "Setups",
                keyColumn: "Id",
                keyValue: new Guid("0707aff8-0901-4f67-bfa4-52a19bbe4237"),
                column: "Key",
                value: "BannerHome");

            migrationBuilder.UpdateData(
                table: "Setups",
                keyColumn: "Id",
                keyValue: new Guid("8aae9ff9-48ae-4222-a144-0abf5070d798"),
                column: "Value",
                value: "");

            migrationBuilder.UpdateData(
                table: "Setups",
                keyColumn: "Id",
                keyValue: new Guid("e8989e76-346f-457d-ba42-5e669e1eec84"),
                column: "Value",
                value: "");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("0f054b3e-aaf0-44ae-a2af-4d1f1fa69b02"),
                column: "Password",
                value: "$2b$10$3RiW0GODe/YF2ZpnxWJFWOPRZ787zu4B6WwGZAv/c2releOoIwpga");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("d06fc2b3-d60b-4ad2-8794-829daa444506"),
                column: "Password",
                value: "$2b$10$0fGkrixt4k5teEXkqPuzietA1oUFdWFqrBuRGXpuwV87rqlZKBty2");

            migrationBuilder.CreateIndex(
                name: "IX_Tournaments_GameID",
                table: "Tournaments",
                column: "GameID");

            migrationBuilder.AddForeignKey(
                name: "FK_Tournaments_Games_GameID",
                table: "Tournaments",
                column: "GameID",
                principalTable: "Games",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
