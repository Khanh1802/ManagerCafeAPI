using ManagerCafe.Contracts.Dtos.WareHouseDtos;

namespace ManagerCafe.Contracts.Dtos.InventoryDtos
{
    public class InventoryOrderDetail
    {
        public Guid ProductId { get; set; }
        public string ProductName { get; set; }
        public List<WareHouseDto> WareHouses { get; set; }
        public int TotalQuatity { get; set; }
        public decimal Price { get; set; }
    }
}
