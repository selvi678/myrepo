using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project.Models
{
    public class ReturnRequest
    {
        public int ReturnRequestId { get; set; }

        [Required]
        public int OrderId { get; set; }

        [ForeignKey("OrderId")]
        public Order Order { get; set; }

        [Required]
        public string Reason { get; set; }

        public DateTime RequestDate { get; set; } = DateTime.Now;

        public string Status { get; set; } = "Pending";

        public List<ReturnItem> OrderItems { get; set; } = new List<ReturnItem>();

        // This is to track which items the user wants to return
        public List<ReturnItem> ItemsToReturn { get; set; } = new List<ReturnItem>();
    }

    public class ReturnItem
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
    }
}
