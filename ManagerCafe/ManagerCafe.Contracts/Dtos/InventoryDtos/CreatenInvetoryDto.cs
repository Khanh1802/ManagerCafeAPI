namespace ManagerCafe.Contracts.Dtos.InventoryDtos
{
    public class CreatenInvetoryDto
    {
        public Guid? ProductId { get; set; }
        public Guid? WareHouseId { get; set; }
        public int Quatity { get; set; }
    }
}
