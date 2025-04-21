using Microsoft.AspNetCore.Mvc;
using Project.Models;
using Project.Helpers; // Replace 'Project' with your actual project name

using Newtonsoft.Json;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Controllers
{
    public class CartController : Controller
    {
        private readonly AppDbContext _context;

        public CartController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Cart
        public IActionResult Index()
        {
            var cart = GetCart();
            return View(cart);
        }
        public IActionResult YourAction()
        {
            var items = _context.Items.ToList(); // Or your appropriate collection
            return View(items); // Pass items to the view
        }


        // Add product to the cart
        public async Task<IActionResult> AddToCart(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            var cart = GetCart();
            var cartItem = cart.FirstOrDefault(c => c.ProductId == id);

            if (cartItem == null)
            {
                cart.Add(new CartItem
                {
                    ProductId = product.Id,
                    ProductName = product.Name,
                    Price = product.Price,
                    Quantity = 1
                });
            }
            else
            {
                cartItem.Quantity++;
            }

            SaveCart(cart);
            return RedirectToAction(nameof(Index));
        }

        // Update the quantity of a product in the cart
        [HttpPost]
        public IActionResult UpdateCart(int productId, int quantity)
        {
            var cart = GetCart();
            var cartItem = cart.FirstOrDefault(c => c.ProductId == productId);

            if (cartItem != null)
            {
                if (quantity > 0)
                {
                    cartItem.Quantity = quantity;
                }
                else
                {
                    cart.Remove(cartItem);
                }
            }

            SaveCart(cart);
            return RedirectToAction(nameof(Index));
        }

        // Remove product from the cart
        public IActionResult RemoveFromCart(int id)
        {
            var cart = GetCart();
            var cartItem = cart.FirstOrDefault(c => c.ProductId == id);

            if (cartItem != null)
            {
                cart.Remove(cartItem);
            }

            SaveCart(cart);
            return RedirectToAction(nameof(Index));
        }

        // Proceed to checkout (a simple placeholder for the action)
        public IActionResult Checkout()
        {
            var cartItems = GetCart();  // Get items from session (or wherever they are stored)

            var total = cartItems.Sum(item => item.Quantity * item.Price);  // Calculate total in controller

            // Pass the total along with the items to the view
            var model = new CheckoutViewModel
            {
                CartItems = cartItems,
                Total = total
            };

            return View(model);
        }


        // Helper to get cart items from session (or temp data, etc.)
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
