using ManagerCafe.Contracts.Dtos.InventoryDtos;
using ManagerCafe.Contracts.Services;
using ManagerCafe.Share.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ManagerCafeAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class InventoryController : ControllerBase
    {
        private readonly IInventoryService _inventoryService;

        public InventoryController(IInventoryService inventoryService)
        {
            _inventoryService = inventoryService;
        }
        [AllowAnonymous]
        [HttpPost("GetAll")]
        public async Task<IActionResult> GetAllAsync([FromBody] FilterInventoryDto filter)
        {
            try
            {
                var data = await _inventoryService.GetPagedListAsync(filter);
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
                    Message = "Server error " + ex.Message
                });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            try
            {
                var inventory = await _inventoryService.GetByIdAsync(id);
                if (inventory == null)
                {
                    return Ok(new
                    {
                        IsSuccess = false,
                        Message = "Not found id"
                    });
                }
                else
                {
                    return StatusCode(StatusCodes.Status200OK, new
                    {
                        IsSuccess = true,
                        Mesage = "Deleted success"
                    });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    IsSuccess = false,
                    Message = "Server error " + ex.Message
                });
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddAsync(CreatenInvetoryDto create)
        {
            try
            {
                await _inventoryService.AddAsync(create);
                return Ok(new
                {
                    IsSuccess = true,
                    Data = create
                });
            }
            catch (ConflictException ex)
            {
                return StatusCode(StatusCodes.Status409Conflict, new
                {
                    IsSuccess = false,
                    Message = ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    IsSuccess = false,
                    Message = "Server error " + ex.Message
                });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(Guid id, UpdateInventoryDto update)
        {
            try
            {
                await _inventoryService.UpdateAsync(id, update);
                return StatusCode(StatusCodes.Status200OK, new
                {
                    IsSuccess = true,
                    Mesage = "UpdateAsync success"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    IsSuccess = false,
                    Message = "Server error " + ex.Message
                });
            }
        }

        [AllowAnonymous]
        [HttpGet("get-by-product/{id}")]
        public async Task<IActionResult> GetProductInventory(Guid id)
        {
            try
            {
                var get = await _inventoryService.GetProductInventoryAsync(id);
                return StatusCode(StatusCodes.Status200OK, new
                {
                    IsSuccess = true,
                    Data = get
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    IsSuccess = false,
                    Message = "Server error " + ex.Message
                });
            }
        }

        [AllowAnonymous]
        [HttpPost("get-by-listProduct/")]
        public async Task<IActionResult> GetProductInventory([FromBody] List<Guid> id)
        {
            try
            {
                var get = await _inventoryService.GetProductInventoryAsync(id);
                return StatusCode(StatusCodes.Status200OK, new
                {
                    IsSuccess = true,
                    Data = get
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    IsSuccess = false,
                    Message = "Server error " + ex.Message
                });
            }
        }

        [AllowAnonymous]
        [HttpPost("get-product-quantity-inventory")]
        public async Task<IActionResult> GetProductQuantityInventory([FromBody] FilterInventoryDto item)
        {
            try
            {
                var data = await _inventoryService.GetProductAndQuantityInventory(item);
                return StatusCode(StatusCodes.Status200OK, new
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
                    Message = "Server error " + ex.Message
                });
            }
        }
    }
}
