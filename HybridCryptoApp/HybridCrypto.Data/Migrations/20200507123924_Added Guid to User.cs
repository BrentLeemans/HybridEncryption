using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HybridCrypto.Data.Migrations
{
    public partial class AddedGuidtoUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "Guid",
                table: "AspNetUsers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Guid",
                table: "AspNetUsers");
        }
    }
}
