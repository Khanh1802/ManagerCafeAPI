using ManagerCafe.Contracts.Dtos.CartDtos;
using ManagerCafe.Data.Enums;

namespace ManagerCafe.Contracts.Dtos.Orders
{
    public class CreateOrderDto
    {
        public string CustomerName { get; set; }
        public string Phone { get; set; }
        public EnumDelivery Delivery { get; set; }
        public decimal TotalBill { get; set; }
        public List<CartDetailDto> OrderDetails { get; set; }
    }
}
