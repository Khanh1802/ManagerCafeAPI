using ManagerCafe.Contracts.Dtos.Orders;
using ManagerCafe.Contracts.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ManagerCafeAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly ILogger<OrderController> _logger;

        public OrderController(IOrderService orderService, ILogger<OrderController> logger)
        {
            _orderService = orderService;
            _logger = logger;
        }

        [HttpPost()]
        public async Task<IActionResult> CreateAsync([FromBody] CreateOrderDto item)
        {
            try
            {
                var createAsync = await _orderService.CreateAsync(item);
                return StatusCode(StatusCodes.Status200OK, new
                {
                    IsSuccess = true,
                    Data = createAsync
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error");
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    IsSuccess = false,
                    Message = $"{ex.Message}"
                });
            }
        }

        [HttpPost("getall")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllAsync([FromBody] FilterOrderDto item)
        {
            try
            {
                var getAll = await _orderService.GetAsync(item);
                return StatusCode(StatusCodes.Status200OK, new
                {
                    IsSuccess = true,
                    Data = getAll
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error");
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    IsSuccess = false,
                    Message = $"{ex.Message}"
                });
            }
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllAsync(Guid id)
        {
            try
            {
                var orderDetailDtos = await _orderService.GetById(id);
                return StatusCode(StatusCodes.Status200OK, new
                {
                    IsSuccess = true,
                    Data = orderDetailDtos
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error");
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    IsSuccess = false,
                    Message = $"{ex.Message}"
                });
            }
        }
    }
}
