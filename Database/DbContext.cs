using Microsoft.Azure.Functions.Worker.Extensions.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace OrderTracker.Database
{
    public class DatabaseContextFactory : IDesignTimeDbContextFactory<OrderDb>
    {
        public OrderDb CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<OrderDb>();
            optionsBuilder.UseSqlServer("Server=tcp:ordertrackerserver.database.windows.net,1433;Initial Catalog=order_tracker_db;Persist Security Info=False;User ID=adam;Password=iotproject1!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");

            return new OrderDb(optionsBuilder.Options);
        }
    }
}