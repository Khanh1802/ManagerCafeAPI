using AutoMapper;
using ManagerCafe.Contracts.Dtos.WareHouseDtos;
using ManagerCafe.Contracts.Services;
using ManagerCafe.Data.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace ManagerCafeAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WarehouseController : ControllerBase
    {
        private readonly IWareHouseService _warehouseService;

        public WarehouseController(IWareHouseService warehouseService)
        {
            _warehouseService = warehouseService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                return Ok(new
                {
                    IsSuccess = true,
                    Data = await _warehouseService.GetAllAsync()
                });
            }
            catch (Exception ex)
            {
                return StatusCode((int) HttpStatusCode.InternalServerError, new
                {
                    IsSuccess = true,
                    Message = "Server error " + ex.Message
                });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                var warehouse = await _warehouseService.GetByIdAsync(id);
                if (warehouse == null)
                {
                    return Ok(new
                    {
                        IsSuccess = true,
                        Message = "Not found id"
                    });
                }
                else
                {
                    return StatusCode((int) HttpStatusCode.OK, new
                    {
                        IsSusscess = true,
                        Data = warehouse
                    });
                }
            }
            catch (Exception ex)
            {
                return StatusCode((int) HttpStatusCode.InternalServerError, new
                {
                    IsSuccess = false,
                    Message = "Server error " + ex.Message
                });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
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
                    return StatusCode((int) HttpStatusCode.OK, new
                    {
                        IsSuccess = true,
                        Mesage = "Deleted success"
                    });
                }
            }
            catch (Exception ex)
            {
                return StatusCode((int) HttpStatusCode.InternalServerError, new
                {
                    IsSuccess = false,
                    Message = "Server error " + ex.Message
                });
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddWarehouse(CreateWareHouseDto create)
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
                return StatusCode((int) HttpStatusCode.InternalServerError, new
                {
                    IsSuccess = false,
                    Message = "Server error " + ex.Message
                });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(UpdateWareHouseDto update)
        {
            try
            {
                var warehouse = await _warehouseService.GetByIdAsync(update.Id);
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
                    // await _warehouseService.UpdateAsync(update);    
                    return StatusCode((int) HttpStatusCode.OK, new
                    {
                        IsSuccess = true,
                        Mesage = "Update success"
                    });
                }
            }
            catch (Exception ex)
            {
                return StatusCode((int) HttpStatusCode.InternalServerError, new
                {
                    IsSuccess = false,
                    Message = "Server error " + ex.Message
                });
            }
        }
    }
}