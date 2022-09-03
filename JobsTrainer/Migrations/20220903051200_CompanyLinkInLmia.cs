using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobsTrainer.Migrations
{
    public partial class CompanyLinkInLmia : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CompanyLink",
                table: "LmiaInfos",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompanyLink",
                table: "LmiaInfos");
        }
    }
}
