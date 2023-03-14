using ManagerCafe.Share.Commons;

namespace ManagerCafe.Contracts.Dtos.ProductDtos
{
    public class FilterProductDto : PaginationDto
    {
        public Guid? Id { get; set; }
        public string? Name { get; set; }
        public decimal? PriceMin { get; set; }
        public decimal? PriceMax { get; set; }
        public int? Choice { get; set; }
    }
}
