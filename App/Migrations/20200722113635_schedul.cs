using Microsoft.EntityFrameworkCore.Migrations;

namespace PPE.Migrations
{
    public partial class schedul : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AdditionFormId",
                table: "ppe_register",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<decimal>(
                name: "CB",
                table: "ppe_dailyschedule",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "AccumulatedDepreciation",
                table: "ppe_dailyschedule",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "DepreciationForThePeriod",
                table: "ppe_dailyschedule",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdditionFormId",
                table: "ppe_register");

            migrationBuilder.DropColumn(
                name: "DepreciationForThePeriod",
                table: "ppe_dailyschedule");

            migrationBuilder.AlterColumn<decimal>(
                name: "CB",
                table: "ppe_dailyschedule",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<decimal>(
                name: "AccumulatedDepreciation",
                table: "ppe_dailyschedule",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(decimal));
        }
    }
}
