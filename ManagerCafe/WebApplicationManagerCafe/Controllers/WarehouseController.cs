using ManagerCafe.Contracts.Dtos.WareHouseDtos;
using ManagerCafe.Contracts.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ManagerCafeAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class WarehouseController : ControllerBase
    {
        private readonly IWareHouseService _warehouseService;

        public WarehouseController(IWareHouseService warehouseService)
        {
            _warehouseService = warehouseService;
        }

        [HttpPost("GetAll")]
        public async Task<IActionResult> GetAllAsync([FromBody] FilterWareHouseDto filter)
        {
            try
            {
                var data = await _warehouseService.GetPagedListAsync(filter);
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
                    Message = "Server error " + ex.Message
                });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            try
            {
                var warehouse = await _warehouseService.GetByIdAsync(id);
                if (warehouse == null)
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
        public async Task<IActionResult> AddAsync(CreateWareHouseDto create)
        {
            try
            {
                await _warehouseService.AddAsync(create);
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
        public async Task<IActionResult> UpdateAsync(Guid id, UpdateWareHouseDto update)
        {
            try
            {
                await _warehouseService.UpdateAsync(id, update);
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