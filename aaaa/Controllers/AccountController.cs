using Microsoft.AspNetCore.Mvc;
using Project.Models;
using System.Linq;

namespace Project.Controllers
{
    [Route("account")]
    public class AccountController : Controller
    {
        private readonly AppDbContext _context;
        public AccountController(AppDbContext context) => _context = context;

        [HttpGet("register")]
        public IActionResult Register() => View();

        [HttpPost("register")]
        public IActionResult Register(User user)
        {
            if (_context.Users.Any(u => u.Email == user.Email))
            {
                ModelState.AddModelError("Email", "Email already exists.");
                return View(user);
            }
            _context.Users.Add(user);
            _context.SaveChanges();
            return RedirectToAction("Login");
        }

        [HttpGet("login")]
        public IActionResult Login() => View();

        [HttpPost("login")]
        public IActionResult Login(User login)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == login.Email && u.Password == login.Password);
            if (user == null)
            {
                ModelState.AddModelError("", "Invalid credentials");
                return View(login);
            }
            return RedirectToAction("Profile", new { id = user.Id });
        }

        [HttpGet("profile/{id}")]
        public IActionResult Profile(int id)
        {
            var user = _context.Users.Find(id);
            return View(user);
        }

        [HttpGet("edit/{id}")]
        public IActionResult Edit(int id)
        {
            var user = _context.Users.Find(id);
            return View(user);
        }

        [HttpPost("edit/{id}")]
        public IActionResult Edit(User updatedUser)
        {
            var user = _context.Users.Find(updatedUser.Id);
            if (user == null) return NotFound();

            user.FullName = updatedUser.FullName;
            user.Email = updatedUser.Email;
            user.Password = updatedUser.Password;

            _context.SaveChanges();
            return RedirectToAction("Profile", new { id = user.Id });
        }
    }
}
