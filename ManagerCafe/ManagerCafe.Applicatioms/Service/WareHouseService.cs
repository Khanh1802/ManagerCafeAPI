using AutoMapper;
using ManagerCafe.Contracts.Dtos.WareHouseDtos;
using ManagerCafe.Contracts.Services;
using ManagerCafe.Data.Data;
using ManagerCafe.Data.Enums;
using ManagerCafe.Data.Models;
using ManagerCafe.Domain.Repositories;
using ManagerCafe.Share.Commons;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System.Data;

namespace ManagerCafe.Applications.Service
{
    public class WareHouseService : IWareHouseService
    {
        private readonly IWareHouseRepository _wareHouseRepository;
        private readonly ManagerCafeDbContext _context;
        private readonly IMemoryCache _memoryCache;
        private readonly IMapper _mapper;

        public WareHouseService(IWareHouseRepository wareHouseRepository, IMapper mapper, IMemoryCache memoryCache,
            ManagerCafeDbContext context)
        {
            _wareHouseRepository = wareHouseRepository;
            _mapper = mapper;
            _memoryCache = memoryCache;
            _context = context;
        }

        public async Task<WareHouseDto> AddAsync(CreateWareHouseDto item)
        {
            var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var entity = _mapper.Map<CreateWareHouseDto, WareHouse>(item);
                await _wareHouseRepository.AddAsync(entity);
                await transaction.CommitAsync();
                return _mapper.Map<WareHouse, WareHouseDto>(entity);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw ex.GetBaseException();
            }
        }

        public async Task DeleteAsync(Guid key)
        {
            var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var entity = await _wareHouseRepository.GetByIdAsync(key);
                if (entity == null)
                {
                    throw new Exception("Not found WareHouse to detele");
                }

                await _wareHouseRepository.Delete(entity);
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw ex.GetBaseException();
            }
        }

        private async Task<IQueryable<WareHouse>> FilterQueryAbleAsync()
        {
            var query = await _wareHouseRepository.GetQueryableAsync();
            var filter = query.Where(x => !x.IsDeleted);
            return filter;
        }

        public async Task<List<WareHouseDto>> FilterAsync(FilterWareHouseDto item)
        {
            var filters = await _wareHouseRepository.GetQueryableAsync();
            if (!string.IsNullOrEmpty(item.Name))
            {
                filters = filters.Where(x => EF.Functions.Like(x.Name, $"%{item.Name}%"));
            }

            return _mapper.Map<List<WareHouse>, List<WareHouseDto>>(await filters.ToListAsync());
        }

        public async Task<List<WareHouseDto>> FilterChoice(int choice)
        {
            if (Enum.IsDefined(typeof(EnumWareHouseFilter), choice))
            {
                var query = await FilterQueryAbleAsync();
                switch ((EnumWareHouseFilter)choice)
                {
                    case EnumWareHouseFilter.DateAsc:
                        query = query.OrderBy(x => x.CreateTime);
                        break;
                    case EnumWareHouseFilter.DateDesc:
                        query = query.OrderByDescending(x => x.CreateTime);
                        break;
                }

                return new List<WareHouseDto>(
                    _mapper.Map<List<WareHouse>, List<WareHouseDto>>(await query.ToListAsync()));
            }

            throw new Exception("Not found filter Product");
        }

        public async Task<List<WareHouseDto>> GetAllAsync()
        {
            if (_memoryCache.TryGetValue<List<WareHouse>>(WarehouseCacheKey.WarehouseAllKey, out var inventories))
            {
                return _mapper.Map<List<WareHouse>, List<WareHouseDto>>(inventories);
            }

            var entites = await _wareHouseRepository.GetAllAsync();
            _memoryCache.Set(WarehouseCacheKey.WarehouseAllKey, entites, new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(2)
            });
            return _mapper.Map<List<WareHouse>, List<WareHouseDto>>(entites);
        }

        public async Task<WareHouseDto> GetByIdAsync(Guid key)
        {
            var entity = await _wareHouseRepository.GetByIdAsync(key);
            return _mapper.Map<WareHouse, WareHouseDto>(entity);
        }

        public async Task<WareHouseDto> UpdateAsync(Guid id, UpdateWareHouseDto item)
        {
            var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var entity = await _wareHouseRepository.GetByIdAsync(id);
                if (entity == null)
                {
                    throw new Exception("Not found WareHouse to update");
                }

                var update = _mapper.Map<UpdateWareHouseDto, WareHouse>(item, entity);
                await _wareHouseRepository.UpdateAsync(update);
                await transaction.CommitAsync();
                return _mapper.Map<WareHouse, WareHouseDto>(update);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw ex.GetBaseException();
            }
        }

        public async Task<CommonPageDto<WareHouseDto>> GetPagedListAsync(FilterWareHouseDto item)
        {
            var query = await FilterQueryAbleAsync(item);
            var count = query.CountAsync(); query = query.Skip(item.SkipCount).Take(item.MaxResultCount);
            return new CommonPageDto<WareHouseDto>(await count, item,
                _mapper.Map<List<WareHouse>, List<WareHouseDto>>(await query.ToListAsync()));
        }
        private async Task<IQueryable<WareHouse>> FilterQueryAbleAsync(FilterWareHouseDto item)
        {
            var query = await _wareHouseRepository.GetQueryableAsync();
            var filter = query.Where(x => !x.IsDeleted);
            if (item.Id.HasValue && string.IsNullOrEmpty(item.Name))
            {
                filter = filter.Where(x => x.Id == item.Id);
            }
            if (!string.IsNullOrEmpty(item.Name) && !item.Id.HasValue)
            {
                filter = filter.Where(x => EF.Functions.Like(x.Name, $"%{item.Name.Trim()}%"));
            }
            return filter;
        }
    }
}