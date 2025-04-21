namespace Project.Models
{
    public class Delivery
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public string TrackingNumber { get; set; }
        public string Status { get; set; }
        public DateTime EstimatedDelivery { get; set; }
        public string ShippingAddress { get; set; }
        public Order Order { get; set; }
    }
}
