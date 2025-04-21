using Project.Models;

namespace Project.Models
{
    public class CheckoutViewModel
    {
        public List<CartItem> CartItems { get; set; }
        public decimal Total { get; set; }
    }
}
