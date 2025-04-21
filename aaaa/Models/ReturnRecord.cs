namespace Project.Models
{
    public class ReturnRecord
    {
        public int Id { get; set; }
        public int OrderItemId { get; set; }
        public string Reason { get; set; }
        public DateTime ReturnDate { get; set; }

        public OrderItem OrderItem { get; set; }
        public int Quantity { get; set; }
        public String Status { get; set; } = "pending";
    }
}
