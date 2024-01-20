using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using OrderTracker.Database;
using Microsoft.EntityFrameworkCore;
using OrderTracker.Commands;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureServices(services => {
        // services.AddDbContext<OrderDb>(options =>
        // {
        //     var connectionString = "";
        //     options.UseSqlServer(connectionString);
        // });
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();
        services.AddSingleton<DatabaseOrderService>();
    })
    .Build();

host.Run();
