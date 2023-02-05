using AutoMapper;
using ManagerCafe.Commons;
using ManagerCafe.Contracts.Dtos.InventoryTransactionDtos;
using ManagerCafe.Data.Data;
using ManagerCafe.Data.Enums;
using ManagerCafe.Data.Models;
using ManagerCafe.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ManagerCafe.Contracts.Services
{
    public class InventoryTransactionService : IInventoryTransactionService
    {
        private readonly IInventoryTransactionRepository _inventoryTransactionRepository;
        private readonly IInventoryRepository _inventoryRepository;
        private readonly ManagerCafeDbContext _context;
        private readonly IMapper _mapper;
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

        private async Task<IQueryable<InventoryTransactionDto>> FilterQueryStatisticAbleAsync(FilterInventoryTransactionDto item)
        {
            var query = await _inventoryTransactionRepository.GetQueryableAsync();
            var filter = query
                .Include(x => x.Inventory)
                .ThenInclude(k => k.Product)
                .Include(x => x.Inventory)
                .ThenInclude(k => k.WareHouse)
                .Where(x => !x.Inventory.IsDeleted)
                .GroupBy(x => x.InventoryId).Select(x => new InventoryTransactionDto
                {
                    CreateTime = x.FirstOrDefault().CreateTime,
                    ProductName = x.FirstOrDefault().Inventory.Product.Name,
                    WarehouseName = x.FirstOrDefault().Inventory.WareHouse.Name,
                    Quatity = x.Sum(k => k.Quatity),
                    Type = x.FirstOrDefault().Type,
                    InventoryId = x.FirstOrDefault().InventoryId,
                });
            return filter;
        }

        public async Task<List<InventoryTransactionDto>> GetAllAsync()
        {
            var entites = await _inventoryTransactionRepository.GetAllAsync();
            return _mapper.Map<List<InventoryTransaction>, List<InventoryTransactionDto>>(entites);
        }

        public async Task<CommonPageDto<InventoryTransactionDto>> GetPagedStatisticListAsync(FilterInventoryTransactionDto item, int enums)
        {
            if (Enum.IsDefined(typeof(EnumInventoryTransactionFilter), enums))
            {
                var queryBuilder = await FilterQueryStatisticAbleAsync(item);
                var totalCount = await queryBuilder.CountAsync();

                switch ((EnumInventoryTransactionFilter)enums)
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
                    .Take(item.TakeMaxResultCount);

                return new CommonPageDto<InventoryTransactionDto>(totalCount, item, await queryBuilder.ToListAsync());
            }
            return new CommonPageDto<InventoryTransactionDto>();
        }

        public async Task<List<InventoryTransactionDto>> FilterStatisticFindAsync(FilterInventoryTransactionDto item)
        {
            var query = await _inventoryTransactionRepository.GetQueryableAsync();
            var toDate = item.ToDate.AddDays(1).AddSeconds(-1);
            var filter = query
                .Include(x => x.Inventory)
                .ThenInclude(k => k.WareHouse)
                .Include(x => x.Inventory)
                .ThenInclude(k => k.Product)
                .Where(x => x.Inventory.CreateTime >= item.FromDate && x.Inventory.CreateTime <= toDate
                && x.Inventory.ProductId == item.ProductId && x.Inventory.WareHouseId == item.WarehouseId
                && x.Type == item.Type);
            return _mapper.Map<List<InventoryTransaction>, List<InventoryTransactionDto>>(await filter.ToListAsync());
        }

        public async Task<CommonPageDto<InventoryTransactionDto>> GetPagedHistoryListAsync(FilterInventoryTransactionDto item, int choice)
        {
            if (Enum.IsDefined(typeof(EnumInventoryTransactionFilter), choice))
            {
                var query = await FilterQueryAbleHistoryAsync(item);
                var count = query.CountAsync();
                switch ((EnumInventoryTransactionFilter)choice)
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
                query = query.Skip(item.SkipCount).Take(item.TakeMaxResultCount);
                return new CommonPageDto<InventoryTransactionDto>(await count, item, _mapper.Map<List<InventoryTransaction>, List<InventoryTransactionDto>>(await query.ToListAsync()));
            }
            return new CommonPageDto<InventoryTransactionDto>();
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
            var toDate = item.ToDate.AddDays(1).AddSeconds(-1);
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
