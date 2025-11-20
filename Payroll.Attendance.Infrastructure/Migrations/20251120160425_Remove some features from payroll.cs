using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Payroll.Attendance.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Removesomefeaturesfrompayroll : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Users",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2025, 11, 20, 16, 4, 25, 175, DateTimeKind.Utc).AddTicks(6923),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2025, 11, 20, 16, 2, 49, 776, DateTimeKind.Utc).AddTicks(6401));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Users",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2025, 11, 20, 16, 2, 49, 776, DateTimeKind.Utc).AddTicks(6401),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2025, 11, 20, 16, 4, 25, 175, DateTimeKind.Utc).AddTicks(6923));
        }
    }
}
