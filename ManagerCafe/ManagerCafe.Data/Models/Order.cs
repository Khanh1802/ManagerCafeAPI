using ManagerCafe.Data.Enums;
using ManagerCafe.Data.Interface;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ManagerCafe.Data.Models
{
    public class Order : IHasAuditedEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        //public Guid StaffId { get; set; }
        //public Guid CustomerId { get; set; }
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
        //public User Staff { get; set; }
        //public User Customer { get; set; }
    }
}
