using System.Net;
using ManagerCafe.Contracts.Dtos.ProductDtos;
using ManagerCafe.Contracts.Services;
using ManagerCafe.Share.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ManagerCafeAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private const int _takeCount = 5;
        private const int _orderByPrice = 1;
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync(int indexPage)
        {
            try
            {
                var data = await _productService.GetPagedListAsync(new FilterProductDto()
                {
                    SkipCount = (indexPage - 1) * _takeCount,
                    TakeMaxResultCount = _takeCount,
                    CurrentPage = indexPage
                }, _orderByPrice);
                if(data.Data.Count == 0)
                {
                    return StatusCode(StatusCodes.Status404NotFound, new
                    {
                        IsSuccess = false,
                        Message = "Not found data"
                    });
                }
                return Ok(new
                {
                    IsSuccess = true,
                    Data = data.Data
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
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            try
            {
                var product = await _productService.GetByIdAsync(id);
                if (product == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound, new
                    {
                        IsSusscess = true,
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
        [Authorize]
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
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    IsSuccess = false,
                    Message = "Serve error " + ex.Message
                });
            }
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateAsync(Guid id, [FromBody] UpdateProductDto update)
        {
            try
            {
               var put =  await _productService.UpdateAsync(id, update);

                return Ok(new
                {
                    IsSuccess = true,
                    Data = update,
                    Message = "Update success"
                });
            }
            catch (NotFoundException ex)
            {
                return StatusCode(StatusCodes.Status404NotFound, new
                {
                    IsSuccess = false,
                    Message = "Serve error " + ex.Message
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
        [Authorize]
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
            catch(NotFoundException ex)
            {
                return StatusCode(StatusCodes.Status404NotFound, new
                {
                    IsSuccess = false,
                    Message = "Serve error " + ex.Message
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