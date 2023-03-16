namespace ManagerCafe.Contracts.Dtos.CartDtos
{
    public class CreateCartDto
    {
        public Guid ProductId { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
