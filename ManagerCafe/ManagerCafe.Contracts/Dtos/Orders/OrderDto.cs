using ManagerCafe.Data.Enums;
using ManagerCafe.Data.Models;

namespace ManagerCafe.Contracts.Dtos.Orders
{
    public class OrderDto
    {
        public Guid Id { get; set; }
        public decimal TotalBill { get; set; }
        public string Code { get; set; }
        public EnumOrderStatus Status { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime? DeletetionTime { get; set; }
        public DateTime? LastModificationTime { get; set; }
        public bool IsDeleted { get; set; }

        public List<OrderDetail> OrderDetails { get; set; }
        public string Url { get; set; }
        public string StripeOrderId { get; set; }
        public bool HasPayment { get; set; }
    }
}
