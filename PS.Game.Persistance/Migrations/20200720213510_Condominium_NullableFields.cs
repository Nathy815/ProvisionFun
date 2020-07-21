using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PS.Game.Persistance.Migrations
{
    public partial class Condominium_NullableFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ZipCode",
                table: "Condominiums",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "Number",
                table: "Condominiums",
                nullable: true,
                oldClrType: typeof(string));

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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ZipCode",
                table: "Condominiums",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Number",
                table: "Condominiums",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

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
    }
}
