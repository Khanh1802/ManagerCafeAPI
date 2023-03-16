using AutoMapper;
using ManagerCafe.Contracts.Dtos.CartDtos;
using ManagerCafe.Contracts.Services;
using Newtonsoft.Json;
using StackExchange.Redis;
namespace ManagerCafe.Applications.Service
{
    public class CartService : ICartService
    {
        private readonly IDatabase _redis;
        private readonly IMapper _mapper;
        public CartService(IDatabase redis, IMapper mapper)
        {
            _redis = redis;
            _mapper = mapper;
        }

        public async Task<ShoppingCartDto> GetCart(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return new ShoppingCartDto();
            }
            var cacheItem = new RedisKey($"Cart:{id}");
            var cart = await _redis.StringGetAsync(cacheItem);
            if (cart.HasValue)
            {
                var shoppingCart = JsonConvert.DeserializeObject<ShoppingCartDto>(cart.ToString());
                return shoppingCart;
            }
            return new ShoppingCartDto();
        }

        public async Task<ShoppingCartDto> CreateCartAsync(CreateShoppingDto item)
        {
            var notFound = false;
            // Create Key
            var cacheItem = new RedisKey($"Cart:{item.Phone}");
            // Find Key
            var cart = await _redis.StringGetAsync(cacheItem);
            // CreateCartDto
            var createCartDto = new CartDto()
            {
                ProductName = item.NameProduct,
                Price = item.Price,
                ProductId = item.ProductId,
                Quantity = item.Quantity,
                TotalPrice = item.TotalPrice,
            };
            // Neu Key ko co thì tạo mới Key và Value
            if (!cart.HasValue)
            {
                var shoppingDto = new ShoppingCartDto()
                {
                    TotalBill = item.TotalBill,
                    Phone = item.Phone,
                    Delivery = item.Delivery,
                    Address = item.Address,
                    NameUser = item.NameUser,
                };
                shoppingDto.Carts = new List<CartDto>();
                shoppingDto.Carts.Add(createCartDto);

                cart = new RedisValue(JsonConvert.SerializeObject(shoppingDto));
                await _redis.StringSetAsync(cacheItem, cart);
                //Convert lai thanh object
                return JsonConvert.DeserializeObject<ShoppingCartDto>(cart.ToString());
            }
            else
            {
                var shoppingDto = JsonConvert.DeserializeObject<ShoppingCartDto>(cart.ToString());
                shoppingDto.Phone = item.Phone;
                shoppingDto.TotalBill = item.TotalBill;
                shoppingDto.Address = item.Address;
                shoppingDto.NameUser = item.NameUser;
                shoppingDto.Delivery = item.Delivery;
                foreach (var cartItem in shoppingDto.Carts)
                {
                    if (cartItem.ProductId == createCartDto.ProductId)
                    {
                        cartItem.Quantity += createCartDto.Quantity;
                        cartItem.TotalPrice += createCartDto.TotalPrice;
                        notFound = true;
                        break;
                    }
                }
                if (!notFound)
                {
                    shoppingDto.Carts.Add(createCartDto);
                }
                cart = new RedisValue(JsonConvert.SerializeObject(shoppingDto));
                await _redis.StringSetAsync(cacheItem, cart);
                return JsonConvert.DeserializeObject<ShoppingCartDto>(cart.ToString());
            }
        }

        public async Task<ShoppingCartDto> UpdateCartAsync(UpdateCartDto item)
        {
            var cacheItem = new RedisKey($"Cart:{item.Phone}");
            var cart = await _redis.StringGetAsync(cacheItem);
            if (cart.HasValue)
            {
                var shoppingCart = JsonConvert.DeserializeObject<ShoppingCartDto>(cart.ToString());
                var remove = shoppingCart.Carts.FirstOrDefault(x => x.ProductId == item.ProductId);
                if (remove != null)
                {
                    shoppingCart.Carts.Remove(remove);
                    var result = new RedisValue(JsonConvert.SerializeObject(shoppingCart));
                    await _redis.StringSetAsync(cacheItem, result);
                }
                return shoppingCart;
            }
            return new ShoppingCartDto();
        }

        public async Task DeleteCartAsync(string phone)
        {
            var cacheItem = new RedisKey($"Cart:{phone}");
            var cart = await _redis.StringGetAsync(cacheItem);
            if (cart.HasValue)
            {
                await _redis.StringGetDeleteAsync(cacheItem);
            }
        }
    }
}
