using System.Data;
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
                Status = orderEntity.Status,
            };
            db.Orders.Add(entity);
            db.SaveChanges();
            orderEntity.Id = entity.Id;
            return orderEntity;
        }

        public void DeleteOrder(int orderId)
        {
            var orderToDelete = db.Orders.Find(orderId);

            if (orderToDelete != null)
            {
                db.Orders.Remove(orderToDelete);
                db.SaveChanges();
            }
        }

        public IEnumerable<OrderEntity> GetOrderEntities()
        {
            var orderList = db.Orders.Select(s => new OrderEntity
            {
                Id = s.Id,
                CustomerName = s.CustomerName,
                Price = s.Price,
                Date = s.Date,
                Status = s.Status,
                ModifiedStatus = s.ModifiedStatus
            });

            return orderList;
        }
        public OrderEntity UpdateOrderStatus(int orderId, string status)
        {
            var orderToUpdate = db.Orders.Find(orderId);
            if (orderToUpdate != null || orderToUpdate.Status != status)
            {
                orderToUpdate.Status = status;
                orderToUpdate.ModifiedStatus = DateTime.Now;
                db.SaveChanges();
            }
            return orderToUpdate;
        }

        public IEnumerable<OrderEntity> FindOrderByStatus(string status)
        {
            return db.Orders.Where(order => order.Status == status).ToList();
        }

    }


}