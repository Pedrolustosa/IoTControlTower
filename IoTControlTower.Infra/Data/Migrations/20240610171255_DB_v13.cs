using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace IoTControlTower.Infra.Migrations
{
    /// <inheritdoc />
    public partial class DB_v13 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "55c8c0d3-cb10-4327-b081-7a6dd9b7f079");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "cc966dbd-e69a-478d-bc24-64b82ac6a255");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "137c9b7e-4c6d-492b-895c-d186c314ab09", "2", "User", "User" },
                    { "2b6c8507-2e00-4930-a59d-8aa7fe68fad5", "1", "Admin", "Admin" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "137c9b7e-4c6d-492b-895c-d186c314ab09");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2b6c8507-2e00-4930-a59d-8aa7fe68fad5");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "55c8c0d3-cb10-4327-b081-7a6dd9b7f079", "2", "User", "User" },
                    { "cc966dbd-e69a-478d-bc24-64b82ac6a255", "1", "Admin", "Admin" }
                });
        }
    }
}
