using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebAPI.Migrations
{
    /// <inheritdoc />
    public partial class ComplaintStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "ComplaintStatuses",
                keyColumn: "Id",
                keyValue: 1,
                column: "StatusName",
                value: "جديدة");

            migrationBuilder.UpdateData(
                table: "ComplaintStatuses",
                keyColumn: "Id",
                keyValue: 3,
                column: "StatusName",
                value: "منجزة");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "ComplaintStatuses",
                keyColumn: "Id",
                keyValue: 1,
                column: "StatusName",
                value: "قيد المراجعة");

            migrationBuilder.UpdateData(
                table: "ComplaintStatuses",
                keyColumn: "Id",
                keyValue: 3,
                column: "StatusName",
                value: "تم الحل");
        }
    }
}
