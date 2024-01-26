using System.Data;
using Microsoft.EntityFrameworkCore;
using OrderTracker.Database;
using OrderTracker.Database.Entities;
using CsvHelper;
using System.Globalization;
using ClosedXML.Excel;

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

            return orderList.AsNoTracking().ToList();
        }
        public OrderEntity UpdateOrderStatus(int orderId, OrderStatus status)
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

        public IEnumerable<OrderEntity> FindOrderByStatus(OrderStatus status)
        {
            return db.Orders.Where(order => order.Status == status).ToList();
        }

        public string ExportToCsv()
        {
            var orders = GetOrderEntities();
                
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

                return writer.ToString();
                }
        }

        public byte[] ExportToExcel()
        {
            var orders = GetOrderEntities();
            using(var workbook = new XLWorkbook())
            {
                var worksheet = workbook.AddWorksheet("Orders");

                worksheet.Cell(1,1).Value = "Id";
                worksheet.Cell(1,2).Value = "CustomerName";
                worksheet.Cell(1,3).Value = "Price";
                worksheet.Cell(1,4).Value = "Date";
                worksheet.Cell(1,5).Value = "Status";
                worksheet.Cell(1,6).Value = "ModifiedStatus";

                for(int i = 0;i<orders.Count();i++)
                {
                    var order = orders.ElementAt(i);

                    worksheet.Cell(i+2,1).Value = order.Id;
                    worksheet.Cell(i+2,2).Value = order.CustomerName;
                    worksheet.Cell(i+2,3).Value = order.Price;
                    worksheet.Cell(i+2,4).Value = order.Date.ToString("yyyy-MM-dd HH:mm:ss");
                    worksheet.Cell(i+2,5).Value = order.Status.ToString();
                    worksheet.Cell(i+2,6).Value = order.ModifiedStatus.ToString();
                }

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    workbook.SaveAs(memoryStream);
                    memoryStream.Seek(0,SeekOrigin.Begin);

                    return memoryStream.ToArray();
                }
            }
        }




    }
}