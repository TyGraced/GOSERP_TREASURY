using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PPE.Migrations
{
    public partial class reg : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ppe_reassessment",
                columns: table => new
                {
                    ReassessmentId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AssetNumber = table.Column<string>(maxLength: 50, nullable: true),
                    LpoNumber = table.Column<string>(maxLength: 50, nullable: true),
                    AssetClassificationId = table.Column<int>(nullable: false),
                    Description = table.Column<string>(maxLength: 50, nullable: true),
                    Cost = table.Column<decimal>(nullable: false),
                    DateOfPurchaase = table.Column<DateTime>(nullable: false),
                    Quantity = table.Column<int>(nullable: false),
                    DepreciationStartDate = table.Column<DateTime>(nullable: false),
                    UsefulLife = table.Column<int>(nullable: false),
                    ResidualValue = table.Column<decimal>(nullable: false),
                    Location = table.Column<string>(nullable: true),
                    DepreciationForThePeriod = table.Column<decimal>(maxLength: 500, nullable: false),
                    AccumulatedDepreciation = table.Column<decimal>(nullable: false),
                    NetBookValue = table.Column<decimal>(nullable: false),
                    RemainingUsefulLife = table.Column<int>(nullable: false),
                    ProposedUsefulLife = table.Column<int>(nullable: false),
                    Active = table.Column<bool>(nullable: false),
                    Deleted = table.Column<bool>(nullable: false),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    UpdatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ppe_reassessment", x => x.ReassessmentId);
                });

            migrationBuilder.CreateTable(
                name: "ppe_register",
                columns: table => new
                {
                    RegisterId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AssetNumber = table.Column<string>(maxLength: 50, nullable: true),
                    LpoNumber = table.Column<string>(maxLength: 50, nullable: true),
                    AssetClassificationId = table.Column<int>(nullable: false),
                    Description = table.Column<string>(maxLength: 50, nullable: true),
                    Cost = table.Column<decimal>(nullable: false),
                    DateOfPurchaase = table.Column<DateTime>(nullable: false),
                    Quantity = table.Column<int>(nullable: false),
                    DepreciationStartDate = table.Column<DateTime>(nullable: false),
                    UsefulLife = table.Column<int>(nullable: false),
                    ResidualValue = table.Column<decimal>(nullable: false),
                    Location = table.Column<string>(nullable: true),
                    DepreciationForThePeriod = table.Column<decimal>(maxLength: 500, nullable: false),
                    AccumulatedDepreciation = table.Column<decimal>(nullable: false),
                    NetBookValue = table.Column<decimal>(nullable: false),
                    Active = table.Column<bool>(nullable: false),
                    Deleted = table.Column<bool>(nullable: false),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    UpdatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ppe_register", x => x.RegisterId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ppe_reassessment");

            migrationBuilder.DropTable(
                name: "ppe_register");
        }
    }
}
