using ManagerCafe.Contracts.Dtos.OrderDetails;
using ManagerCafe.Data.Enums;
using ManagerCafe.Data.Models;

namespace ManagerCafe.Contracts.Dtos.Orders
{
    public class OrderDto
    {
        public Guid Id { get; set; }
        public decimal TotalBill { get; set; }
        public EnumDelivery Delivery { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime? DeletetionTime { get; set; }
        public DateTime? LastModificationTime { get; set; }
        public bool IsDeleted { get; set; }

        public List<OrderDetailDto> OrderDetails { get; set; }
        public string Phone { get; set; }
        public string CustomerName { get; set; }
    }
}
