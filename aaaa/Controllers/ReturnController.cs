using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Project.Helpers; // Replace 'Project' with your actual project name
using Project.Models;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Controllers
{
    public class ReturnController : Controller
    {
        private readonly AppDbContext _context;

        public ReturnController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Return/Request
        public IActionResult Request(int orderId)
        {
            var order = _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .FirstOrDefault(o => o.OrderId == orderId);

            if (order == null)
            {
                return NotFound();
            }

            // Only allow return if the order status is "Paid"
            if (order.Status != "Paid")
            {
                return RedirectToAction("Index", "Order"); // Redirect to order list if not paid
            }

            // Create a ReturnRequest and populate OrderItems
            var returnRequest = new ReturnRequest
            {
                OrderId = order.OrderId,
                OrderItems = order.OrderItems.Select(oi => new ReturnItem
                {
                    ProductId = oi.ProductId,
                    ProductName = oi.Product.Name,
                    Quantity = oi.Quantity
                }).ToList()
            };

            return View(returnRequest);  // Passing the ReturnRequest model with OrderItems to the view
        }


        // POST: Return/Request
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Request(ReturnRequest returnRequest)
        {
            if (ModelState.IsValid)
            {
                var order = await _context.Orders
                    .Include(o => o.OrderItems)
                    .FirstOrDefaultAsync(o => o.OrderId == returnRequest.OrderId);

                if (order == null)
                {
                    return NotFound();
                }

                // Create a return request for each return item
                foreach (var item in returnRequest.ItemsToReturn)
                {
                    var orderItem = order.OrderItems
                        .FirstOrDefault(oi => oi.ProductId == item.ProductId);

                    if (orderItem != null)
                    {
                        var returnRecord = new ReturnRecord
                        {
                            OrderItemId = orderItem.Id,
                            Quantity = item.Quantity,
                            ReturnDate = DateTime.Now,
                            Status = "Pending" // Status when the return is initiated
                        };

                        _context.ReturnRecord.Add(returnRecord);

                        // Update the product stock
                        var product = await _context.Products
                            .FirstOrDefaultAsync(p => p.Id == item.ProductId);

                        if (product != null)
                        {
                            product.Stock += item.Quantity; // Increase stock when product is returned
                        }
                    }
                }

                await _context.SaveChangesAsync();

                return RedirectToAction("Confirmation", new { orderId = order.OrderId });
            }

            return View(returnRequest);
        }

        // GET: Return/Confirmation
        public IActionResult Confirmation(int orderId)
        {
            var order = _context.Orders
                .Include(o => o.OrderItems)
                .FirstOrDefault(o => o.OrderId == orderId);

            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        public async Task<IActionResult> ReturnProduct()
        {
            var result = await _context.Products.ToListAsync();
            return View(result);
        }

        // GET: Return/ProcessReturn
        public async Task<IActionResult> ProcessReturn(int returnRecordId)
        {
            var returnRecord = await _context.ReturnRecord
                .Include(rr => rr.OrderItem)
                .ThenInclude(oi => oi.Product)
                .FirstOrDefaultAsync(rr => rr.Id == returnRecordId);

            if (returnRecord == null)
            {
                return NotFound();
            }

            // Mark the return as processed
            returnRecord.Status = "Processed";

            // Fetch the related order
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.ReturnRecord)
                .FirstOrDefaultAsync(o => o.OrderId == returnRecord.OrderItem.OrderId);

            if (order != null && order.OrderItems.All(oi => oi.ReturnRecord.All(rr => rr.Status == "Processed")))
            {
                order.Status = "Returned";
            }

            await _context.SaveChangesAsync();

            return RedirectToAction("Confirmation", new { orderId = returnRecord.OrderItem.OrderId });
        }

    }
}
