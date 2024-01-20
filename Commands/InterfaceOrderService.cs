using OrderTracker.Database.Entities;

namespace OrderTracker.Commands
{
    public interface InterfaceOrderService
    {
        IEnumerable<OrderEntity> GetOrderEntities();

        OrderEntity AddOrder(OrderEntity orderEntity);
    }

}