using Microsoft.EntityFrameworkCore.Migrations;

namespace PPE.Migrations
{
    public partial class niu : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RegisterId",
                table: "ppe_dailyschedule",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RegisterId",
                table: "ppe_dailyschedule");
        }
    }
}
