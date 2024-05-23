using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace IoTControlTower.Infra.Migrations
{
    /// <inheritdoc />
    public partial class DB_v6 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3820e684-a6d4-4e93-90ec-d3f5593b7436");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d0c74314-c6e9-40cb-af20-b934c93137e7");

            migrationBuilder.AlterColumn<DateTime>(
                name: "RegistrationDate",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdateDate",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "17ca17d0-ea7e-4a5b-9ec0-94b6723478be", "1", "Admin", "Admin" },
                    { "f87b9c34-3562-455d-89f4-fe64920abc7f", "2", "User", "User" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "17ca17d0-ea7e-4a5b-9ec0-94b6723478be");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f87b9c34-3562-455d-89f4-fe64920abc7f");

            migrationBuilder.DropColumn(
                name: "UpdateDate",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<DateTime>(
                name: "RegistrationDate",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "3820e684-a6d4-4e93-90ec-d3f5593b7436", "2", "User", "User" },
                    { "d0c74314-c6e9-40cb-af20-b934c93137e7", "1", "Admin", "Admin" }
                });
        }
    }
}
