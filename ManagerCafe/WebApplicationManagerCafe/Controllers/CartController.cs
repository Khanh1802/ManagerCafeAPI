using ManagerCafe.Contracts.Dtos.CartDtos;
using ManagerCafe.Contracts.Services;
using Microsoft.AspNetCore.Mvc;

namespace ManagerCafeAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateCart([FromBody] CreateCartDto create)
        {
            try
            {
                var createCart = await _cartService.CreateCartAsync(create);
                return Ok(new
                {
                    IsSuccess = true,
                    Data = createCart
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    IsSuccess = false,
                    Message = "Serve error " + ex.Message
                });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCart(string id)
        {
            var shoppingCart = await _cartService.GetCart(id);
            try
            {
                return Ok(new
                {
                    IsSuccess = true,
                    Data = shoppingCart
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    IsSuccess = false,
                    Message = "Serve error " + ex.Message
                });
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateAsync([FromBody] UpdateCartDto item)
        {
            try
            {
                await _cartService.UpdateCartAsync(item);
                return StatusCode(StatusCodes.Status204NoContent, new
                {
                    IsSuccess = true,
                    Message = "Update success"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    IsSuccess = false,
                    Message = "Serve error " + ex.Message
                });
            }
        }

        [HttpDelete("{phone}")]
        public async Task<IActionResult> DeleteAsync(string phone)
        {
            try
            {
                await _cartService.DeleteCartAsync(phone);
                return StatusCode(StatusCodes.Status200OK, new
                {
                    IsSuccess = true,
                    Message = "Deleted success"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    IsSuccess = false,
                    Message = "Serve error " + ex.Message
                });
            }
        }
    }
}
