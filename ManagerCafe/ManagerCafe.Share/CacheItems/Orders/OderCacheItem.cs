using ManagerCafe.Data.Enums;

namespace ManagerCafe.Share.CacheItems.Orders
{
    public class OrderCacheItem
    {
        public Guid StaffId { get; set; }
        public Guid CustomerId { get; set; }
        public decimal TotalBill { get; set; }
        public string Code { get; set; }
        public EnumOrderStatus Status { get; set; }
    }
}
