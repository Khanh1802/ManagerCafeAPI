using ManagerCafe.Share.Commons;

namespace ManagerCafe.Contracts.Dtos.InventoryDtos
{
    public class FilterInventoryDto : PaginationDto
    {
        public Guid? Id { get; set; }
        public Guid? ProductId { get; set; }
        public Guid? WareHouseId { get; set; }
        public int? Quatity { get; set; }
        public string? ProductName { get; set; }
        public string? WareHouseName { get; set; }
        public int? Choice { get; set; }
        public string? NameSearch { get; set; }
    }
}
