using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GameSIteV01.Migrations
{
    public partial class AddTimeInGame4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Achivments",
                table: "Users",
                newName: "TimeInGame");

            migrationBuilder.AddColumn<int>(
                name: "Kills",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Kills",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "TimeInGame",
                table: "Users",
                newName: "Achivments");
        }
    }
}
