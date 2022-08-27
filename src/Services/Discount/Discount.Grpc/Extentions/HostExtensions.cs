using Npgsql;

namespace Discount.Grpc.Extentions
{
    public static class HostExtensions
    {
        public static IHost MigrateDatabase<TContext>(this IHost host, int? retry = 0)
        {
            int retryForAvaiability = retry.Value;
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var configuration = services.GetRequiredService<IConfiguration>();
                var logger = services.GetRequiredService<ILogger<TContext>>();

                try
                {
                    logger.LogInformation("Migrating PostgresSqlDatabase Start.");
                    var connection = new NpgsqlConnection(configuration.GetValue<string>("DatabaseSetting:ConnectionString"));
                    connection.Open();
                    using var command = new NpgsqlCommand
                    {
                        Connection = connection
                    };
                    command.CommandText = "Drop Table if Exists Coupon";
                    command.ExecuteNonQuery();
                    command.CommandText = @"CREATE TABLE Coupon(id serial Primary Key,productname varchar(24) NOT NULL,description text ,amount int)";
                    command.ExecuteNonQuery();
                    command.CommandText = "INSERT INTO Coupon(ProductName, Description, Amount) VALUES('IPhone X', 'IPhone Discount', 150);";
                    command.ExecuteNonQuery();
                    command.CommandText = "INSERT INTO Coupon(ProductName, Description, Amount) VALUES('Samsung 10', 'Samsung Discount', 100);";
                    command.ExecuteNonQuery();
                    logger.LogInformation("Migrating PostgresSqlDatabase End.");

                } 
                catch (NpgsqlException ex)
                {
                    logger.LogError(ex, "Exeption in Migration PostgressSql Database");
                    if(retryForAvaiability < 10)
                    {
                        retryForAvaiability ++;
                        Thread.Sleep(2000);
                        MigrateDatabase<TContext>(host, retryForAvaiability);
                    }
                }
            }
            return host;
        }
    }
}
