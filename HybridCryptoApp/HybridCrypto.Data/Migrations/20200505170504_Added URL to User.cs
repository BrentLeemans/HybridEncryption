using Microsoft.EntityFrameworkCore.Migrations;

namespace HybridCrypto.Data.Migrations
{
    public partial class AddedURLtoUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "URL",
                table: "AspNetUsers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "URL",
                table: "AspNetUsers");
        }
    }
}
