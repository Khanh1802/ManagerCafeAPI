
namespace ManagerCafe.Contracts.Dtos.OrderDetails
{
    public class CreateOrderDetailDto
    {
        public Guid ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal Price { get; set; }
    }
}
