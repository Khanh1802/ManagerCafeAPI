using System.Net;
using ManagerCafe.Contracts.Dtos.ProductDtos;
using ManagerCafe.Contracts.Services;
using Microsoft.AspNetCore.Mvc;

namespace ManagerCafeAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            try
            {
                return Ok(new
                {
                    IsSuccess = true,
                    Data = await _productService.GetAllAsync()
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
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                var product = await _productService.GetByIdAsync(id);
                if (product == null)
                {
                    return StatusCode((int) HttpStatusCode.NotFound, new
                    {
                        IsSusscess = false,
                        Message = "Not found id"
                    });
                }
                else
                {
                    return StatusCode(StatusCodes.Status200OK, new
                    {
                        IsSusscess = true,
                        Data = product
                    });
                }
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
                await _productService.AddAsync(product);
                return Ok(new
                {
                    IsSuccess = true,
                    Data = product,
                    Message = "Create success"
                });
            }
            catch (Exception ex)
            {
                return StatusCode((int) HttpStatusCode.InternalServerError, new
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
                await _productService.UpdateAsync(id, update);
                return Ok(new
                {
                    IsSuccess = true,
                    Data = update,
                    Message = "Update success"
                });
            }
            catch (Exception ex)
            {
                return StatusCode((int) HttpStatusCode.InternalServerError, new
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
            catch (Exception ex)
            {
                return StatusCode((int) HttpStatusCode.InternalServerError, new
                {
                    IsSuccess = false,
                    Message = "Serve error " + ex.Message
                });
            }
        }
    }
}