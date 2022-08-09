using Basket.API.Entities;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace Basket.API.Repositories
{
    public class BasketRepository : IBasketRepository
    {
        private readonly IDistributedCache _redisCache;

        public BasketRepository(IDistributedCache redisCache)
        {
            _redisCache = redisCache;
        }

        public async Task DeleteBasket(string userName)
        {
            await _redisCache.RemoveAsync(userName);
        }

        public async Task<ShopingCart> GetBasket(string userName)
        {
            var basket = await _redisCache.GetStringAsync(userName);
            if (string.IsNullOrEmpty(basket)) return null;
            return JsonConvert.DeserializeObject<ShopingCart>(basket);
        }

        public async Task<ShopingCart> UpdateBasket(ShopingCart shopingCart)
        {
            var strItems = JsonConvert.SerializeObject(shopingCart);
           await _redisCache.SetStringAsync(shopingCart.UserName,strItems);
            return await GetBasket(shopingCart.UserName);
        }
    }
}
