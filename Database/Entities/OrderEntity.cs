using System.Text.Json.Serialization;

namespace OrderTracker.Database.Entities
{
    public enum OrderStatus
    {
        Opłacone,
        Wysłane,
        Dostarczone,
        Odebrane,
    }
    public class OrderEntity
    {   
        public int Id {get;set;}
        public string CustomerName {get;set;}
        public double Price {get;set;}
        public DateTime Date {get;set;} = DateTime.Now;
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public OrderStatus Status {get;set;}
        public DateTime? ModifiedStatus {get;set;}
    }
}