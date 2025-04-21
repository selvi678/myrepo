namespace Project.Models
{
    public class OrderItem
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Price { get; set; }
        public Product Product { get; set; }
        public ICollection<ReturnRecord> ReturnRecord { get; set; }

    }
}
