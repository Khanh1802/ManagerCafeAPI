using AutoMapper;
using ManagerCafe.Contracts.Dtos.Orders;
using ManagerCafe.Contracts.Services;
using ManagerCafe.Data.Data;
using ManagerCafe.Data.Models;
using ManagerCafe.Domain.Repositories;

namespace ManagerCafe.Applications.Service
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ICartService _cartService;
        private readonly IMapper _mapper;
        private readonly ManagerCafeDbContext _context;

        public OrderService(IOrderRepository orderRepository, IMapper mapper, ManagerCafeDbContext context, ICartService cartService)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
            _context = context;
            _cartService = cartService;
        }

        public async Task<OrderDto> CreateAsync(CreateOrderDto item)
        {
            var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var entity = _mapper.Map<CreateOrderDto, Order>(item);
                var rn = new Random();
                var prefix = "";
                for (int i = 0; i < 5; i++)
                {
                    prefix += rn.Next(0, 10);
                }
                entity.Code = $"CAFE_{DateTime.Now:yyyyMMddHHmmss}_{prefix}";
                var create = await _orderRepository.AddAsync(entity);
                await _cartService.DeleteCartAsync(item.Phone);
                await transaction.CommitAsync();
                return _mapper.Map<Order, OrderDto>(create);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw ex.GetBaseException();
            }
        }

        public async Task DeleteAsync(Guid id)
        {
        }
    }
}
