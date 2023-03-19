namespace ManagerCafe.Contracts.Dtos.InventoryDtos
{
    public class ProductInventoryDto
    {
        public string WarehouseName { get; set; }
        public Guid WarehouseId { get; set; }
        public int Quantity { get; set; }
    }
}
