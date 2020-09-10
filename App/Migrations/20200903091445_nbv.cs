using Microsoft.EntityFrameworkCore.Migrations;

namespace PPE.Migrations
{
    public partial class nbv : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "NBV",
                table: "ppe_register",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Proceed",
                table: "ppe_register",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ProposedDisposalDate",
                table: "ppe_register",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReasonForDisposal",
                table: "ppe_register",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RequestDate",
                table: "ppe_register",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NBV",
                table: "ppe_register");

            migrationBuilder.DropColumn(
                name: "Proceed",
                table: "ppe_register");

            migrationBuilder.DropColumn(
                name: "ProposedDisposalDate",
                table: "ppe_register");

            migrationBuilder.DropColumn(
                name: "ReasonForDisposal",
                table: "ppe_register");

            migrationBuilder.DropColumn(
                name: "RequestDate",
                table: "ppe_register");
        }
    }
}
