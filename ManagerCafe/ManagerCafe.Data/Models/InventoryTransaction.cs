using ManagerCafe.Data.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ManagerCafe.Data.Models
{
    public class InventoryTransaction
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public Guid InventoryId { get; set; }

        public int Quatity { get; set; }
        public EnumInventoryTransation Type { get; set; }
        public DateTime CreateTime { get; set; }
        public Inventory Inventory { get; set; }
    }
}
