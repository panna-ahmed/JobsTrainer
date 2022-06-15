using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobsTrainer.Migrations
{
    public partial class NewColumns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Link",
                table: "TrainJobs");

            migrationBuilder.AddColumn<bool>(
                name: "IsEasy",
                table: "TrainJobs",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "TrainJobs",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsEasy",
                table: "TrainJobs");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "TrainJobs");

            migrationBuilder.AddColumn<string>(
                name: "Link",
                table: "TrainJobs",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
