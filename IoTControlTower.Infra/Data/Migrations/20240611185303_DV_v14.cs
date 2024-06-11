using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace IoTControlTower.Infra.Migrations
{
    /// <inheritdoc />
    public partial class DV_v14 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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
                    { "59dfeeb3-9cbc-4ef3-8467-eb91642c68dd", "2", "User", "User" },
                    { "ced7f024-9ccd-4041-bbba-fdc1ec63457f", "1", "Admin", "Admin" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "59dfeeb3-9cbc-4ef3-8467-eb91642c68dd");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ced7f024-9ccd-4041-bbba-fdc1ec63457f");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "137c9b7e-4c6d-492b-895c-d186c314ab09", "2", "User", "User" },
                    { "2b6c8507-2e00-4930-a59d-8aa7fe68fad5", "1", "Admin", "Admin" }
                });
        }
    }
}
