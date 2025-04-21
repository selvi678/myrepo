using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project.Models;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Controllers
{
    public class DeliveryController : Controller
    {
        private readonly AppDbContext _context;

        public DeliveryController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Delivery/TrackOrder
        public IActionResult TrackOrder(int orderId)
        {
            var order = _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .FirstOrDefault(o => o.OrderId == orderId);

            if (order == null)
            {
                return NotFound();
            }

            var deliveryStatus = order.Status; // You can map this status to more specific delivery statuses like "Shipped", "Delivered"

            return View(new DeliveryStatusViewModel
            {
                OrderId = order.OrderId,
                DeliveryStatus = deliveryStatus
            });
        }

        // GET: Delivery/UpdateStatus
        public IActionResult UpdateStatus(int orderId)
        {
            var order = _context.Orders
                .FirstOrDefault(o => o.OrderId == orderId);

            if (order == null)
            {
                return NotFound();
            }

            var deliveryStatus = new DeliveryStatusViewModel
            {
                OrderId = order.OrderId,
                DeliveryStatus = order.Status
            };

            return View(deliveryStatus);
        }

        // POST: Delivery/UpdateStatus
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateStatus(DeliveryStatusViewModel model)
        {
            if (ModelState.IsValid)
            {
                var order = await _context.Orders
                    .FirstOrDefaultAsync(o => o.OrderId == model.OrderId);

                if (order == null)
                {
                    return NotFound();
                }

                // Update delivery status
                order.Status = model.DeliveryStatus;
                _context.Update(order);
                await _context.SaveChangesAsync();

                // Optionally, you can add additional logic here, like sending notifications to customers

                return RedirectToAction("TrackOrder", new { orderId = order.OrderId });
            }

            return View(model);
        }

        // GET: Delivery/DeliveryDetails/5
        public async Task<IActionResult> DeliveryDetails(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .FirstOrDefaultAsync(m => m.OrderId == id);

            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // GET: Delivery/ConfirmDelivery
        public async Task<IActionResult> ConfirmDelivery(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .FirstOrDefaultAsync(o => o.OrderId == id);

            if (order == null)
            {
                return NotFound();
            }

            order.Status = "Delivered"; // Mark order as delivered
            _context.Update(order);
            await _context.SaveChangesAsync();

            return RedirectToAction("DeliveryDetails", new { id = order.OrderId });
        }
    }
}
