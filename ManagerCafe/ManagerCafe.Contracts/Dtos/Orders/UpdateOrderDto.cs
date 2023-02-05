using ManagerCafe.Data.Enums;

namespace ManagerCafe.Contracts.Dtos.Orders
{
    public class UpdateOrderDto
    {
        public Guid Id { get; set; }
        public Guid StaffId { get; set; }
        public Guid CustomerId { get; set; }
        public string Code { get; set; }
        public EnumOrderStatus Status { get; set; }
    }
}
