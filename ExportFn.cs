using System.Globalization;
using System.Net;
using CsvHelper;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using OrderTracker.Commands;
using OrderTracker.Database.Entities;

namespace OrderTracker.Functions
{
    public class ExportFn
    {
        private readonly ILogger _logger;
        private readonly DatabaseOrderService databaseOrderService;

        public ExportFn(ILoggerFactory loggerFactory, DatabaseOrderService databaseOrderService)
        {
            _logger = loggerFactory.CreateLogger<ExportFn>();
            this.databaseOrderService = databaseOrderService;
        }

        [Function("ExportFn")]
        public HttpResponseData Run([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequestData req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "text/csv; charset=utf-8");

            try 
            {
                IEnumerable<OrderEntity> orders = databaseOrderService.GetOrderEntities();
                
                using (var writer = new StringWriter())
                {
                    var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);

                    csv.WriteField("Id");
                    csv.WriteField("CustomerName");
                    csv.WriteField("Price");
                    csv.WriteField("Date");
                    csv.WriteField("Status");
                    csv.WriteField("ModifiedStatus");


                    foreach (var order in orders)
                    {
                        csv.WriteField(order.Id);
                        csv.WriteField(order.CustomerName);
                        csv.WriteField(order.Price);
                        csv.WriteField(order.Status);
                        csv.WriteField(order.Date);
                        csv.WriteField(order.ModifiedStatus);
                        csv.NextRecord();
                    }

                    response.WriteString(writer.ToString());
                }
            }
            catch(Exception exception)
            {
                _logger.LogError($"Error exporting data to CSV: {exception.Message}");
                response = req.CreateResponse(HttpStatusCode.InternalServerError);
                response.WriteString($"Error: {exception.Message}");
            }
            return response;
        }
    }
}