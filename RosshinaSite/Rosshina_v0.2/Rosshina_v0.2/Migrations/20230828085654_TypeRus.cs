using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rosshina_v0._2.Migrations
{
    public partial class TypeRus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TypeRus",
                table: "CompaniesModels",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TypeRus",
                table: "CompaniesModels");
        }
    }
}
