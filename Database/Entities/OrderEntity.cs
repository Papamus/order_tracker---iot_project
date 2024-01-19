namespace OrderTracker.Database.Entities
{

    public class OrderEntity
    {
        public int Id {get;set;}
        public string CustomerName {get;set;}
        public double Price {get;set;}
        public DateTime Date {get;set;}
        public string Status {get;set;}
        public DateTime? ModifiedStatus {get;set;}
    }
}