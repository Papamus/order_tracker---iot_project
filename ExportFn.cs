using System.Globalization;
using System.Net;
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

            // var reader = new StreamReader(req.Body, System.Text.Encoding.UTF8);
            // switch (reader.ToString()){
            //     case "xls":
            //         try
            //         {
            //             response.Headers.Add("Content-Type", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
            //             response.Headers.Add("Content-Disposition", "attachment; filename=orders.xlsx");
            //             response.WriteString(databaseOrderService.ExportToExcel());
            //         }
            //         catch(Exception exception)
            //         {
            //             _logger.LogError($"Error exporting data to Excel: {exception.Message}");
            //             response = req.CreateResponse(HttpStatusCode.InternalServerError);
            //             response.WriteString($"Error: {exception.Message}");
            //         }
            //         break;

            //     default:
            //         try 
            //         {
            //             response.WriteString(databaseOrderService.ExportToCsv());
            //         }
            //         catch(Exception exception)
            //         {
            //             _logger.LogError($"Error exporting data to CSV: {exception.Message}");
            //             response = req.CreateResponse(HttpStatusCode.InternalServerError);
            //             response.WriteString($"Error: {exception.Message}");
            //         }
                    
            //         break;
            // }
            try 
            {
                response.WriteString(databaseOrderService.ExportToCsv());
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