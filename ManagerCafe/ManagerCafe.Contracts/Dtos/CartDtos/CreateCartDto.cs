using ManagerCafe.Data.Enums;

namespace ManagerCafe.Contracts.Dtos.CartDtos
{
    public class CreateCartDto
    {
        public string Phone { get; set; }
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string NameProduct { get; set; }
        public EnumDelivery Delivery { get; set; }
        public string CustomerName { get; set; }
        public string Address { get; set; }
    }
}
