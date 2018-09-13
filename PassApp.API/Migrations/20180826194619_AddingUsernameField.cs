using Microsoft.EntityFrameworkCore.Migrations;

namespace PassApp.API.Migrations
{
    public partial class AddingUsernameField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Username",
                table: "Passwords",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Username",
                table: "Passwords");
        }
    }
}
