using Microsoft.EntityFrameworkCore.Migrations;

namespace PPE.Migrations
{
    public partial class companyif : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "ppe_register",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "ppe_additionform",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "ppe_register");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "ppe_additionform");
        }
    }
}
