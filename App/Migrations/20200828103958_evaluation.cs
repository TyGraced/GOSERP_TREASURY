using Microsoft.EntityFrameworkCore.Migrations;

namespace PPE.Migrations
{
    public partial class evaluation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "ProposedResidualValue",
                table: "ppe_register",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<decimal>(
                name: "ReEvaluatedCost",
                table: "ppe_register",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReEvaluatedCost",
                table: "ppe_register");

            migrationBuilder.AlterColumn<int>(
                name: "ProposedResidualValue",
                table: "ppe_register",
                type: "int",
                nullable: false,
                oldClrType: typeof(decimal));
        }
    }
}
