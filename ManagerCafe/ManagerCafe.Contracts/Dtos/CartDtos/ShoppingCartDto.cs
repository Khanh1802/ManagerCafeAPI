using ManagerCafe.Data.Enums;

namespace ManagerCafe.Contracts.Dtos.CartDtos
{
    public class ShoppingCartDto
    {
        public string NameUser { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public decimal TotalBill { get; set; }
        public EnumDelivery Delivery { get; set; }
        public List<CartDto> Carts { get; set; }
    }
}
