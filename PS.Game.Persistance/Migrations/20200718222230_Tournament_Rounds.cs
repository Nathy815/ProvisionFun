using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PS.Game.Persistance.Migrations
{
    public partial class Tournament_Rounds : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "Tournaments");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Matches");

            migrationBuilder.AddColumn<int>(
                name: "RoundSolo",
                table: "Tournaments",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RoundTeam",
                table: "Tournaments",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Round",
                table: "Matches",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("0f054b3e-aaf0-44ae-a2af-4d1f1fa69b02"),
                column: "Password",
                value: "$2b$10$pULpx4mnTZwloXK5uW5JuugokJih/WzBZkABdRO/e/S8n3a1HfYUC");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("d06fc2b3-d60b-4ad2-8794-829daa444506"),
                column: "Password",
                value: "$2b$10$.AM41bg9SFHPqGWCw6398.9BlfKsibsdNUQVqYlkW/6LrDrJIEYn.");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RoundSolo",
                table: "Tournaments");

            migrationBuilder.DropColumn(
                name: "RoundTeam",
                table: "Tournaments");

            migrationBuilder.DropColumn(
                name: "Round",
                table: "Matches");

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "Tournaments",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "Matches",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("0f054b3e-aaf0-44ae-a2af-4d1f1fa69b02"),
                column: "Password",
                value: "$2b$10$CyWSzLpMjcCTOk8UHeiR8.ooyxWRYgf0GmwSDecx0I71M4h95QWHa");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("d06fc2b3-d60b-4ad2-8794-829daa444506"),
                column: "Password",
                value: "$2b$10$aYRoeIG4CST.BCseIAJ3PO1oLw1ThAIi9hpMOIoSyKmqQukRRmQbi");
        }
    }
}
