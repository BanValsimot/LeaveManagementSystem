using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace LeaveManagementSystem.Web.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeedRolesAndAdmin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "363b0dc6-6a9f-4aff-8e58-17d5bad0abed", null, "Employee", "EMPLOYEE" },
                    { "40fb5ecf-a2a8-452c-9ad4-baaafcc4725c", null, "Administrator", "ADMINISTRATOR" },
                    { "9f89515a-0c92-49d7-887a-9d8c537c3ab3", null, "Supervisor", "SUPERVISOR" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "56f05747-4972-49dd-8d67-941fdcc08825", 0, "d44951cf-9c4a-4303-9d3e-834603da9136", "admin@localhost.com", true, false, null, "", "ADMIN@LOCALHOST.COM", "AQAAAAIAAYagAAAAEG1aHZGVjUzK96PRQ4Hlb4V9OFCULr/iH2l63HpNHKkcy3HF0hLL0dTkG0cs4726oQ==", null, false, "058905c2-3ee6-451d-88ae-7a05fbc49454", false, "admin@localhost.com" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "40fb5ecf-a2a8-452c-9ad4-baaafcc4725c", "56f05747-4972-49dd-8d67-941fdcc08825" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "363b0dc6-6a9f-4aff-8e58-17d5bad0abed");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9f89515a-0c92-49d7-887a-9d8c537c3ab3");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "40fb5ecf-a2a8-452c-9ad4-baaafcc4725c", "56f05747-4972-49dd-8d67-941fdcc08825" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "40fb5ecf-a2a8-452c-9ad4-baaafcc4725c");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "56f05747-4972-49dd-8d67-941fdcc08825");
        }
    }
}
