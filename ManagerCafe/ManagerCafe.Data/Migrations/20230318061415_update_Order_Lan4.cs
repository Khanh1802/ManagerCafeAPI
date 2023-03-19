using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ManagerCafe.Data.Migrations
{
    public partial class update_Order_Lan4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Inventory_ProductId_WareHouseId_IsDeleted",
                table: "Inventory",
                columns: new[] { "ProductId", "WareHouseId", "IsDeleted" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Inventory_ProductId_WareHouseId_IsDeleted",
                table: "Inventory");
        }
    }
}
