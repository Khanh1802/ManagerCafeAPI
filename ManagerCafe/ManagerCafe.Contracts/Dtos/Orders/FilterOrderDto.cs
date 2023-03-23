using ManagerCafe.Data.Enums;
using ManagerCafe.Share.Commons;

namespace ManagerCafe.Contracts.Dtos.Orders
{
    public class FilterOrderDto : PaginationDto
    {
        public Guid? Id { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
    }
}
