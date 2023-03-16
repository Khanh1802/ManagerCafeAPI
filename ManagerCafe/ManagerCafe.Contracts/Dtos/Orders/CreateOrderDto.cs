using ManagerCafe.Data.Enums;

namespace ManagerCafe.Contracts.Dtos.Orders
{
    public class CreateOrderDto
    {
        public Guid StaffId { get; set; }
        public Guid CustomerId { get; set; }
        public string Code { get; set; }
        public EnumOrderDelivery Status { get; set; }
    }
}
