using Microsoft.EntityFrameworkCore.Migrations;

namespace PPE.Migrations
{
    public partial class residualvalue : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProposedResidualValue",
                table: "ppe_reassessment",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProposedResidualValue",
                table: "ppe_reassessment");
        }
    }
}
