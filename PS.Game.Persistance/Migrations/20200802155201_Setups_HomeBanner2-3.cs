using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PS.Game.Persistance.Migrations
{
    public partial class Setups_HomeBanner23 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Setups",
                columns: new[] { "Id", "Active", "CreatedDate", "Key", "ModifiedDate", "Value" },
                values: new object[,]
                {
                    { new Guid("b3a92942-b10c-4a82-bc2e-e58627ee7010"), true, new DateTime(2020, 7, 4, 13, 55, 0, 0, DateTimeKind.Unspecified), "HomeBanner2", new DateTime(2020, 7, 4, 13, 55, 0, 0, DateTimeKind.Unspecified), "" },
                    { new Guid("e180712c-a3a9-489b-ac61-2a64defa8c34"), true, new DateTime(2020, 7, 4, 13, 55, 0, 0, DateTimeKind.Unspecified), "HomeBanner3", new DateTime(2020, 7, 4, 13, 55, 0, 0, DateTimeKind.Unspecified), "" }
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("0f054b3e-aaf0-44ae-a2af-4d1f1fa69b02"),
                column: "Password",
                value: "$2b$10$0k4TbDmVPRUS7/Jhc89Ntu5P7yR6N4QzH/7ud6AUYMcC3eCrG4Wle");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("d06fc2b3-d60b-4ad2-8794-829daa444506"),
                column: "Password",
                value: "$2b$10$.zBuJUiHuZzBq/g6j6JXTOkDwX8zULroFE6cGE5gy2KO10k7qVlBq");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Setups",
                keyColumn: "Id",
                keyValue: new Guid("b3a92942-b10c-4a82-bc2e-e58627ee7010"));

            migrationBuilder.DeleteData(
                table: "Setups",
                keyColumn: "Id",
                keyValue: new Guid("e180712c-a3a9-489b-ac61-2a64defa8c34"));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("0f054b3e-aaf0-44ae-a2af-4d1f1fa69b02"),
                column: "Password",
                value: "$2b$10$CIkLiATX3FidNMnFgj8nt.ZmnXaZe9Jlh1vEQPJ7eXZmfDsZ4KZU2");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("d06fc2b3-d60b-4ad2-8794-829daa444506"),
                column: "Password",
                value: "$2b$10$ppPWAXTO5OSr/1YDhiuDDOrJTEie79ZkJStJyfomQSRETs3nG6dBC");
        }
    }
}
