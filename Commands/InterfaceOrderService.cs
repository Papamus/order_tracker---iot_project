using OrderTracker.Database.Entities;

namespace OrderTracker.Commands
{
    public interface InterfaceOrderService
    {
        IEnumerable<OrderEntity> GetOrderEntities();

        OrderEntity AddOrder(OrderEntity orderEntity);

        void DeleteOrder(int orderId);

        OrderEntity UpdateOrderStatus(int id, string status);

        IEnumerable<OrderEntity> FindOrderByStatus(string status);
    }

}