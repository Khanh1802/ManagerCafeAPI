﻿using AutoMapper;
using ManagerCafe.Contracts.Dtos.OrderDetails;
using ManagerCafe.Contracts.Dtos.Orders;
using ManagerCafe.Contracts.Services;
using ManagerCafe.Data.Data;
using ManagerCafe.Data.Models;
using ManagerCafe.Domain.Repositories;
using ManagerCafe.Share.Commons;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using Order = ManagerCafe.Data.Models.Order;

namespace ManagerCafe.Applications.Service
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ICartService _cartService;
        private readonly IMapper _mapper;
        private readonly IInventoryService _inventoryService;
        private readonly IInventoryTransactionService _inventoryTransactionService;
        private readonly IDatabase _redis;
        private readonly ManagerCafeDbContext _context;

        public OrderService(IOrderRepository orderRepository, IMapper mapper, ManagerCafeDbContext context, ICartService cartService, IInventoryService inventoryService, IInventoryTransactionService inventoryTransactionService, IDatabase redis)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
            _context = context;
            _cartService = cartService;
            _inventoryService = inventoryService;
            _inventoryTransactionService = inventoryTransactionService;
            _redis = redis;
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

                #region MyRegion
                //var dictionary = new Dictionary<Guid, List<ProductInventoryDto>>();
                //for (int i = 0; i < item.OrderDetails.Count; i++)
                //{
                //    var inventoryIds = await _inventoryService.GetProductInventoryAsync(item.OrderDetails[i].ProductId);
                //    dictionary.Add(item.OrderDetails[i].ProductId, inventoryIds);
                //}

                //foreach (var items in dictionary)
                //{
                //    var productId = item.OrderDetails.Where(x => x.ProductId == items.Key).FirstOrDefault();
                //    if (productId != null)
                //    {
                //        var quantity = items.Value.Sum(x => x.Quantity);
                //        if (quantity - productId.Quantity < 0)
                //        {
                //            throw new Exception("");
                //        }
                //    }
                //}
                #endregion

                #region MyRegion
                //var productIds = entity.OrderDetails.ToDictionary(x => x.ProductId, x => x.Quantity);
                //var inventories = await _inventoryService.GetProductInventoryAsync(productIds.Keys.ToList());
                //foreach (var items in inventories)
                //{
                //    var totalInventoryQuantity = items.Value.Sum(x => x.Quantity);

                //    if (totalInventoryQuantity <= 0 || totalInventoryQuantity - productIds[items.Key] < 0)
                //    {
                //        throw new Exception("");
                //    }
                //}
                #endregion

                var dictionary = item.OrderDetails.ToDictionary(x => x.ProductId, x => x.Quantity);
                var productInventoryDictionary = await _inventoryService.GetProductInventoryAsync(dictionary.Keys.ToList());
                foreach (var product in productInventoryDictionary)
                {
                    var totalQuantity = product.Value.Sum(x => x.Quantity);
                    if (totalQuantity == 0 || totalQuantity - dictionary[product.Key] < 0)
                    {
                        var nameProduct = item.OrderDetails.Where(x => x.ProductId == product.Key)
                            .Select(x => x.ProductName).FirstOrDefault();
                        throw new Exception($"{nameProduct} not enought quantity");
                    }
                }
                var create = await _orderRepository.AddAsync(entity);

                await _inventoryTransactionService.UpdateOrderAsync(_mapper.Map<Order, OrderDto>(create));
                await transaction.CommitAsync();
                await _cartService.DeleteCartAsync(item.Phone);
                return _mapper.Map<Order, OrderDto>(create);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception(ex.Message);
            }
        }

        public async Task<CommonPageDto<OrderDto>> GetAsync(FilterOrderDto item)
        {
            var query = await _orderRepository.GetQueryableAsync();
            var filter = query
                .Include(x => x.OrderDetails)
                .Where(x => x.CreateTime >= item.FromDate && x.CreateTime <= item.ToDate);
            var countAsync = await filter.CountAsync();
            filter = filter.Skip(item.SkipCount).Take(item.MaxResultCount);
            return new CommonPageDto<OrderDto>(countAsync, item, _mapper.Map<List<Order>, List<OrderDto>>(await filter.ToListAsync()));
        }

        public async Task<List<OrderDetailDto>> GetById(Guid id)
        {
            var query = _context.OrderDetails.AsQueryable();

            var filter = await query
                .Where(x => x.OrderId == id).ToListAsync();
            return _mapper.Map<List<OrderDetail>, List<OrderDetailDto>>(filter);
        }
    }
}
