using System.Net;
using System.Text.Json;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using OrderTracker.Commands;
using OrderTracker.Database.Entities;

namespace OrderTracker.Functions
{
    public class OrderFn
    {
        private readonly ILogger _logger;

        private readonly DatabaseOrderService databaseOrderService;

        public OrderFn(ILoggerFactory loggerFactory, DatabaseOrderService databaseOrderService)
        {
            _logger = loggerFactory.CreateLogger<OrderFn>();
            this.databaseOrderService = databaseOrderService;
        }

        [Function("OrderFn")]
        public HttpResponseData Run([HttpTrigger(AuthorizationLevel.Function, "get", "post", "delete")] HttpRequestData req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            var response = req.CreateResponse(HttpStatusCode.OK);
             switch  (req.Method){
                case "POST":
                    StreamReader reader = new StreamReader(req.Body, System.Text.Encoding.UTF8);
                    var json = reader.ReadToEnd();
                    var order = JsonSerializer.Deserialize<OrderEntity>(json);
                    var res =  databaseOrderService.AddOrder(order);
                    response.WriteAsJsonAsync(res);
                    break;
                case "PUT":
                    // StreamReader putReader = new StreamReader(req.Body, System.Text.Encoding.UTF8);
                    // var putJson = putReader.ReadToEnd();
                    // var updatedPerson = JsonSerializer.Deserialize<Person>(putJson);
                    // var putPerson = peopleService1.Update(updatedPerson.Id, updatedPerson.FirstName, updatedPerson.LastName);
                    // response.WriteAsJsonAsync(putPerson);
                    // break;
                case "GET":
                    var getorder = databaseOrderService.GetOrderEntities();
                    response.WriteAsJsonAsync(getorder);
                    break;
                case "DELETE":
                    StreamReader deleteReader = new StreamReader(req.Body, System.Text.Encoding.UTF8);
                    var deleteJson = deleteReader.ReadToEnd();
                    var orderToDelete = JsonSerializer.Deserialize<OrderEntity>(deleteJson);
                    databaseOrderService.DeleteOrder(orderToDelete.Id);
                    response.WriteString("Order deleted successfully");
                    break;   
            }
            return response;
        }
    }
}
