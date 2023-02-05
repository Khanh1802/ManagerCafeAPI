namespace ManagerCafe.Contracts.Dtos.ProductDtos
{
    public class UpdateProductDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal PriceBuy { get; set; }
        public decimal PriceSell { get; set; }
    }
}
