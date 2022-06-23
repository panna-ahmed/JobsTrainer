using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobsTrainer.Migrations
{
    public partial class IndexedJob : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_TrainJobs_Title_CreatedAt",
                table: "TrainJobs",
                columns: new[] { "Title", "CreatedAt" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TrainJobs_Title_CreatedAt",
                table: "TrainJobs");
        }
    }
}
