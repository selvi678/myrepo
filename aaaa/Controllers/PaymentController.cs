using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project.Models;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Controllers
{
    public class PaymentController : Controller
    {
        private readonly AppDbContext _context;

        public PaymentController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Payment/Index
        public IActionResult Index()
        {
            // Assuming that an order has already been created and stored in session or passed to this view
            var order = GetOrderFromSession();
            if (order == null)
            {
                return RedirectToAction("Index", "Order"); // Redirect to order if no order is available
            }

            return View(order);
        }

        // POST: Payment/Process
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProcessPayment(int orderId, string paymentMethod)
        {
            // Fetch the order by ID
            var order = await _context.Orders
                .FirstOrDefaultAsync(o => o.OrderId == orderId);

            if (order == null)
            {
                return NotFound();
            }

            // Process payment (e.g., integrate with a payment gateway here)
            bool paymentSuccess = ProcessPaymentGateway(paymentMethod);

            if (paymentSuccess)
            {
                // Update the order status to "Paid"
                order.Status = "Paid";
                order.PaymentMethod = paymentMethod;
                await _context.SaveChangesAsync();

                // Optionally, update the stock for the products in the order
                foreach (var orderItem in order.OrderItems)
                {
                    var product = await _context.Products
                        .FirstOrDefaultAsync(p => p.Id == orderItem.ProductId);
                    if (product != null)
                    {
                        product.Stock -= orderItem.Quantity;  // Deduct stock from product
                    }
                }

                await _context.SaveChangesAsync();

                return RedirectToAction("Confirmation", new { orderId = order.OrderId });
            }
            else
            {
                // If payment fails, return to the payment page with an error message
                TempData["ErrorMessage"] = "Payment failed. Please try again.";
                return RedirectToAction("Index");
            }
        }

        // GET: Payment/Confirmation
        public async Task<IActionResult> Confirmation(int orderId)
        {
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .FirstOrDefaultAsync(o => o.OrderId == orderId);

            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // Helper method to get the current order from session (for simplicity)
        private Order GetOrderFromSession()
        {
            // Retrieve order from session or database (depending on your logic)
            var orderId = HttpContext.Session.GetInt32("OrderId");
            return orderId.HasValue ? _context.Orders.FirstOrDefault(o => o.OrderId == orderId.Value) : null;
        }

        // Simulate payment gateway processing
        private bool ProcessPaymentGateway(string paymentMethod)
        {
            // In a real system, you would integrate with a payment gateway like Stripe, PayPal, etc.
            // For now, we're simulating payment processing. Return true for a successful payment.
            return true;
        }
    }
}
