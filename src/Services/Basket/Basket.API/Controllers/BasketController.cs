using Basket.API.Entities;
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

        public BasketController(IBasketRepository repository)
        {
            _repository = repository;
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
            return Ok(await _repository.UpdateBasket(basketItem));
        }
        [HttpDelete]
        [ProducesResponseType(typeof(void),(int)HttpStatusCode.OK)]
        public async Task<ActionResult<ShopingCart>> DeleteBasket(string userName)
        {
            await _repository.DeleteBasket(userName);
            return Ok();
        }
    }
}
