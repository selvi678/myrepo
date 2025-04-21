using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace YourNamespace.Models
{
    public class Item
    {
        [Key] // Specifies that ItemId is the primary key
        public int ItemId { get; set; }

        [Required] // Marks the Name as a required field
        [StringLength(100)] // Limits the length of the Name to 100 characters
        public string Name { get; set; }

        [Required] // Marks the Quantity as required
        [Range(0, int.MaxValue)] // Ensures Quantity is a non-negative number
        public int Quantity { get; set; }

        [Required] // Marks the Price as required
        [Column(TypeName = "decimal(18,2)")] // Ensures the Price is stored as decimal with 2 decimal places
        public decimal Price { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now; // Automatically set the creation date
    }
}
