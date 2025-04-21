using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project.Helpers; // Replace 'Project' with your actual project name
using Project.Models;
using Newtonsoft.Json;
using System.Linq;
using System.Threading.Tasks;
namespace Project.Controllers
{
    public class OrderController : Controller
    {
        private readonly AppDbContext _context;  // Use ApplicationDbContext

        public OrderController(AppDbContext context) => _context = context;

        // GET: Order/Index
        public async Task<IActionResult> Index()
        {
            var orders = await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .ToListAsync();
            return View(orders);
        }

        // GET: Order/Details/5
        public async Task<IActionResult> Details(int? id)
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

        // GET: Order/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Order/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ShippingAddress, PaymentMethod")] Order order)
        {
            if (ModelState.IsValid)
            {
                // Retrieve the items from the session/cart
                var cart = GetCart();
                if (cart.Count == 0)  // Prefer Count > 0 instead of Any()
                {
                    return RedirectToAction("Index", "Cart"); // Redirect if cart is empty
                }

                // Create a new order
                order.OrderDate = DateTime.Now;
                order.Status = "Pending"; // Default status
                _context.Add(order);
                await _context.SaveChangesAsync();

                // Add items to the order
                foreach (var cartItem in cart)
                {
                    var orderItem = new OrderItem
                    {
                        ProductId = cartItem.ProductId,
                        Quantity = cartItem.Quantity,
                        Price = cartItem.Price,
                        OrderId = order.OrderId
                    };
                    _context.OrderItems.Add(orderItem);
                }

                await _context.SaveChangesAsync();

                // Clear the cart after placing the order
                SaveCart(new List<CartItem>());

                return RedirectToAction(nameof(Details), new { id = order.OrderId });
            }
            return View(order);
        }

        // Helper to get cart items from session
        private List<CartItem> GetCart()
        {
            var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart");
            return cart ?? new List<CartItem>();
        }

        // Helper to save cart items to session
        private void SaveCart(List<CartItem> cart)
        {
            HttpContext.Session.SetObjectAsJson("Cart", cart);
        }
    }
}
