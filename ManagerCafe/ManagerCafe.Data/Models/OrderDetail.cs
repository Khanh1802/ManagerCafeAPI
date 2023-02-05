using ManagerCafe.Data.Interface;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ManagerCafe.Data.Models
{
    public class OrderDetail : IHasAuditedEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public Guid WareHouseId { get; set; }
        public Guid OrderId { get; set; }
        public string ProductName { get; set; }
        public string WarehouseName { get; set; }
        public int Quaity { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal Price { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime? DeletetionTime { get; set; }
        public DateTime? LastModificationTime { get; set; }
        public bool IsDeleted { get; set; }
        public Order Order { get; set; }
        public Product Product { get; set; }
        public WareHouse WareHouse { get; set; }
    }
}
