using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project.Models;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Controllers
{
    public class ShippingController : Controller
    {
        private readonly AppDbContext _context;

        public ShippingController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Shipping/SelectShippingMethod/5
        public IActionResult SelectShippingMethod(int orderId)
        {
            var order = _context.Orders
                .FirstOrDefault(o => o.OrderId == orderId);

            if (order == null)
            {
                return NotFound();
            }

            // You can use a shipping provider to get rates, or have fixed shipping methods
            var shippingMethods = new[] { "Standard", "Express", "Next-Day" };
            return View(shippingMethods);
        }

        // POST: Shipping/SetShippingMethod/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SetShippingMethod(int orderId, string shippingMethod)
        {
            var order = await _context.Orders
                .FirstOrDefaultAsync(o => o.OrderId == orderId);

            if (order == null)
            {
                return NotFound();
            }

            order.ShippingMethod = shippingMethod;
            _context.Update(order);
            await _context.SaveChangesAsync();

            return RedirectToAction("TrackOrder", "Delivery", new { orderId = order.OrderId });
        }
    }
}
