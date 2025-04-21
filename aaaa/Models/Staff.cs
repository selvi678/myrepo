using System;
using System.ComponentModel.DataAnnotations;

namespace Project.Models
{
    public class Staff
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Full Name")]
        public string FullName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Contact Number")]
        public string ContactNo { get; set; }

        [Required]
        public string Password { get; set; }

        public string Role { get; set; }

        public string Address { get; set; }

        [Required]
        [Display(Name = "Joining Date")]
        public DateTime JoiningDate { get; set; }

        [Required]
        public bool Status { get; set; }
    }
}
