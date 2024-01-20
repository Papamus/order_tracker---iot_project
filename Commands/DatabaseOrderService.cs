using OrderTracker.Database;
using OrderTracker.Database.Entities;

namespace OrderTracker.Commands
{
    public class DatabaseOrderService : InterfaceOrderService
    {

        private readonly OrderDb db;
        public DatabaseOrderService(OrderDb db)
        {
            this.db = db;
        }

        public OrderEntity AddOrder(OrderEntity orderEntity)
        {
            var entity = new OrderEntity
            {
                CustomerName = orderEntity.CustomerName,
                Price = orderEntity.Price,
                Date = orderEntity.Date,
                Status = orderEntity.Status,
                ModifiedStatus = orderEntity.ModifiedStatus
            };
            db.Orders.Add(entity);
            db.SaveChanges();
            orderEntity.Id = entity.Id;
            return orderEntity;
        }

        public IEnumerable<OrderEntity> GetOrderEntities()
        {
            var orderList = db.Orders.Select(s => new OrderEntity
            {
                CustomerName = s.CustomerName,
                Price = s.Price,
                Date = s.Date,
                Status = s.Status,
                ModifiedStatus = s.ModifiedStatus
            });

            return orderList;
        }
    }


}