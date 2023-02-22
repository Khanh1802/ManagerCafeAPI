using ManagerCafe.Applications.Service;
using ManagerCafe.Contracts.Dtos.InventoryDtos;
using ManagerCafe.Contracts.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ManagerCafeAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryController : ControllerBase
    {
        private readonly IInventoryService _inventoryService;

        public InventoryController(IInventoryService inventoryService)
        {
            _inventoryService = inventoryService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            try
            {
                return Ok(new
                {
                    IsSuccess = true,
                    Data = await _inventoryService.GetAllAsync()
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

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            try
            {
                var inventory = await _inventoryService.GetByIdAsync(id);
                if (inventory == null)
                {
                    return Ok(new
                    {
                        IsSuccess = true,
                        Message = "Not found id"
                    });
                }
                else
                {
                    return StatusCode(StatusCodes.Status200OK, new
                    {
                        IsSusscess = true,
                        Data = inventory
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

        [HttpDelete("{id}")]
        [Authorize]
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
        [Authorize]
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
        [Authorize]
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
    }
}
