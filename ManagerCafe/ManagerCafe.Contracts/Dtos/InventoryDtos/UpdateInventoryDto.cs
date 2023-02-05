namespace ManagerCafe.Contracts.Dtos.InventoryDtos
{
    public class UpdateInventoryDto
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public Guid WareHouseId { get; set; }
        public int Quatity { get; set; }
    }
}
