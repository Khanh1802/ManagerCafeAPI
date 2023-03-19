using ManagerCafe.Contracts.Dtos.InventoryTransactionDtos;
using ManagerCafe.Contracts.Services;
using Microsoft.AspNetCore.Mvc;

namespace ManagerCafeAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryTransactionController : ControllerBase
    {
        private readonly IInventoryTransactionService _transactionService;

        public InventoryTransactionController(IInventoryTransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        [HttpPost("GetAllStatistics")]
        public async Task<IActionResult> GetAllStatistics([FromBody] FilterInventoryTransactionDto filter)
        {
            var inventoryStatistics = await _transactionService.GetPagedStatisticListAsync(filter);
            try
            {
                return StatusCode(StatusCodes.Status200OK, new
                {
                    IsSuccess = true,
                    Data = inventoryStatistics.Data,
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    IsSuccess = false,
                    Data = inventoryStatistics,
                    Message = "Serve error " + ex.Message
                });
            }
        }

        [HttpPost("GetAllHistory")]
        public async Task<IActionResult> GetAllHistory([FromBody] FilterInventoryTransactionDto filter)
        {
            var inventoryHistory = await _transactionService.GetPagedHistoryListAsync(filter);
            try
            {
                return StatusCode(StatusCodes.Status200OK, new
                {
                    IsSuccess = true,
                    Data = inventoryHistory
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    IsSuccess = false,
                    Data = inventoryHistory,
                    Message = "Serve error " + ex.Message
                });
            }
        }
    }
}
