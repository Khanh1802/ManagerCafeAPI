using ManagerCafe.Data.Enums;
using ManagerCafe.Data.Models;

namespace ManagerCafe.Contracts.Dtos.InventoryTransactionDtos
{
    public class InventoryTransactionDto
    {
        public Guid Id { get; set; }
        public string ProductName { get; set; }
        public string WarehouseName { get; set; }
        public Guid InventoryId { get; set; }
        public int Quatity { get; set; }
        public EnumInventoryTransation Type { get; set; }

        public DateTime CreateTime { get; set; }
    }
}
