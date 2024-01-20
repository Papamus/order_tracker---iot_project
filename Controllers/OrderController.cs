using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OrderTracker.Commands;
using OrderTracker.Database.Entities;

namespace OrderTracker.Controller
{
    [ApiController]
    [Route("orders")]

    public class OrderController : ControllerBase
    {
        private readonly ILogger<OrderController> logger;
        private readonly InterfaceOrderService orderService;


        public OrderController(ILogger<OrderController> logger, InterfaceOrderService orderService)
        {
            this.logger = logger;
            this.orderService = orderService;
        }

        [HttpGet]
        public IEnumerable<OrderEntity> GetOrderEntities()
        {
            return orderService.GetOrderEntities();
        }

        [HttpPost]
        public OrderEntity AddOrder([FromBody] OrderEntity orderEntity)
        {
            return orderService.AddOrder(orderEntity);
        }
    }
   
}