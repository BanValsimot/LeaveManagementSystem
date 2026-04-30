using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LeaveManagementSystem.Web.Data.Migrations
{
    /// <inheritdoc />
    public partial class FixSeeding : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "56f05747-4972-49dd-8d67-941fdcc08825",
                columns: new[] { "ConcurrencyStamp", "NormalizedEmail", "PasswordHash", "SecurityStamp" },
                values: new object[] { "ff7a9976-2b37-4ce0-974b-b3fad94c3c47", "ADMIN@LOCALHOST.COM", "AQAAAAIAAYagAAAAEF2UByBaWBEyzJkhiGTxb12vt5bVxw4xiRu4efLyRbjqY62ZkbMvFR8XTK8xxebBDg==", "48c922ed-5f33-4d16-8906-e19d6469a6b2" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "56f05747-4972-49dd-8d67-941fdcc08825",
                columns: new[] { "ConcurrencyStamp", "NormalizedEmail", "PasswordHash", "SecurityStamp" },
                values: new object[] { "d44951cf-9c4a-4303-9d3e-834603da9136", "", "AQAAAAIAAYagAAAAEG1aHZGVjUzK96PRQ4Hlb4V9OFCULr/iH2l63HpNHKkcy3HF0hLL0dTkG0cs4726oQ==", "058905c2-3ee6-451d-88ae-7a05fbc49454" });
        }
    }
}
