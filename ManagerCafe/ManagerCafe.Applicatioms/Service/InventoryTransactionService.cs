using AutoMapper;
using ManagerCafe.Contracts.Dtos.InventoryTransactionDtos;
using ManagerCafe.Contracts.Dtos.Orders;
using ManagerCafe.Contracts.Services;
using ManagerCafe.Data.Data;
using ManagerCafe.Data.Enums;
using ManagerCafe.Data.Models;
using ManagerCafe.Domain.Repositories;
using ManagerCafe.Share.Commons;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;

namespace ManagerCafe.Applications.Service
{
    public class InventoryTransactionService : IInventoryTransactionService
    {
        private readonly IInventoryTransactionRepository _inventoryTransactionRepository;
        private readonly ManagerCafeDbContext _context;
        private readonly IMapper _mapper;
        private readonly IInventoryRepository _inventoryRepository;
        public InventoryTransactionService(IInventoryTransactionRepository inventoryTransactionRepository, ManagerCafeDbContext context, IMapper mapper, IInventoryRepository inventoryRepository)
        {
            _inventoryTransactionRepository = inventoryTransactionRepository;
            _context = context;
            _mapper = mapper;
            _inventoryRepository = inventoryRepository;
        }

        public async Task AddAsync(CreateInventoryTransactionDto item)
        {
            var create = _mapper.Map<CreateInventoryTransactionDto, InventoryTransaction>(item);
            await _inventoryTransactionRepository.AddAsync(create);
        }

        private async Task<IQueryable<InventoryTransaction>> IQueryInventoryTransaction()
        {
            var query = await _inventoryTransactionRepository.GetQueryableAsync();
            query = query.Include(x => x.Inventory)
                    .ThenInclude(x => x.Product)
                    .Include(x => x.Inventory)
                    .ThenInclude(x => x.WareHouse)
                    .Where(x => !x.Inventory.IsDeleted);
            return query;
        }
        private async Task<IQueryable<InventoryTransactionDto>> FilterPageAsync(FilterInventoryTransactionDto item)
        {
            var query = await IQueryInventoryTransaction();
            if (item.Id.HasValue || !string.IsNullOrEmpty(item.NameSearch))
            {
                query = query.Where(x => (x.Inventory.Id == item.Id ||
                EF.Functions.Like(x.Inventory.WareHouse.Name, $"%{item.NameSearch.Trim()}%")
                   || EF.Functions.Like(x.Inventory.Product.Name, $"%{item.NameSearch.Trim()}%")));
            }
            else if (item.ProductId.HasValue && item.WarehouseId.HasValue)
            {
                query = query.Where(x => x.Inventory.ProductId == item.ProductId && x.Inventory.WareHouseId == item.WarehouseId);
            }
            var filter = query.GroupBy(x => x.InventoryId).Select(x => new InventoryTransactionDto
            {
                CreateTime = x.FirstOrDefault().CreateTime,
                ProductName = x.FirstOrDefault().Inventory.Product.Name,
                WarehouseName = x.FirstOrDefault().Inventory.WareHouse.Name,
                Quatity = x.Sum(k => k.Quatity),
                Type = x.FirstOrDefault().Type,
                InventoryId = x.FirstOrDefault().InventoryId,
            });
            if (item.FromDate.HasValue && item.ToDate.HasValue)
            {
                var toDate = item.ToDate?.AddDays(1).AddSeconds(-1);
                filter = filter.Where(x => x.CreateTime >= item.FromDate && x.CreateTime <= item.ToDate);
            }
            return filter;
        }

        public async Task<List<InventoryTransactionDto>> GetAllAsync()
        {
            var entites = await _inventoryTransactionRepository.GetAllAsync();
            return _mapper.Map<List<InventoryTransaction>, List<InventoryTransactionDto>>(entites);
        }

        public async Task<CommonPageDto<InventoryTransactionDto>> GetPagedStatisticListAsync(FilterInventoryTransactionDto item)
        {
            var queryBuilder = await FilterPageAsync(item);
            var totalCount = await queryBuilder.CountAsync();

            switch (item.Choice)
            {
                case EnumInventoryTransactionFilter.DateAsc:
                    {
                        queryBuilder = queryBuilder
                            .OrderBy(x => x.CreateTime);
                        break;
                    }
                case EnumInventoryTransactionFilter.DateDesc:
                    {
                        queryBuilder = queryBuilder
                            .OrderByDescending(x => x.CreateTime);
                        break;
                    }
                case EnumInventoryTransactionFilter.QuatityAsc:
                    {
                        queryBuilder = queryBuilder
                            .OrderBy(x => x.Quatity);
                        break;
                    }
                case EnumInventoryTransactionFilter.QuatytiDesc:
                    {
                        queryBuilder = queryBuilder
                            .OrderByDescending(x => x.Quatity);
                        break;
                    }
            }

            queryBuilder = queryBuilder
                .Skip(item.SkipCount)
                .Take(item.MaxResultCount);

            return new CommonPageDto<InventoryTransactionDto>(totalCount, item, await queryBuilder.ToListAsync());

            return new CommonPageDto<InventoryTransactionDto>();
        }

        public async Task<List<InventoryTransactionDto>> FilterStatisticFindAsync(FilterInventoryTransactionDto item)
        {
            var query = await _inventoryTransactionRepository.GetQueryableAsync();
            var filter = query
                .Include(x => x.Inventory)
                .ThenInclude(k => k.WareHouse)
                .Include(x => x.Inventory)
                .ThenInclude(k => k.Product)
                .Where(x => x.Inventory.ProductId == item.ProductId && x.Inventory.WareHouseId == item.WarehouseId
                && x.Type == item.Type);
            if (item.FromDate.HasValue && item.ToDate.HasValue)
            {
                var toDate = item.ToDate?.AddDays(1).AddSeconds(-1);
                query = query.Where(x => x.Inventory.CreateTime >= item.FromDate && x.Inventory.CreateTime <= item.FromDate);
            }
            return _mapper.Map<List<InventoryTransaction>, List<InventoryTransactionDto>>(await filter.ToListAsync());
        }

