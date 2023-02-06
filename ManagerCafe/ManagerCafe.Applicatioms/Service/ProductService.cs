using AutoMapper;
using AutoMapper.QueryableExtensions;
using ManagerCafe.Commons;
using ManagerCafe.Contracts.Dtos.Orders;
using ManagerCafe.Contracts.Dtos.ProductDtos;
using ManagerCafe.Contracts.Services;
using ManagerCafe.Data.Data;
using ManagerCafe.Data.Enums;
using ManagerCafe.Data.Models;
using ManagerCafe.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace ManagerCafe.Applications.Service
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _memoryCache;
        private readonly ManagerCafeDbContext _context;

        public ProductService(IProductRepository productRepository, IMapper mapper, IMemoryCache memoryCache,
            ManagerCafeDbContext context)
        {
            _productRepository = productRepository;
            _mapper = mapper;
            _memoryCache = memoryCache;
            _context = context;
        }

        public async Task<ProductDto> AddAsync(CreateProductDto item)
        {
            var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var entity = _mapper.Map<CreateProductDto, Product>(item);
                await _productRepository.AddAsync(entity);
                await transaction.CommitAsync();
                return _mapper.Map<Product, ProductDto>(entity);
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
                var entity = await _productRepository.GetByIdAsync(key);
                if (entity is null)
                {
                    throw new Exception("Not found Product to delete");
                }

                await _productRepository.Delete(entity);
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw ex.GetBaseException();
            }
        }

        public async Task<List<ProductDto>> FilterAsync(FilterProductDto item)
        {
            //var filters = await (await _productRepository.GetQueryableAsync())
            // .Where(x => EF.Functions.Like(x.Name, $"%{item.Name}%"))
            // .ToListAsync();

            var filters = await _productRepository.GetQueryableAsync();

            if (!string.IsNullOrEmpty(item.Name))
            {
                filters = filters.Where(x =>
                    EF.Functions.Match(x.Name, $"*{item.Name}*", MySqlMatchSearchMode.Boolean));
            }

            if (item.PriceBuy > 0)
            {
                filters = filters.Where(x => x.PriceBuy == item.PriceBuy);
            }

            if (item.PriceSell > 0)
            {
                filters = filters.Where(x => x.PriceSell == item.PriceSell);
            }

            // Hướng vẫn cách xem query
            //var query = filters.ToQueryString();
            return _mapper.Map<List<Product>, List<ProductDto>>(await filters.ToListAsync());
        }

        private async Task<IQueryable<Product>> FilterQueryAbleAsync(FilterProductDto item)
        {
            var query = await _productRepository.GetQueryableAsync();
            var filter = query.Where(x => !x.IsDeleted);
            return filter;
        }

        public async Task<List<ProductDto>> GetAllAsync()
        {
            // Get dữ liệu sau khi Set hay còn gọi là cache
            //Đọc dữ liệu trong cache ra
            if (_memoryCache.TryGetValue<List<Product>>(ProductCacheKey.ProductAllKey, out var products))
            {
                return _mapper.Map<List<Product>, List<ProductDto>>(products);
            }

            var entites = await _productRepository.GetAllAsync();

            //Set lại cache với thời gian hết hạn bắt đầu từ bây giờ 2m
            // NOTE: Khi add và update nên dùng remove theo Key để build lại cache 
            _memoryCache.Set<List<Product>>(ProductCacheKey.ProductAllKey, entites, new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(2)
            });
            return _mapper.Map<List<Product>, List<ProductDto>>(entites);
        }

        public async Task<ProductDto> GetByIdAsync(Guid key)
        {
            if (_memoryCache.TryGetValue<Product>(key, out var product))
            {
                return _mapper.Map<Product, ProductDto>(product);
            }

            var entity = await _productRepository.GetByIdAsync(key);
            if (!entity.IsDeleted)
            {
                _memoryCache.Set<Product>(key, entity, new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(2)
                });
                return _mapper.Map<Product, ProductDto>(entity);
            }

            return null;
        }

        public async Task<ProductDto> UpdateAsync(Guid id, UpdateProductDto item)
        {
            var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var entity = await _productRepository.GetByIdAsync(id);
                if (entity is null)
                {
                    throw new Exception("Not found Product to update");
                }

                var update = _mapper.Map<UpdateProductDto, Product>(item, entity);
                await _productRepository.UpdateAsync(update);
                await transaction.CommitAsync();
                return _mapper.Map<Product, ProductDto>(update);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw ex.GetBaseException();
            }
        }

        public async Task<CommonPageDto<ProductDto>> GetPagedListAsync(FilterProductDto item, int choice)
        {
            if (Enum.IsDefined(typeof(EnumProductFilter), choice))
            {
                var query = await FilterQueryAbleAsync(item);
                var count = query.CountAsync();
                switch ((EnumProductFilter) choice)
                {
                    case EnumProductFilter.PriceAsc:
                        query = query.OrderBy(x => x.PriceSell);
                        break;
                    case EnumProductFilter.PriceDesc:
                        query = query.OrderByDescending(x => x.PriceSell);
                        break;
                    case EnumProductFilter.DateAsc:
                        query = query.OrderBy(x => x.CreateTime);
                        break;
                    case EnumProductFilter.DateDesc:
                        query = query.OrderByDescending(x => x.CreateTime);
                        break;
                }

                query = query.Skip(item.SkipCount).Take(item.TakeMaxResultCount);
                return new CommonPageDto<ProductDto>(await count, item,
                    _mapper.Map<List<Product>, List<ProductDto>>(await query.ToListAsync()));
            }

            return new CommonPageDto<ProductDto>();
        }

        //public async Task<List<T>> SearchProductAsync<T>(FilterProductDto filter) where T : class
        //{
        //    var query = await _productRepository.GetQueryableAsync();
        //    return await query.ProjectTo<T>(_mapper.ConfigurationProvider).ToListAsync();
        //}

        public async Task<CommonPageDto<SearchProductDto>> SearchProductAsync(FilterProductDto filter)
        {
            var query = await _productRepository.GetQueryableAsync();
            var products = query.ProjectTo<SearchProductDto>(_mapper.ConfigurationProvider);
            if (!string.IsNullOrEmpty(filter.Name))
            {
                products = products.Where(x => EF.Functions.Like(x.Name, $"%{filter.Name}%"));
                return new CommonPageDto<SearchProductDto>
                (await products.CountAsync(), filter,
                    await products.Skip(filter.SkipCount).Take(filter.TakeMaxResultCount).ToListAsync());
            }

            return new CommonPageDto<SearchProductDto>
            (await products.CountAsync(), filter,
                await products.Skip(filter.SkipCount).Take(filter.TakeMaxResultCount).ToListAsync());
        }
    }
}