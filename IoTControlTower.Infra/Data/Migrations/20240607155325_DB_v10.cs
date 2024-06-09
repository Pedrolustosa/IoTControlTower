using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace IoTControlTower.Infra.Migrations
{
    /// <inheritdoc />
    public partial class DB_v10 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "6b679ff5-766f-4ab2-9a74-4f82a39283e9");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "704ad174-a3d6-4a79-88ce-7f0dd5139661");

            migrationBuilder.AddColumn<string>(
                name: "FirmwareVersion",
                table: "Devices",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IpAddress",
                table: "Devices",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastCommunication",
                table: "Devices",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Location",
                table: "Devices",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "d9809e97-c5ba-40ee-9bab-e8ad404ee531", "2", "User", "User" },
                    { "fc0aa34e-560c-4d90-ad0f-7c05e4daa754", "1", "Admin", "Admin" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d9809e97-c5ba-40ee-9bab-e8ad404ee531");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "fc0aa34e-560c-4d90-ad0f-7c05e4daa754");

            migrationBuilder.DropColumn(
                name: "FirmwareVersion",
                table: "Devices");

            migrationBuilder.DropColumn(
                name: "IpAddress",
                table: "Devices");

            migrationBuilder.DropColumn(
                name: "LastCommunication",
                table: "Devices");

            migrationBuilder.DropColumn(
                name: "Location",
                table: "Devices");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "6b679ff5-766f-4ab2-9a74-4f82a39283e9", "1", "Admin", "Admin" },
                    { "704ad174-a3d6-4a79-88ce-7f0dd5139661", "2", "User", "User" }
                });
        }
    }
}
