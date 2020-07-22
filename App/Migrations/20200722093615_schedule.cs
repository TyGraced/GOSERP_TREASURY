using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PPE.Migrations
{
    public partial class schedule : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ppe_dailyschedule",
                columns: table => new
                {
                    PpeDailyScheduleId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Period = table.Column<int>(nullable: true),
                    OB = table.Column<decimal>(nullable: true),
                    DailyDepreciation = table.Column<decimal>(nullable: true),
                    AccumulatedDepreciation = table.Column<decimal>(nullable: true),
                    CB = table.Column<decimal>(nullable: true),
                    PeriodDate = table.Column<DateTime>(nullable: true),
                    AdditionId = table.Column<int>(nullable: false),
                    PeriodId = table.Column<int>(nullable: true),
                    EndPeriod = table.Column<bool>(nullable: true),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    Updatedby = table.Column<string>(maxLength: 50, nullable: true),
                    Createdby = table.Column<string>(maxLength: 50, nullable: true),
                    Updatedon = table.Column<DateTime>(nullable: true),
                    Createdon = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ppe_dailyschedule", x => x.PpeDailyScheduleId);
                });

            migrationBuilder.CreateTable(
                name: "ppe_periodicschedule",
                columns: table => new
                {
                    PpePeriodicScheduleId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Period = table.Column<int>(nullable: true),
                    OB = table.Column<decimal>(nullable: true),
                    MonthlyDepreciation = table.Column<decimal>(nullable: true),
                    AccumulatedDepreciation = table.Column<decimal>(nullable: true),
                    CB = table.Column<decimal>(nullable: true),
                    PeriodDate = table.Column<DateTime>(nullable: true),
                    AdditionId = table.Column<int>(nullable: false),
                    PeriodId = table.Column<int>(nullable: true),
                    EndPeriod = table.Column<bool>(nullable: true),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    Updatedby = table.Column<string>(maxLength: 50, nullable: true),
                    Createdby = table.Column<string>(maxLength: 50, nullable: true),
                    Updatedon = table.Column<DateTime>(nullable: true),
                    Createdon = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ppe_periodicschedule", x => x.PpePeriodicScheduleId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ppe_dailyschedule");

            migrationBuilder.DropTable(
                name: "ppe_periodicschedule");
        }
    }
}
