using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PPE.Migrations
{
    public partial class disposal : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ApprovalStatusId",
                table: "ppe_register",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "WorkflowToken",
                table: "ppe_register",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ppe_disposal",
                columns: table => new
                {
                    DisposalId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AssetNumber = table.Column<string>(maxLength: 50, nullable: true),
                    AssetClassificationId = table.Column<int>(nullable: false),
                    Description = table.Column<string>(maxLength: 50, nullable: true),
                    Cost = table.Column<decimal>(nullable: false),
                    DateOfPurchaase = table.Column<DateTime>(nullable: false),
                    Quantity = table.Column<int>(nullable: false),
                    DepreciationStartDate = table.Column<DateTime>(nullable: false),
                    UsefulLife = table.Column<int>(nullable: false),
                    ResidualValue = table.Column<decimal>(nullable: false),
                    Location = table.Column<string>(maxLength: 500, nullable: true),
                    DepreciationForThePeriod = table.Column<decimal>(nullable: false),
                    AccumulatedDepreciation = table.Column<decimal>(nullable: false),
                    NetBookValue = table.Column<decimal>(nullable: false),
                    ProceedFromDisposal = table.Column<decimal>(nullable: false),
                    ReasonForDisposal = table.Column<string>(maxLength: 500, nullable: true),
                    RequestDate = table.Column<DateTime>(nullable: false),
                    ProposedDisposalDate = table.Column<DateTime>(nullable: false),
                    Active = table.Column<bool>(nullable: false),
                    Deleted = table.Column<bool>(nullable: false),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    UpdatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: false),
                    ApprovalStatusId = table.Column<int>(nullable: false),
                    WorkflowToken = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ppe_disposal", x => x.DisposalId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ppe_disposal");

            migrationBuilder.DropColumn(
                name: "ApprovalStatusId",
                table: "ppe_register");

            migrationBuilder.DropColumn(
                name: "WorkflowToken",
                table: "ppe_register");
        }
    }
}
