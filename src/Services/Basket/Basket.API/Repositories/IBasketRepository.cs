using Basket.API.Entities;

namespace Basket.API.Repositories
{
    public interface IBasketRepository
    {
        Task<ShopingCart> GetBasket(string userName);
        Task<ShopingCart> UpdateBasket(ShopingCart shopingCart);
        Task DeleteBasket(string userName);
    }
}
