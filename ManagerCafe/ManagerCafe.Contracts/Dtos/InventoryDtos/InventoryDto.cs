using ManagerCafe.Share.Commons;

namespace ManagerCafe.Contracts.Dtos.InventoryDtos
{
    public class InventoryDto : PaginationDto
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public Guid WareHouseId { get; set; }
        public string? ProductName { get; set; }
        public string? WareHouseName { get; set; }
        public int Quatity { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime? DeletetionTime { get; set; }
        public DateTime? LastModificationTime { get; set; }
        public decimal Price { get; set; }
    }
}
