using Microsoft.EntityFrameworkCore.Migrations;

namespace HybridCrypto.Data.Migrations
{
    public partial class Addedkeytouser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Key",
                table: "AspNetUsers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Key",
                table: "AspNetUsers");
        }
    }
}