        public async Task<CommonPageDto<InventoryTransactionDto>> GetPagedHistoryListAsync(FilterInventoryTransactionDto item)
        {
            if (Enum.IsDefined(typeof(EnumInventoryTransactionFilter), item.Choice))
            {
                var query = await FilterQueryAbleHistoryAsync(item);
                var count = query.CountAsync();
                switch ((EnumInventoryTransactionFilter)item.Choice)
                {
                    case EnumInventoryTransactionFilter.DateAsc:
                        query = query.OrderBy(x => x.CreateTime);
                        break;
                    case EnumInventoryTransactionFilter.DateDesc:
                        query = query.OrderByDescending(x => x.CreateTime);
                        break;
                    case EnumInventoryTransactionFilter.QuatityAsc:
                        query = query.OrderBy(x => x.Quatity);
                        break;
                    case EnumInventoryTransactionFilter.QuatytiDesc:
                        query = query.OrderByDescending(x => x.Quatity);
                        break;
                }
                query = query.Skip(item.SkipCount).Take(item.MaxResultCount);
                return new CommonPageDto<InventoryTransactionDto>(await count, item, _mapper.Map<List<InventoryTransaction>, List<InventoryTransactionDto>>(await query.ToListAsync()));
            }
            return new CommonPageDto<InventoryTransactionDto>();
        }

        public async Task<List<InventoryTransactionDto>> UpdateOrderAsync(OrderDto item)
        {
            try
            {
                var queryInventoryTransaction = await _inventoryTransactionRepository.GetQueryableAsync();

                var createInventoryTransactions = new List<CreateInventoryTransactionDto>();
                foreach (var detailDtoQuantity in item.OrderDetails)
                {
                    var queryInventory = _context.Invetories.AsQueryable();
                    //take inventory
                    var inventories = await queryInventory
                        .OrderByDescending(x => x.Quatity)
                        .Where(x => x.ProductId == detailDtoQuantity.ProductId
                                    && x.Quatity > 0 && !x.IsDeleted).ToListAsync();
                    // take total quantity 
                    var quantityOrder = detailDtoQuantity.Quantity;

                    //var updateInventories = new List<Inventory>();
                    for (int i = 0; i < inventories.Count; i++)
                    {
                        inventories[i].LastModificationTime = DateTime.Now;
                        var inventoryTransaction = new CreateInventoryTransactionDto()
                        {
                            InventoryId = inventories[i].Id,
                            Type = EnumInventoryTransationType.Export,
                            CreateTime = DateTime.Now,
                        };
                        // take inventoryTransaction
                        // 9 > 5
                        if (inventories[i].Quatity >= quantityOrder)
                        {
                            //sub quantity of Inventory
                            inventories[i].Quatity = inventories[i].Quatity - quantityOrder;
                            //add quantity of InventoryTransaction
                            inventoryTransaction.Quatity = quantityOrder;
                            //sub quantity of order
                            quantityOrder = 0;
                        }
                        else
                        {
                            //sub quantity of order
                            quantityOrder = quantityOrder - inventories[i].Quatity;
                            //add quantity of InventoryTransaction
                            inventoryTransaction.Quatity = inventories[i].Quatity;
                            //sub quantity of Inventory
                            inventories[i].Quatity = 0;
                        }
                        //updateInventories.Add(inventory);
                        createInventoryTransactions.Add(inventoryTransaction);
                        if (quantityOrder == 0)
                        {
                            break;
                        }
                    }
                    //Update quantity inventory
                    await _inventoryRepository.UpdateAsync(inventories);
                }
                //Add inventoryTransaction
                if (createInventoryTransactions.Count > 0)
                {
                    await _inventoryTransactionRepository.AddAsync(_mapper.Map<List<CreateInventoryTransactionDto>, List<InventoryTransaction>>(createInventoryTransactions));
                }
            }
            catch (Exception ex)
            {
                throw ex.GetBaseException();
            }

            return new List<InventoryTransactionDto>();

        }

        private async Task<IQueryable<InventoryTransaction>> FilterQueryAbleHistoryAsync(FilterInventoryTransactionDto item)
        {
            var query = await _inventoryTransactionRepository.GetQueryableAsync();
            var filter = query
                .Include(x => x.Inventory)
                .ThenInclude(k => k.Product)
                .Include(x => x.Inventory)
                .ThenInclude(k => k.WareHouse);
            return filter;
        }

        public async Task<List<InventoryTransactionDto>> FilterHistoryFindAsync(FilterInventoryTransactionDto item)
        {
            var toDate = item.ToDate?.AddDays(1).AddSeconds(-1);
            var query = await _inventoryTransactionRepository.GetQueryableAsync();
            var filter = query
                .Include(x => x.Inventory)
                .ThenInclude(k => k.Product)
                .Include(x => x.Inventory)
                .ThenInclude(k => k.WareHouse)
                .Where(x => x.Inventory.ProductId == item.ProductId && x.Inventory.WareHouseId == item.WarehouseId
                && x.Type == item.Type && x.CreateTime >= item.FromDate && x.CreateTime <= toDate);
            return _mapper.Map<List<InventoryTransaction>, List<InventoryTransactionDto>>(await filter.ToListAsync());
        }
    }
}
