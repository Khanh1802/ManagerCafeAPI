using AutoMapper;
using ManagerCafe.Contracts.Dtos.Orders;
using ManagerCafe.Contracts.Services;
using ManagerCafe.Data.Models;
using ManagerCafe.Domain.Repositories;

namespace ManagerCafe.Applications.Service
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;

        public OrderService(IOrderRepository orderRepository, IMapper mapper)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
        }

        public async Task<OrderDto> CreateAsync(CreateOrderDto item)
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
            return _mapper.Map<Order, OrderDto>(create);
        }

        public Task DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
