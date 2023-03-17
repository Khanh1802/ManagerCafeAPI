using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ManagerCafe.Data.Migrations
{
    public partial class update_Order_lan3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Order_StripeOrderId",
                table: "Order");

            migrationBuilder.DropIndex(
                name: "IX_Order_Url",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "StripePriceId",
                table: "OrderDetail");

            migrationBuilder.DropColumn(
                name: "StripeProductId",
                table: "OrderDetail");

            migrationBuilder.DropColumn(
                name: "HasPayment",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "StripeOrderId",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "Url",
                table: "Order");

            migrationBuilder.RenameColumn(
                name: "NameUser",
                table: "Order",
                newName: "CustomerName");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CustomerName",
                table: "Order",
                newName: "NameUser");

            migrationBuilder.AddColumn<string>(
                name: "StripePriceId",
                table: "OrderDetail",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StripeProductId",
                table: "OrderDetail",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "HasPayment",
                table: "Order",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "StripeOrderId",
                table: "Order",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Url",
                table: "Order",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Order_StripeOrderId",
                table: "Order",
                column: "StripeOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Order_Url",
                table: "Order",
                column: "Url");
        }
    }
}
