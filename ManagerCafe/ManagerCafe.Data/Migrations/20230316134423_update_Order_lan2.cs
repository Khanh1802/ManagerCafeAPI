using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ManagerCafe.Data.Migrations
{
    public partial class update_Order_lan2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NameUser",
                table: "Order",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "Order",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NameUser",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "Phone",
                table: "Order");
        }
    }
}
