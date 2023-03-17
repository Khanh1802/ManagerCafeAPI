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
        public decimal TotalBill { get; set; }
        public string Code { get; set; }
        public EnumOrderDelivery Delivery { get; set; }
        public DateTime CreateTime { get; set; }    
        public DateTime? DeletetionTime { get; set; }
        public DateTime? LastModificationTime { get; set; }
        public bool IsDeleted { get; set; }
        public string Phone { get; set; }
        public string CustomerName { get; set; }
        public List<OrderDetail> OrderDetails { get; set; }
    }
}
