using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobsTrainer.Migrations
{
    public partial class ExportFlag : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Exported",
                table: "TrainJobs",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Exported",
                table: "TrainJobs");
        }
    }
}
