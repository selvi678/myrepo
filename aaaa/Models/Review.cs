namespace Project.Models
{
    public class Review
    {
        public int rId { get; set; }
        public int ProductId { get; set; }
        public int CustomerId { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
        public DateTime ReviewDate { get; set; }

        // Navigation properties
        public Product Product { get; set; }
        public User Customer { get; set; }
    }

}
