using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PS.Game.Persistance.Migrations
{
    public partial class Team_CancellationComments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CancellationComments",
                table: "Teams",
                nullable: true);

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CancellationComments",
                table: "Teams");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("0f054b3e-aaf0-44ae-a2af-4d1f1fa69b02"),
                column: "Password",
                value: "$2b$10$4TP60E357ROg2ddvROjW5esNVgojh4PZLJzHZFDPssOQxVDl9yzqi");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("d06fc2b3-d60b-4ad2-8794-829daa444506"),
                column: "Password",
                value: "$2b$10$ZvMhONF.gCWTxE1QcTCxsOkKTthh7yfdVdNF7XomEfyNXaSiTSazq");
        }
    }
}
