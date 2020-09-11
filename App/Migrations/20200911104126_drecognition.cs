using Microsoft.EntityFrameworkCore.Migrations;

namespace PPE.Migrations
{
    public partial class drecognition : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DerecognitionId",
                table: "ppe_disposal",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DerecognitionId",
                table: "ppe_disposal");
        }
    }
}
