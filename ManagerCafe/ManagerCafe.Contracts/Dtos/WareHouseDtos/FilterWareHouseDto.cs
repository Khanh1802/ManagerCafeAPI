using ManagerCafe.Commons;

namespace ManagerCafe.Contracts.Dtos.WareHouseDtos
{
    public class FilterWareHouseDto : PaginationDto
    {
        public Guid? Id { get; set; }
        public string? Name { get; set; }
    }
}
