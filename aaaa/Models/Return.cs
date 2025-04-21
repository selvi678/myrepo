namespace Project.Models
{
    public class Return
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public string Reason { get; set; }
        public DateTime RequestDate { get; set; }
        public string Status { get; set; }
    }
}
