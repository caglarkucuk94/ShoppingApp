using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShoppingApp.Migrations
{
    public partial class isbought : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsBought",
                table: "UserList");

            migrationBuilder.AddColumn<bool>(
                name: "IsBought",
                table: "ProductUserList",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsBought",
                table: "ProductUserList");

            migrationBuilder.AddColumn<bool>(
                name: "IsBought",
                table: "UserList",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
