namespace ManagerCafe.Contracts.Dtos.WareHouseDtos
{
    public class WareHouseDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime? LastModificationTime { get; set; }
    }
}
