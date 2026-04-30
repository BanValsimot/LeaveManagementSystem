using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LeaveManagementSystem.Web.Data.Migrations
{
    /// <inheritdoc />
    public partial class ExtendedUserTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateOnly>(
                name: "DateOfBirth",
                table: "AspNetUsers",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "56f05747-4972-49dd-8d67-941fdcc08825",
                columns: new[] { "ConcurrencyStamp", "DateOfBirth", "FirstName", "LastName", "PasswordHash", "SecurityStamp" },
                values: new object[] { "f3fd0ff4-25a2-4900-8e1d-c3088091c705", new DateOnly(1986, 11, 7), "Default", "Default", "AQAAAAIAAYagAAAAEBDKVmzZlSP0hhMej2DNcn3AQ0E5loevXwcIw75IDoGbABIZU4ja5TOp+JrYqtEDrw==", "ab33f95d-1b04-4625-bc1a-5c2ff8d3ec91" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateOfBirth",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "AspNetUsers");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "56f05747-4972-49dd-8d67-941fdcc08825",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "ff7a9976-2b37-4ce0-974b-b3fad94c3c47", "AQAAAAIAAYagAAAAEF2UByBaWBEyzJkhiGTxb12vt5bVxw4xiRu4efLyRbjqY62ZkbMvFR8XTK8xxebBDg==", "48c922ed-5f33-4d16-8906-e19d6469a6b2" });
        }
    }
}
