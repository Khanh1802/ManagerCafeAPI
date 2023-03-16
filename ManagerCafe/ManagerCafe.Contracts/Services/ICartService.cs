using ManagerCafe.Contracts.Dtos.CartDtos;

namespace ManagerCafe.Contracts.Services
{
    public interface ICartService
    {
        Task<ShoppingCartDto> GetCart(string id);
        Task<ShoppingCartDto> CreateCartAsync(CreateShoppingDto item);
        Task<ShoppingCartDto> UpdateCartAsync(UpdateCartDto item);
        Task DeleteCartAsync(string phone);
    }
}
