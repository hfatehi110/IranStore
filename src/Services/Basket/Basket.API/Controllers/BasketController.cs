 using Basket.API.Entities;
using Basket.API.GrpcServices;
using Basket.API.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Basket.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class BasketController : Controller
    {
        private readonly IBasketRepository _repository;
        private readonly DiscountGrpcSevice _discountGrpcSevice;

        public BasketController(IBasketRepository repository, DiscountGrpcSevice discountGrpcSevice)
        {
            _repository = repository;
            _discountGrpcSevice = discountGrpcSevice;
        }

        [HttpGet("{userName}",Name = "GetBasket")]
        [ProducesResponseType(typeof(ShopingCart),(int)HttpStatusCode.OK)]
        public async Task<ActionResult<ShopingCart>> GetBasket(string userName)
        {
            var basket = await _repository.GetBasket(userName);
            return Ok(basket ?? new ShopingCart(userName));
        }

        [HttpPost]
        [ProducesResponseType(typeof(ShopingCart),(int)HttpStatusCode.OK)]
        public async Task<ActionResult<ShopingCart>> UpdateBasket([FromBody] ShopingCart basketItem)
        {
            //Todo : Communicate With Discount.grpc
            //Todo : Calculate latest price of product into shopping cart
            // consume Discount Grpc
            foreach (var item in basketItem.Items)
            {
                var coupone = await _discountGrpcSevice.GetDiscount(item.ProductName);
                item.Price -= coupone.Amount;
            }

            return Ok(await _repository.UpdateBasket(basketItem));
        }

        [HttpDelete("{userName}",Name ="DeleteBasket")]
        [ProducesResponseType(typeof(void),(int)HttpStatusCode.OK)]
        public async Task<ActionResult<ShopingCart>> DeleteBasket(string userName)
        {
            await _repository.DeleteBasket(userName);
            return Ok();
        }
    }
}
