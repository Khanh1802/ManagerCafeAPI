using ManagerCafe.Data.Enums;
using ManagerCafe.Share.Commons;

namespace ManagerCafe.Contracts.Dtos.InventoryTransactionDtos
{
    public class FilterInventoryTransactionDto : PaginationDto
    {
        public Guid? Id { get; set; }
        public EnumInventoryTransationType Type { get; set; }
        public Guid? WarehouseId { get; set; }
        public Guid? ProductId { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public EnumInventoryTransactionFilter Choice { get; set; }
        public string? NameSearch { get; set; }
    }
}
