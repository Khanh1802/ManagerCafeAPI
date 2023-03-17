using ManagerCafe.Contracts.Dtos.OrderDetails;
using ManagerCafe.Data.Enums;

namespace ManagerCafe.Contracts.Dtos.Orders
{
    public class CreateOrderDto
    {
        public string CustomerName { get; set; }
        public string Phone { get; set; }
        public EnumOrderDelivery Delivery { get; set; }
        public decimal TotalBill { get; set; }
        public List<CreateOrderDetailDto> OrderDetails { get; set; }
    }
}
