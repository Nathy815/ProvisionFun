using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PS.Game.Persistance.Migrations
{
    public partial class Team_CancellationSent : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "CancellationSent",
                table: "Teams",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("0f054b3e-aaf0-44ae-a2af-4d1f1fa69b02"),
                column: "Password",
                value: "$2b$10$rQIR35rdgoOGxRrBp64LNePDn59FgwHYByr1P4Xo5eD9wxNjJocAe");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("d06fc2b3-d60b-4ad2-8794-829daa444506"),
                column: "Password",
                value: "$2b$10$ooYzZ.LEF8yOciUetR2E9e2zRkARr/EnWDsnVKUpM2PabUAqGtkjS");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CancellationSent",
                table: "Teams");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("0f054b3e-aaf0-44ae-a2af-4d1f1fa69b02"),
                column: "Password",
                value: "$2b$10$84oIbHyNaz/dkums7hxVo.bQUWHx5tuRrdNb26sxO2zZezd3ZnXTC");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("d06fc2b3-d60b-4ad2-8794-829daa444506"),
                column: "Password",
                value: "$2b$10$iG2orXoqqa0zMmVx57H1S.e4x0jjD/JTl/u0B/lkPeLsolJqEvRIC");
        }
    }
}
