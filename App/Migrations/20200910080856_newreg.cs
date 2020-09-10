using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PPE.Migrations
{
    public partial class newreg : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NBV",
                table: "ppe_register");

            migrationBuilder.AlterColumn<DateTime>(
                name: "RequestDate",
                table: "ppe_register",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ProposedDisposalDate",
                table: "ppe_register",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "RequestDate",
                table: "ppe_register",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(DateTime));

            migrationBuilder.AlterColumn<string>(
                name: "ProposedDisposalDate",
                table: "ppe_register",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(DateTime));

            migrationBuilder.AddColumn<int>(
                name: "NBV",
                table: "ppe_register",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
