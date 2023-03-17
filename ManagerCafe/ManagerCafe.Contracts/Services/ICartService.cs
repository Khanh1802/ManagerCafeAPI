using ManagerCafe.Contracts.Dtos.CartDtos;

namespace ManagerCafe.Contracts.Services
{
    public interface ICartService
    {
        Task<CartDto> GetCart(string phone);
        Task<CartDto> CreateCartAsync(CreateCartDto item);
        Task<CartDto> UpdateCartAsync(UpdateCartDto item);
        Task DeleteCartAsync(string phone);
    }
}
