using ManagerCafe.Data.Models;

namespace ManagerCafe.Contracts.Dtos.Orders
{
    public class OrderDto
    {
        public Guid Id { get; set; }
        public Guid StaffId { get; set; }
        public Guid CustomerId { get; set; }
        public decimal TotalBill { get; set; }
        public string Code { get; set; }

        public List<OrderDetail> OrderDetails { get; set; }

        public User Staff { get; set; }
        public User Customer { get; set; }
    }
}
