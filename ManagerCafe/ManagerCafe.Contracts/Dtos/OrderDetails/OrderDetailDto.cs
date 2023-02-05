
namespace ManagerCafe.Contracts.Dtos.OrderDetails
{
    public class OrderDetailDto
    {
        public Guid ProductId { get; set; }
        public Guid WareHouseId { get; set; }
        public Guid? OrderId { get; set; }
        public string ProductName { get; set; }
        public string WarehouseName { get; set; }
        public int Quaity { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal Price { get; set; }
    }
}
