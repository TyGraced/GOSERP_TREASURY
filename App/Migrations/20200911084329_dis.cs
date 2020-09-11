using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PPE.Migrations
{
    public partial class dis : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ppe_periodicschedule");

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

            migrationBuilder.DropColumn(
                name: "DateOfPurchaase",
                table: "ppe_disposal");

            migrationBuilder.DropColumn(
                name: "DepreciationForThePeriod",
                table: "ppe_disposal");

            migrationBuilder.DropColumn(
                name: "Location",
                table: "ppe_disposal");

            migrationBuilder.DropColumn(
                name: "ProposedDisposalDate",
                table: "ppe_disposal");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "ppe_disposal");

            migrationBuilder.DropColumn(
                name: "ReasonForDisposal",
                table: "ppe_disposal");

            migrationBuilder.DropColumn(
                name: "RequestDate",
                table: "ppe_disposal");

            migrationBuilder.DropColumn(
                name: "ResidualValue",
                table: "ppe_disposal");

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
                name: "UsefulLife",
                table: "ppe_disposal");

            migrationBuilder.CreateTable(
                name: "ppe_derecognition",
                columns: table => new
                {
                    DerecognitionId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReasonForDisposal = table.Column<string>(nullable: true),
                    RequestDate = table.Column<DateTime>(nullable: false),
                    ProposedDisposalDate = table.Column<DateTime>(nullable: false),
                    ApprovalStatusId = table.Column<int>(nullable: false),
                    NBV = table.Column<decimal>(nullable: false),
                    WorkflowToken = table.Column<string>(nullable: true),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    Updatedby = table.Column<string>(maxLength: 50, nullable: true),
                    Createdby = table.Column<string>(maxLength: 50, nullable: true),
                    Updatedon = table.Column<DateTime>(nullable: true),
                    Createdon = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ppe_derecognition", x => x.DerecognitionId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ppe_derecognition");

            migrationBuilder.AddColumn<int>(
                name: "Proceed",
                table: "ppe_register",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "ProposedDisposalDate",
                table: "ppe_register",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "ReasonForDisposal",
                table: "ppe_register",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RequestDate",
                table: "ppe_register",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DateOfPurchaase",
                table: "ppe_disposal",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<decimal>(
                name: "DepreciationForThePeriod",
                table: "ppe_disposal",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "Location",
                table: "ppe_disposal",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ProposedDisposalDate",
                table: "ppe_disposal",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "ppe_disposal",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ReasonForDisposal",
                table: "ppe_disposal",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RequestDate",
                table: "ppe_disposal",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<decimal>(
                name: "ResidualValue",
                table: "ppe_disposal",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "SubGlAccumulatedDepreciation",
                table: "ppe_disposal",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SubGlDepreciation",
                table: "ppe_disposal",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SubGlDisposal",
                table: "ppe_disposal",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UsefulLife",
                table: "ppe_disposal",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "ppe_periodicschedule",
                columns: table => new
                {
                    PpePeriodicScheduleId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccumulatedDepreciation = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: true),
                    AdditionId = table.Column<int>(type: "int", nullable: false),
                    CB = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Createdby = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Createdon = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: true),
                    EndPeriod = table.Column<bool>(type: "bit", nullable: true),
                    MonthlyDepreciation = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    OB = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Period = table.Column<int>(type: "int", nullable: true),
                    PeriodDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PeriodId = table.Column<int>(type: "int", nullable: true),
                    Updatedby = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Updatedon = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ppe_periodicschedule", x => x.PpePeriodicScheduleId);
                });
        }
    }
}
