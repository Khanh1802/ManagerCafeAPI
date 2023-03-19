using ManagerCafe.Contracts.Dtos.ProductDtos;
using ManagerCafe.Contracts.Services;
using ManagerCafe.Share.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ManagerCafeAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }
        [AllowAnonymous]
        [HttpPost("GetAll")]
        public async Task<IActionResult> GetAllAsync([FromBody] FilterProductDto filter)
        {
            try
            {
                var data = await _productService.GetPagedListAsync(filter);
                return Ok(new
                {
                    IsSuccess = true,
                    Data = data
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

        [HttpPost]
        public async Task<IActionResult> AddAsync([FromBody] CreateProductDto product)
        {
            try
            {
                var createProduct = await _productService.AddAsync(product);
                return Ok(new
                {
                    IsSuccess = true,
                    Data = createProduct,
                    Message = "Create success"
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

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(Guid id, [FromBody] UpdateProductDto update)
        {
            try
            {
                var updateProduct = await _productService.UpdateAsync(id, update);

                return Ok(new
                {
                    IsSuccess = true,
                    Data = updateProduct,
                    Message = "Update success"
                });
            }
            catch (NotFoundException ex)
            {
                return StatusCode(StatusCodes.Status404NotFound, new
                {
                    IsSuccess = false,
                    Message = "Error " + ex.Message
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

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            try
            {
                await _productService.DeleteAsync(id);
                return Ok(new
                {
                    IsSuccess = true,
                    Message = "Deleted success"
                });
            }
            catch (NotFoundException ex)
            {
                return StatusCode(StatusCodes.Status404NotFound, new
                {
                    IsSuccess = false,
                    Message = "Error " + ex.Message
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
        [AllowAnonymous]
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            try
            {
                var product = await _productService.GetByIdAsync(id);

                return Ok(new
                {
                    IsSuccess = true,
                    Data = product
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