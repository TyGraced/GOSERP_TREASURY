using Microsoft.EntityFrameworkCore.Migrations;

namespace PPE.Migrations
{
    public partial class approvalstatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ApprovalStatusId",
                table: "ppe_reassessment",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "WorkflowToken",
                table: "ppe_reassessment",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApprovalStatusId",
                table: "ppe_reassessment");

            migrationBuilder.DropColumn(
                name: "WorkflowToken",
                table: "ppe_reassessment");
        }
    }
}
