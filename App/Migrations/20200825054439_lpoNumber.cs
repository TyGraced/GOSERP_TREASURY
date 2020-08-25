using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PPE.Migrations
{
    public partial class lpoNumber : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ppe_lpo",
                table: "ppe_lpo");

            migrationBuilder.DropColumn(
                name: "LPOId",
                table: "ppe_lpo");

            migrationBuilder.DropColumn(
                name: "Active",
                table: "ppe_lpo");

            migrationBuilder.DropColumn(
                name: "Cost",
                table: "ppe_lpo");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "ppe_lpo");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "ppe_lpo");

            migrationBuilder.DropColumn(
                name: "DateOfPurchase",
                table: "ppe_lpo");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "ppe_lpo");

            migrationBuilder.DropColumn(
                name: "UpdatedOn",
                table: "ppe_lpo");

            migrationBuilder.RenameColumn(
                name: "LpoNumber",
                table: "ppe_lpo",
                newName: "LPONumber");

            migrationBuilder.AlterColumn<string>(
                name: "LPONumber",
                table: "ppe_lpo",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "ppe_lpo",
                maxLength: 2000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PLPOId",
                table: "ppe_lpo",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "ppe_lpo",
                maxLength: 550,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "AmountPayable",
                table: "ppe_lpo",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "ApprovalStatusId",
                table: "ppe_lpo",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "BidAndTenderId",
                table: "ppe_lpo",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "BidComplete",
                table: "ppe_lpo",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "DebitGl",
                table: "ppe_lpo",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeliveryDate",
                table: "ppe_lpo",
                type: "date",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<decimal>(
                name: "GrossAmount",
                table: "ppe_lpo",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "JobStatus",
                table: "ppe_lpo",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "ppe_lpo",
                maxLength: 250,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "PurchaseReqNoteId",
                table: "ppe_lpo",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "RequestDate",
                table: "ppe_lpo",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "ServiceTerm",
                table: "ppe_lpo",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "SupplierAddress",
                table: "ppe_lpo",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SupplierIds",
                table: "ppe_lpo",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SupplierNumber",
                table: "ppe_lpo",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Tax",
                table: "ppe_lpo",
                type: "money",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "Taxes",
                table: "ppe_lpo",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Total",
                table: "ppe_lpo",
                type: "money",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "WinnerSupplierId",
                table: "ppe_lpo",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "WorkflowToken",
                table: "ppe_lpo",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ppe_lpo",
                table: "ppe_lpo",
                column: "PLPOId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ppe_lpo",
                table: "ppe_lpo");

            migrationBuilder.DropColumn(
                name: "PLPOId",
                table: "ppe_lpo");

            migrationBuilder.DropColumn(
                name: "Address",
                table: "ppe_lpo");

            migrationBuilder.DropColumn(
                name: "AmountPayable",
                table: "ppe_lpo");

            migrationBuilder.DropColumn(
                name: "ApprovalStatusId",
                table: "ppe_lpo");

            migrationBuilder.DropColumn(
                name: "BidAndTenderId",
                table: "ppe_lpo");

            migrationBuilder.DropColumn(
                name: "BidComplete",
                table: "ppe_lpo");

            migrationBuilder.DropColumn(
                name: "DebitGl",
                table: "ppe_lpo");

            migrationBuilder.DropColumn(
                name: "DeliveryDate",
                table: "ppe_lpo");

            migrationBuilder.DropColumn(
                name: "GrossAmount",
                table: "ppe_lpo");

            migrationBuilder.DropColumn(
                name: "JobStatus",
                table: "ppe_lpo");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "ppe_lpo");

            migrationBuilder.DropColumn(
                name: "PurchaseReqNoteId",
                table: "ppe_lpo");

            migrationBuilder.DropColumn(
                name: "RequestDate",
                table: "ppe_lpo");

            migrationBuilder.DropColumn(
                name: "ServiceTerm",
                table: "ppe_lpo");

            migrationBuilder.DropColumn(
                name: "SupplierAddress",
                table: "ppe_lpo");

            migrationBuilder.DropColumn(
                name: "SupplierIds",
                table: "ppe_lpo");

            migrationBuilder.DropColumn(
                name: "SupplierNumber",
                table: "ppe_lpo");

            migrationBuilder.DropColumn(
                name: "Tax",
                table: "ppe_lpo");

            migrationBuilder.DropColumn(
                name: "Taxes",
                table: "ppe_lpo");

            migrationBuilder.DropColumn(
                name: "Total",
                table: "ppe_lpo");

            migrationBuilder.DropColumn(
                name: "WinnerSupplierId",
                table: "ppe_lpo");

            migrationBuilder.DropColumn(
                name: "WorkflowToken",
                table: "ppe_lpo");

            migrationBuilder.RenameColumn(
                name: "LPONumber",
                table: "ppe_lpo",
                newName: "LpoNumber");

            migrationBuilder.AlterColumn<string>(
                name: "LpoNumber",
                table: "ppe_lpo",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "ppe_lpo",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 2000,
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LPOId",
                table: "ppe_lpo",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<bool>(
                name: "Active",
                table: "ppe_lpo",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<decimal>(
                name: "Cost",
                table: "ppe_lpo",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "ppe_lpo",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "ppe_lpo",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DateOfPurchase",
                table: "ppe_lpo",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "ppe_lpo",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedOn",
                table: "ppe_lpo",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddPrimaryKey(
                name: "PK_ppe_lpo",
                table: "ppe_lpo",
                column: "LPOId");
        }
    }
}
