using Microsoft.EntityFrameworkCore.Migrations;

namespace PPE.Migrations
{
    public partial class proposed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProposedResidualValue",
                table: "ppe_register",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ProposedUsefulLife",
                table: "ppe_register",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RemainingUsefulLife",
                table: "ppe_register",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProposedResidualValue",
                table: "ppe_register");

            migrationBuilder.DropColumn(
                name: "ProposedUsefulLife",
                table: "ppe_register");

            migrationBuilder.DropColumn(
                name: "RemainingUsefulLife",
                table: "ppe_register");
        }
    }
}
