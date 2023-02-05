namespace ManagerCafe.Contracts.Dtos.Orders
{
    public class SearchProductDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal PriceSell { get; set; }
    }
}
