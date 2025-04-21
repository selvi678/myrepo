using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project.Models;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Controllers
{
    public class ReviewController : Controller
    {
        private readonly AppDbContext _context;

        public ReviewController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Review/Create/5 (Create a review for a product)
        public IActionResult Create(int productId)
        {
            var product = _context.Products
                .FirstOrDefault(p => p.Id == productId);

            if (product == null)
            {
                return NotFound();
            }

            var review = new Review
            {
                ProductId = productId
            };

            return View(review);
        }

        // POST: Review/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Review review)
        {
            if (ModelState.IsValid)
            {
                review.ReviewDate = DateTime.Now;
                _context.Reviews.Add(review);
                await _context.SaveChangesAsync();

                return RedirectToAction("Details", "Product", new { id = review.ProductId });
            }

            return View(review);
        }

        // GET: Review/List/5 (List all reviews for a product)
        public IActionResult List(int productId)
        {
            var product = _context.Products
                .FirstOrDefault(p => p.Id == productId);

            if (product == null)
            {
                return NotFound();
            }

            var reviews = _context.Reviews
                .Where(r => r.ProductId == productId)
                .Include(r => r.Customer)
                .ToList();

            return View(reviews);
        }

        // GET: Review/Edit/5 (Edit an existing review)
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var review = await _context.Reviews
                .FirstOrDefaultAsync(r => r.rId == id);

            if (review == null)
            {
                return NotFound();
            }

            return View(review);
        }

        // POST: Review/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Review review)
        {
            if (id != review.rId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(review);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Reviews.Any(r => r.rId == review.rId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return RedirectToAction("List", new { productId = review.ProductId });
            }

            return View(review);
        }

        // GET: Review/Delete/5 (Delete a review)
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var review = await _context.Reviews
                .FirstOrDefaultAsync(m => m.rId == id);

            if (review == null)
            {
                return NotFound();
            }

            return View(review);
        }

        // POST: Review/Delete/5 (Confirm delete of review)
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var review = await _context.Reviews.FindAsync(id);
            _context.Reviews.Remove(review);
            await _context.SaveChangesAsync();

            return RedirectToAction("List", new { productId = review.ProductId });
        }
    }
}
