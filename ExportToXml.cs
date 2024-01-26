using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using OrderTracker.Commands;

namespace OrderTracker.Functions
{
    public class ExportToXml
    {
        private readonly ILogger _logger;

        private readonly DatabaseOrderService databaseOrderService;

        public ExportToXml(ILoggerFactory loggerFactory, DatabaseOrderService databaseOrderService)
        {
            _logger = loggerFactory.CreateLogger<ExportToXml>();
            this.databaseOrderService = databaseOrderService;
        }

        [Function("ExportToXml")]
        public HttpResponseData Run([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequestData req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
            response.Headers.Add("Content-Disposition", "attachment; filename=orders.xlsx");

            try
            {
                response.WriteBytes(databaseOrderService.ExportToExcel());
            }
            catch(Exception exception)
            {
                _logger.LogError($"Error exporting data to Excel: {exception.Message}");
                response = req.CreateResponse(HttpStatusCode.InternalServerError);
                response.WriteString($"Error: {exception.Message}");
            }
            return response;
        }
    }
}
