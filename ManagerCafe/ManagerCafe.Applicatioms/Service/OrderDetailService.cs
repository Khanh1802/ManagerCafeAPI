using AutoMapper;
using ManagerCafe.CacheItems.OrderDetails;
using ManagerCafe.Contracts.Dtos.OrderDetails;
using ManagerCafe.Contracts.Services;
using ManagerCafe.Data.Data;

namespace ManagerCafe.Applications.Service
{
    public class OrderDetailService : IOrderDetailService
    {
        private readonly ManagerCafeDbContext _context;
        private readonly IInventoryService _inventoryService;
        private readonly IOrderDetailCacheService _orderDetailCacheService;
        private readonly IMapper _mapper;
        public OrderDetailService(ManagerCafeDbContext context, IOrderDetailCacheService orderDetailCacheService, IInventoryService inventoryService, IMapper mapper)
        {
            _context = context;
            _orderDetailCacheService = orderDetailCacheService;
            _inventoryService = inventoryService;
            _mapper = mapper;
        }

        public async Task AddAsync(OrderDetailDto item)
        {
            var inventoryOrderDetail = await _inventoryService.GetInventoryOrderDetail(item.ProductId);
            var orderDetails = _orderDetailCacheService.GetOrderDetails();
            var query = orderDetails
                .Where(x => x.ProductId == item.ProductId)
                .Select(x => x).FirstOrDefault();
            if (query != null)
            {
                if (query.Quaity + item.Quaity > inventoryOrderDetail.TotalQuatity)
                {
                    query.Quaity = inventoryOrderDetail.TotalQuatity;
                }
                else
                {
                    query.Quaity += item.Quaity;
                }
            }
            else
            {
                if (item.Quaity > inventoryOrderDetail.TotalQuatity)
                {
                    item.Quaity = inventoryOrderDetail.TotalQuatity;
                }
                orderDetails.Add(_mapper.Map<OrderDetailDto, OrderDetailCacheItem>(item));
            }
        }

        public void Delete(OrderDetailDto item)
        {
            var orderDetails = _orderDetailCacheService.GetOrderDetails();
            var query = orderDetails.Where(x => x.ProductId == item.ProductId).FirstOrDefault();
            if (query != null)
            {
                orderDetails.Remove(query);
            }
        }
    }
}
