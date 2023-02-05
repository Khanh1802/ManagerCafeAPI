using ManagerCafe.Commons;

namespace ManagerCafe.Contracts.Dtos.ProductDtos
{
    public class FilterProductDto : PaginationDto
    {
        public string Name { get; set; }
        public decimal PriceBuy { get; set; }
        public decimal PriceSell { get; set; }
    }
}
