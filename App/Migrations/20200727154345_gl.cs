using Microsoft.EntityFrameworkCore.Migrations;

namespace PPE.Migrations
{
    public partial class gl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SubGlAccumulatedDepreciation",
                table: "ppe_register",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SubGlAddition",
                table: "ppe_register",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SubGlDepreciation",
                table: "ppe_register",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SubGlDisposal",
                table: "ppe_register",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SubGlAccumulatedDepreciation",
                table: "ppe_reassessment",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SubGlDepreciation",
                table: "ppe_reassessment",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SubGlDisposal",
                table: "ppe_reassessment",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SubGlAccumulatedDepreciation",
                table: "ppe_disposal",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SubGlDepreciation",
                table: "ppe_disposal",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SubGlDisposal",
                table: "ppe_disposal",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SubGlAccumulatedDepreciation",
                table: "ppe_additionform",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SubGlDepreciation",
                table: "ppe_additionform",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SubGlDisposal",
                table: "ppe_additionform",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SubGlAccumulatedDepreciation",
                table: "ppe_register");

            migrationBuilder.DropColumn(
                name: "SubGlAddition",
                table: "ppe_register");

            migrationBuilder.DropColumn(
                name: "SubGlDepreciation",
                table: "ppe_register");

            migrationBuilder.DropColumn(
                name: "SubGlDisposal",
                table: "ppe_register");

            migrationBuilder.DropColumn(
                name: "SubGlAccumulatedDepreciation",
                table: "ppe_reassessment");

            migrationBuilder.DropColumn(
                name: "SubGlDepreciation",
                table: "ppe_reassessment");

            migrationBuilder.DropColumn(
                name: "SubGlDisposal",
                table: "ppe_reassessment");

            migrationBuilder.DropColumn(
                name: "SubGlAccumulatedDepreciation",
                table: "ppe_disposal");

            migrationBuilder.DropColumn(
                name: "SubGlDepreciation",
                table: "ppe_disposal");

            migrationBuilder.DropColumn(
                name: "SubGlDisposal",
                table: "ppe_disposal");

            migrationBuilder.DropColumn(
                name: "SubGlAccumulatedDepreciation",
                table: "ppe_additionform");

            migrationBuilder.DropColumn(
                name: "SubGlDepreciation",
                table: "ppe_additionform");

            migrationBuilder.DropColumn(
                name: "SubGlDisposal",
                table: "ppe_additionform");
        }
    }
}
