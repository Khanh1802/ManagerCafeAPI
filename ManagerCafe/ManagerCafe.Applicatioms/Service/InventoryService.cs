using AutoMapper;
using ManagerCafe.Contracts.Dtos.InventoryDtos;
using ManagerCafe.Contracts.Dtos.InventoryTransactionDtos;
using ManagerCafe.Contracts.Dtos.WareHouseDtos;
using ManagerCafe.Contracts.Services;
using ManagerCafe.Data.Data;
using ManagerCafe.Data.Enums;
using ManagerCafe.Data.Models;
using ManagerCafe.Domain.Repositories;
using ManagerCafe.Share.Commons;
using ManagerCafe.Share.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace ManagerCafe.Applications.Service
{
    public class InventoryService : IInventoryService
    {
        private readonly IInventoryRepository _inventoryRepository;
        private readonly IMemoryCache _memoryCache;
        private readonly IMapper _mapper;
        private readonly IInventoryTransactionService _inventoryTransactionService;
        private readonly IDatabase _redis;
        private readonly ManagerCafeDbContext _context;

        public InventoryService(IInventoryRepository inventoryRepository, IMapper mapper, ManagerCafeDbContext context,
            IInventoryTransactionService inventoryTransactionService, IMemoryCache memoryCache, IDatabase redis)
        {
            _inventoryRepository = inventoryRepository;
            _mapper = mapper;
            _context = context;
            _inventoryTransactionService = inventoryTransactionService;
            _memoryCache = memoryCache;
            _redis = redis;
        }

        public async Task<InventoryDto> AddAsync(CreatenInvetoryDto item)
        {
            var entity = new Inventory();
            var filter = await FilterInventoryAsync(new FilterInventoryDto()
            {
                ProductId = item.ProductId,
                WareHouseId = item.WareHouseId,
            });
            if (filter != null)
            {
                if (filter is not null)
                {
                    throw new ConflictException("Inventory have been create");
                }
            }
            else
            {
                var transaction = await _context.Database.BeginTransactionAsync();
                try
                {
                    var createInventory = _mapper.Map<CreatenInvetoryDto, Inventory>(item);
                    entity = await _inventoryRepository.AddAsync(createInventory);

                    var inventoryTransaction = new CreateInventoryTransactionDto()
                    {
                        Quatity = entity.Quatity,
                        InventoryId = entity.Id,
                        Type = EnumInventoryTransation.Import
                    };
                    await _inventoryTransactionService.AddAsync(inventoryTransaction);
                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw ex.GetBaseException();
                }
            }

            return _mapper.Map<Inventory, InventoryDto>(entity);
        }

        public async Task DeleteAsync(Guid key)
        {
            var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var entity = await _inventoryRepository.GetByIdAsync(key);
                if (entity is null)
                {
                    throw new Exception("Not found Inventory to delete");
                }

                await _inventoryRepository.Delete(entity);
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw ex.GetBaseException();
            }
        }

        public async Task<InventoryDto> FilterInventoryAsync(FilterInventoryDto item)
        {
            var filter = await _inventoryRepository.GetQueryableAsync();

            if (item.ProductId != null)
            {
                filter = filter.Where(x => x.ProductId == item.ProductId);
            }

            if (item.WareHouseId != null)
            {
                filter = filter.Where(x => x.WareHouseId == item.WareHouseId);
            }

            var inventory = await filter
                .Include(x => x.WareHouse)
                .Include(x => x.Product)
                .Where(x => !x.Product.IsDeleted && !x.WareHouse.IsDeleted)
                .FirstOrDefaultAsync();
            return _mapper.Map<Inventory, InventoryDto>(inventory);
        }

        public async Task<List<InventoryDto>> GetAllAsync()
        {
            if (_memoryCache.TryGetValue<List<Inventory>>(InventoryCacheKey.InventoryAllKey, out var inventories))
            {
                return _mapper.Map<List<Inventory>, List<InventoryDto>>(inventories);
            }

            var entites = await _inventoryRepository.GetAllAsync();
            _memoryCache.Set<List<Inventory>>(InventoryCacheKey.InventoryAllKey, entites, new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(2)
            });
            return _mapper.Map<List<Inventory>, List<InventoryDto>>(entites);
        }

        public async Task<InventoryDto> GetByIdAsync(Guid key)
        {
            var entity = await _inventoryRepository.GetByIdAsync(key);
            return _mapper.Map<Inventory, InventoryDto>(entity);
        }

        public async Task<CommonPageDto<InventoryDto>> GetPagedListAsync(FilterInventoryDto item)
        {
            if (Enum.IsDefined(typeof(EnumChoiceFilter), item.Choice))
            {
                var query = await FilterQueryAbleAsync(item);
                var count = query.CountAsync();
                if ((item.WareHouseId.HasValue && item.ProductId.HasValue) && !item.Id.HasValue && string.IsNullOrEmpty(item.NameSearch))
                {
                    query = query.Where(x => x.WareHouseId == item.WareHouseId);
                    query = query.Where(x => x.ProductId == item.ProductId);
                }
                if (item.Id.HasValue)
                {
                    query = query.Where(x => x.Id == item.Id);
                }
                if (!string.IsNullOrEmpty(item.NameSearch))
                {
                    query = query.Where(x => EF.Functions.Like(x.Product.Name, $"{item.NameSearch}") ||
                    EF.Functions.Like(x.WareHouse.Name, $"%{item.NameSearch}%"));
                }
                switch ((EnumChoiceFilter)item.Choice)
                {
                    case EnumChoiceFilter.DateAsc:
                        query = query.OrderBy(x => x.CreateTime);
                        break;
                    case EnumChoiceFilter.DateDesc:
                        query = query.OrderByDescending(x => x.CreateTime);
                        break;
                    case EnumChoiceFilter.QuatityAsc:
                        query = query.OrderBy(x => x.Quatity);
                        break;
                    case EnumChoiceFilter.QuatytiDesc:
                        query = query.OrderByDescending(x => x.Quatity);
                        break;
                }

                query = query.Skip(item.SkipCount).Take(item.MaxResultCount);
                return new CommonPageDto<InventoryDto>(await count, item,
                    _mapper.Map<List<Inventory>, List<InventoryDto>>(await query.ToListAsync()));
            }

            return new CommonPageDto<InventoryDto>();
        }

        private async Task<IQueryable<Inventory>> FilterQueryAbleAsync(FilterInventoryDto item)
        {
            var query = await _inventoryRepository.GetQueryableAsync();
            var filter = query
                .Include(x => x.Product)
                .Include(x => x.WareHouse)
                .Where(x => !x.Product.IsDeleted && !x.WareHouse.IsDeleted);
            return filter;
        }

        public async Task<InventoryDto> UpdateAsync(Guid id, UpdateInventoryDto item)
        {
            //1 Khởi tạo Init transaction
            var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var entity = await _inventoryRepository.GetByIdAsync(id);
                if (entity is null)
                {
                    throw new Exception("Not found Inventory to update");
                }
                item.Quatity += entity.Quatity;
                var update = _mapper.Map<UpdateInventoryDto, Inventory>(item, entity);
                await _inventoryRepository.UpdateAsync(update);

                // Khi ko bị lỗi thì save tất cả thay đổi xuống Db
                //1
                var inventoryTransaction = new CreateInventoryTransactionDto()
                {
                    Quatity = item.Quatity,
                    InventoryId = id,
                    Type = EnumInventoryTransation.Import
                };
                await _inventoryTransactionService.AddAsync(inventoryTransaction);
                await transaction.CommitAsync();
                return _mapper.Map<Inventory, InventoryDto>(entity);
            }
            catch (Exception ex)
            {
                //Nếu có lỗi trong lúc down xuống db thì trả lại như cũ
                await transaction.RollbackAsync();
                throw ex.GetBaseException();
            }
        }

        public async Task<List<InventoryDto>> FindByIdProductAndWarehouse(FilterInventoryDto item)
        {
            var filter = await _inventoryRepository.GetQueryableAsync();
            var InventoriesDto = filter.Include(x => x.Product).Where(k => !k.IsDeleted)
                .Include(x => x.WareHouse).Where(x => !x.IsDeleted)
                .Where(x => x.Product.Id == item.ProductId && x.WareHouseId == item.WareHouseId)
                .Select(x => x).ToListAsync();
            return _mapper.Map<List<Inventory>, List<InventoryDto>>(await InventoriesDto);
        }

        public async Task<InventoryOrderDetail> GetInventoryOrderDetail(Guid productId)
        {
            var queryInventory = await _inventoryRepository.GetQueryableAsync();
            var inventoryOrderDetails = queryInventory
                .Include(x => x.Product)
                .Include(x => x.WareHouse)
                .Where(x => x.ProductId == productId
                            && !x.Product.IsDeleted
                            && !x.WareHouse.IsDeleted)
                .GroupBy(x => x.ProductId)
                .Select(x => new InventoryOrderDetail
                {
                    TotalQuatity = x.Sum(x => x.Quatity),
                    Price = x.FirstOrDefault().Product.PriceSell,
                    ProductId = x.FirstOrDefault().ProductId,
                    ProductName = x.FirstOrDefault().Product.Name,
                    WareHouses = _mapper.Map<List<WareHouse>, List<WareHouseDto>>(x.Select(x => x.WareHouse).ToList())
                });

            return await inventoryOrderDetails.FirstOrDefaultAsync();
        }

        /// <summary>
        /// Take total warehouse
        /// </summary>
        /// <param name="productId"></param>
        /// <returns>productInventories</returns>
        public async Task<List<ProductInventoryDto>> GetProductInventoryAsync(Guid productId)
        {
            //Step 1: doc redis
            //Step 2: Co thi tra ve luon
            //Step 3: Ko co thi xuong Db lay
            var cacheKey = new RedisKey($"Inventory:{productId}");

            var getRedis = await _redis.StringGetAsync(cacheKey);
            if (getRedis.HasValue)
            {
                var productInventories = JsonConvert.DeserializeObject<List<ProductInventoryDto>>(getRedis.ToString());
                return productInventories;
            }

            var inventory = await _inventoryRepository.GetQueryableAsync();
            var warehouses = await inventory
                .Include(x => x.WareHouse)
                .Where(x => x.ProductId == productId).ToListAsync();
            var result = _mapper.Map<List<Inventory>, List<ProductInventoryDto>>(warehouses);
            var convertJson = JsonConvert.SerializeObject(result);
            var redisValue = new RedisValue(convertJson);
            await _redis.StringSetAsync(cacheKey, redisValue, TimeSpan.FromHours(2));
            return result;
        }
        /// <summary>
        /// Take total warehouse
        /// </summary>
        /// <param name="productIds"></param>
        /// <returns>Guid and productInventories</returns>
        public async Task<Dictionary<Guid, List<ProductInventoryDto>>> GetProductInventoryAsync(List<Guid> productIds)
        {
            var result = new Dictionary<Guid, List<ProductInventoryDto>>();
            foreach (var productId in productIds)
            {
                var productInventory = await GetProductInventoryAsync(productId);
                result.Add(productId,productInventory);
            }

            return result;
        }
        public Task<List<InventoryDto>> FilterAsync(FilterInventoryDto item)
        {
            throw new NotImplementedException();
        }
    }
}