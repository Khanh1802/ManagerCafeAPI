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

        public async Task<CartDto> GetCart(string phone)
        {
            if (string.IsNullOrEmpty(phone))
            {
                return new CartDto();
            }
            var cacheItem = new RedisKey($"Cart:{phone}");
            var cart = await _redis.StringGetAsync(cacheItem);
            if (cart.HasValue)
            {
                var shoppingCart = JsonConvert.DeserializeObject<CartDto>(cart.ToString());
                return shoppingCart;
            }
            return new CartDto();
        }

        public async Task<CartDto> CreateCartAsync(CreateCartDto item)
        {
            var notFound = false;
            // Create Key
            var cacheItem = new RedisKey($"Cart:{item.Phone}");
            // Find Key
            var cart = await _redis.StringGetAsync(cacheItem);
            // CreateCartDto
            var cartDetailDto = new CartDetailDto()
            {
                ProductName = item.NameProduct,
                Price = item.Price,
                ProductId = item.ProductId,
                Quantity = item.Quantity
            };
            // Neu Key ko co thì tạo mới Key và Value
            if (!cart.HasValue)
            {
                var cartDto = new CartDto()
                {
                    Phone = item.Phone,
                    Delivery = item.Delivery,
                    Address = item.Address,
                    CustomerName = item.CustomerName,
                };
                cartDto.Carts = new List<CartDetailDto>();
                cartDetailDto.TotalPrice = cartDetailDto.Price * cartDetailDto.Quantity;
                cartDto.Carts.Add(cartDetailDto);
                // totalbill
                cartDto.TotalBill = cartDetailDto.TotalPrice;

                cart = new RedisValue(JsonConvert.SerializeObject(cartDto));
                await _redis.StringSetAsync(cacheItem, cart);
                //Convert lai thanh object
                return JsonConvert.DeserializeObject<CartDto>(cart.ToString());
            }

            else
            {
                var cartDto = JsonConvert.DeserializeObject<CartDto>(cart.ToString());
                cartDto.Phone = item.Phone;
                cartDto.Address = item.Address;
                cartDto.CustomerName = item.CustomerName;
                cartDto.Delivery = item.Delivery;
                cartDto.TotalBill = 0;

                foreach (var cartDetailItem in cartDto.Carts)
                {
                    if (cartDetailItem.ProductId == cartDetailDto.ProductId)
                    {
                        cartDetailItem.Quantity += cartDetailDto.Quantity;
                        cartDetailItem.TotalPrice = cartDetailItem.Price * cartDetailItem.Quantity;
                        notFound = true;
                    }
                    else
                    {
                        cartDetailItem.TotalPrice = cartDetailItem.Price * cartDetailItem.Quantity;
                    }
                    cartDto.TotalBill += cartDetailItem.TotalPrice;
                }

                if (!notFound)
                {
                    cartDetailDto.TotalPrice = item.Price * item.Quantity;
                    cartDto.Carts.Add(cartDetailDto);
                    cartDto.TotalBill += cartDetailDto.TotalPrice;
                }
                cart = new RedisValue(JsonConvert.SerializeObject(cartDto));
                await _redis.StringSetAsync(cacheItem, cart);
                return JsonConvert.DeserializeObject<CartDto>(cart.ToString());
            }
        }

        public async Task<CartDto> UpdateCartAsync(UpdateCartDto item)
        {
            var cacheItem = new RedisKey($"Cart:{item.Phone}");
            var cart = await _redis.StringGetAsync(cacheItem);
            if (cart.HasValue)
            {
                var cartDto = JsonConvert.DeserializeObject<CartDto>(cart.ToString());
                var remove = cartDto.Carts.FirstOrDefault(x => x.ProductId == item.ProductId);
                if (remove != null)
                {
                    cartDto.Carts.Remove(remove);
                    cartDto.TotalBill = 0;
                    foreach (var cartDetailItem in cartDto.Carts)
                    {
                        cartDto.TotalBill += cartDetailItem.TotalPrice;
                    }

                    var result = new RedisValue(JsonConvert.SerializeObject(cartDto));
                    await _redis.StringSetAsync(cacheItem, result);
                }
                return cartDto;
            }
            return new CartDto();
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
