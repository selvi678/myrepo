using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project.Models;

namespace Project.Controllers
{
    [Route("admin")]
    public class AdminController(AppDbContext context) : Controller
    {
        private readonly AppDbContext _context = context;

        // GET: admin/dashboard
        [HttpGet("dashboard")]
        public async Task<IActionResult> Dashboard()
        {
            ViewBag.TotalProducts = await _context.Products.CountAsync();
            ViewBag.TotalOrders = await _context.Orders.CountAsync();
            ViewBag.TotalCategories = await _context.Categories.CountAsync();
            ViewBag.TotalStaff = await _context.Staff.CountAsync();
            return View();
        }

        // GET: admin/products
        [HttpGet("products")]
        public async Task<IActionResult> ManageProducts()
        {
            var products = await _context.Products.Include(p => p.Category).ToListAsync();
            return View(products);
        }

        // GET: admin/orders
        [HttpGet("orders")]
        public async Task<IActionResult> ManageOrders()
        {
            var orders = await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .ToListAsync();
            return View(orders);
        }
        [HttpGet("staff")]
        public async Task<IActionResult> ManageStaff()
        {
            var staff = await _context.Staff.ToListAsync();
            return View(staff);
        }

        // GET: admin/categories
        [HttpGet("categories")]
        public async Task<IActionResult> ManageCategories()
        {
            var categories = await _context.Categories.ToListAsync();
            return View(categories);
        }

        // GET: admin/product/delete/5
        [HttpGet("product/delete/{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return NotFound();

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(ManageProducts));
        }

        // GET: admin/order/delete/5
        [HttpGet("order/delete/{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null) return NotFound();

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(ManageOrders));
        }

        // GET: admin/category/delete/5
        [HttpGet("category/delete/{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null) return NotFound();

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(ManageCategories));
        }
    }
}
