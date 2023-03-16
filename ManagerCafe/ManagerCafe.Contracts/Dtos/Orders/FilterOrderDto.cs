using ManagerCafe.Data.Enums;

namespace ManagerCafe.Contracts.Dtos.Orders
{
    public class FilterOrderDto
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public EnumOrderDelivery Status { get; set; }
    }
}
