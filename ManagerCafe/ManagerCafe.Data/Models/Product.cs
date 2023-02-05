using ManagerCafe.Data.Interface;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ManagerCafe.Data.Models
{
    public class Product : IHasAuditedEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string Name { get; set; }
        [NotMapped] // It mean above data not show 
        public string Code { get; set; }
        public decimal PriceBuy { get; set; }
        public decimal PriceSell { get; set; }
        public DateTime CreateTime { get; set; } 
        public DateTime? DeletetionTime { get; set; }
        public DateTime? LastModificationTime { get; set; }
        public bool IsDeleted { get; set; }
        public List<Inventory> Invetories { get; set; }
    }
}
