using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobsTrainer.Migrations
{
    public partial class SkillInTrain : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Skills",
                table: "TrainJobs",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Skills",
                table: "TrainJobs");
        }
    }
}
