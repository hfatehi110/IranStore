using Dapper;
using Discount.Grpc.Entities;
using Npgsql;

namespace Discount.Grpc.Repositories
{
    public class DiscountRepository : IDiscountRepository
    {
        private readonly IConfiguration _configuration;

        public DiscountRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<bool> CreateDiscount(Coupon coupon)
        {
            using (var connection = new NpgsqlConnection
              (_configuration.GetValue<string>("DatabaseSetting:ConnectionString")))
            {
                var result = await connection.ExecuteAsync
                    ($"INSERT INTO Coupon(ProductName, Description, Amount) VALUES({coupon.ProductName}, {coupon.Description}, {coupon.Amount}); ");
                if (result == 0)
                    return false;
                return true;
            }
        }

        public async Task<bool> DeleteDiscount(string productName)
        {
            using (var connection = new NpgsqlConnection
            (_configuration.GetValue<string>("DatabaseSetting:ConnectionString")))
            {
                var result = await connection.ExecuteAsync
                    ($"delete from  coupon where   ProductName={productName}); ");
                if (result == 0)
                    return false;
                return true;
            }
        }

        public async Task<Coupon> GetDiscount(string productName)
        {
            using (var connection = new NpgsqlConnection
                (_configuration.GetValue<string>("DatabaseSetting:ConnectionString")))
            {
                var coupon = await connection.QueryFirstOrDefaultAsync<Coupon>
                    ($"select * from coupon where productName = @productName", new { productName = productName });
                if (coupon == null)
                    return new Coupon { Description = "No Discount", Id = 0, Amount = 0, ProductName = "NoProduct" };
                return coupon;
            }
        }

        public async Task<bool> UpdateDiscount(Coupon coupon)
        {
            using (var connection = new NpgsqlConnection
             (_configuration.GetValue<string>("DatabaseSetting:ConnectionString")))
            {
                var result = await  connection.ExecuteAsync
                    ($"update coupon set  ProductName={coupon.ProductName}, Description={coupon.Description}, Amount={coupon.Amount}); ");
                if (result == 0)
                    return false;
                return true;
            }
        }
    }
}
