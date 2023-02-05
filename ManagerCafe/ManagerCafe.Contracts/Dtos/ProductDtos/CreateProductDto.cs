namespace ManagerCafe.Contracts.Dtos.ProductDtos
{
    public class CreateProductDto
    {
        public string Name { get; set; }
        public decimal PriceBuy { get; set; }
        public decimal PriceSell { get; set; }
    }
}
