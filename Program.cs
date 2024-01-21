using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using OrderTracker.Database;
using Microsoft.EntityFrameworkCore;
using OrderTracker.Commands;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureServices(services => {
        services.AddDbContext<OrderDb>(options =>
        {
            var connectionString = "Server=tcp:ordertrackerserver.database.windows.net,1433;Initial Catalog=order_tracker_db;Persist Security Info=False;User ID=adam;Password=iotproject1!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
            options.UseSqlServer(connectionString);
        });
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();
        services.AddSingleton<DatabaseOrderService>();
    })
    .Build();

host.Run();
