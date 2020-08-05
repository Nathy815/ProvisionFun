using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PS.Game.Persistance.Migrations
{
    public partial class Player_AddCellphone : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("0f054b3e-aaf0-44ae-a2af-4d1f1fa69b02"),
                column: "Password",
                value: "$2b$10$mdXlj2xksIUjcfGvEFNi5eqZDO2UfuQBpn7591zuLm0Tzhi9sukJa");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("d06fc2b3-d60b-4ad2-8794-829daa444506"),
                column: "Password",
                value: "$2b$10$mTbwIekgJcdw/DTFBj6E9u9ELHieA9xWfnuoOrp1FPJFWN2HDf.Ii");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("0f054b3e-aaf0-44ae-a2af-4d1f1fa69b02"),
                column: "Password",
                value: "$2b$10$h4D1xK9WzPBUHfANqKTv7.ACXP5kRBUbMwAh0Ev36JF5LX9VLiNIK");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("d06fc2b3-d60b-4ad2-8794-829daa444506"),
                column: "Password",
                value: "$2b$10$ukoTphMn3afdKmI9D10p0uUct3gnmdpilXrM59tnjvuFdJ5P8LXmS");
        }
    }
}
