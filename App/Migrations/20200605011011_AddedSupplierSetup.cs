using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Puchase_and_payables.Migrations
{
    public partial class AddedSupplierSetup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GL",
                table: "cor_suppliertype",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "cor_serviceterms",
                columns: table => new
                {
                    ServiceTermsId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true),
                    CompanyId = table.Column<int>(nullable: false),
                    Header = table.Column<string>(nullable: true),
                    Content = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cor_serviceterms", x => x.ServiceTermsId);
                });

            migrationBuilder.CreateTable(
                name: "cor_tasksetup",
                columns: table => new
                {
                    TaskSetupId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true),
                    CompanyId = table.Column<int>(nullable: false),
                    Percentage = table.Column<double>(nullable: false),
                    Type = table.Column<string>(nullable: true),
                    SubGL = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cor_tasksetup", x => x.TaskSetupId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "cor_serviceterms");

            migrationBuilder.DropTable(
                name: "cor_tasksetup");

            migrationBuilder.DropColumn(
                name: "GL",
                table: "cor_suppliertype");
        }
    }
}
