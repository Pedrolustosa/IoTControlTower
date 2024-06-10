using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace IoTControlTower.Infra.Migrations
{
    /// <inheritdoc />
    public partial class DB_v11 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d9809e97-c5ba-40ee-9bab-e8ad404ee531");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "fc0aa34e-560c-4d90-ad0f-7c05e4daa754");

            migrationBuilder.AddColumn<string>(
                name: "AlarmSettings",
                table: "Devices",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ConnectionType",
                table: "Devices",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "HealthStatus",
                table: "Devices",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "InstallationDate",
                table: "Devices",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastHealthCheckDate",
                table: "Devices",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastKnownStatus",
                table: "Devices",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastMaintenanceDate",
                table: "Devices",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MaintenanceHistory",
                table: "Devices",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ManufactureDate",
                table: "Devices",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Owner",
                table: "Devices",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SensorType",
                table: "Devices",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SerialNumber",
                table: "Devices",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "242eed9b-2d69-4dd4-9bc9-0bb1a26e5237", "2", "User", "User" },
                    { "5d4d94eb-eeff-447d-aaf2-a4aa271d94dc", "1", "Admin", "Admin" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "242eed9b-2d69-4dd4-9bc9-0bb1a26e5237");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5d4d94eb-eeff-447d-aaf2-a4aa271d94dc");

            migrationBuilder.DropColumn(
                name: "AlarmSettings",
                table: "Devices");

            migrationBuilder.DropColumn(
                name: "ConnectionType",
                table: "Devices");

            migrationBuilder.DropColumn(
                name: "HealthStatus",
                table: "Devices");

            migrationBuilder.DropColumn(
                name: "InstallationDate",
                table: "Devices");

            migrationBuilder.DropColumn(
                name: "LastHealthCheckDate",
                table: "Devices");

            migrationBuilder.DropColumn(
                name: "LastKnownStatus",
                table: "Devices");

            migrationBuilder.DropColumn(
                name: "LastMaintenanceDate",
                table: "Devices");

            migrationBuilder.DropColumn(
                name: "MaintenanceHistory",
                table: "Devices");

            migrationBuilder.DropColumn(
                name: "ManufactureDate",
                table: "Devices");

            migrationBuilder.DropColumn(
                name: "Owner",
                table: "Devices");

            migrationBuilder.DropColumn(
                name: "SensorType",
                table: "Devices");

            migrationBuilder.DropColumn(
                name: "SerialNumber",
                table: "Devices");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "d9809e97-c5ba-40ee-9bab-e8ad404ee531", "2", "User", "User" },
                    { "fc0aa34e-560c-4d90-ad0f-7c05e4daa754", "1", "Admin", "Admin" }
                });
        }
    }
}
