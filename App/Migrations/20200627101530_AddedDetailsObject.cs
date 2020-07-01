using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PPE.Migrations
{
    public partial class AddedDetailsObject : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "cor_approvaldetail",
                columns: table => new
                {
                    ApprovalDetailId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StatusId = table.Column<int>(nullable: false),
                    StaffId = table.Column<int>(nullable: false),
                    Comment = table.Column<string>(nullable: true),
                    Date = table.Column<DateTime>(nullable: false),
                    TargetId = table.Column<int>(nullable: false),
                    WorkflowToken = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cor_approvaldetail", x => x.ApprovalDetailId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "cor_approvaldetail");
        }
    }
}
