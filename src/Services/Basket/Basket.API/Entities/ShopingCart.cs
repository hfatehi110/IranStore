namespace Basket.API.Entities
{
    public class ShopingCart
    {
        public ShopingCart()
        {

        }
        public ShopingCart(string userName)
        {
            UserName = userName;
        }

        public string UserName { get; set; }
        public List<ShoppingCartItem> Items { get; set; } = new List<ShoppingCartItem>();

        public decimal TotalPrice
        {
            get
            {
                return Items.Sum(x => x.Price);
            }
        }
    }
}
