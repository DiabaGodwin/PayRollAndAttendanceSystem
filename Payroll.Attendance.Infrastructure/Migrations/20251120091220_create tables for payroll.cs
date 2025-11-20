using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Payroll.Attendance.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class createtablesforpayroll : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Payperiod",
                table: "Payrolls",
                newName: "PayPeriod");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "Payrolls",
                newName: "PayrollStatus");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Users",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2025, 11, 20, 9, 12, 18, 952, DateTimeKind.Utc).AddTicks(9190),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2025, 11, 13, 10, 52, 39, 828, DateTimeKind.Utc).AddTicks(1587));

            migrationBuilder.AlterColumn<string>(
                name: "PayslipPath",
                table: "Payrolls",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "EmploymentType",
                table: "Payrolls",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Payrolls",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "Month",
                table: "Payrolls",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Year",
                table: "Payrolls",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmploymentType",
                table: "Payrolls");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Payrolls");

            migrationBuilder.DropColumn(
                name: "Month",
                table: "Payrolls");

            migrationBuilder.DropColumn(
                name: "Year",
                table: "Payrolls");

            migrationBuilder.RenameColumn(
                name: "PayPeriod",
                table: "Payrolls",
                newName: "Payperiod");

            migrationBuilder.RenameColumn(
                name: "PayrollStatus",
                table: "Payrolls",
                newName: "Status");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Users",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2025, 11, 13, 10, 52, 39, 828, DateTimeKind.Utc).AddTicks(1587),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2025, 11, 20, 9, 12, 18, 952, DateTimeKind.Utc).AddTicks(9190));

            migrationBuilder.AlterColumn<string>(
                name: "PayslipPath",
                table: "Payrolls",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
