using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Project.Models;
namespace Project.Controllers;

[Route("staff")]
public class StaffController(AppDbContext _context) : Controller
{
    // GET: staff/dashboard
    [HttpGet("dashboard")]
    public async Task<IActionResult> Dashboard()
    {
        ViewBag.TotalOrders = await _context.Orders.CountAsync();
        ViewBag.TotalDeliveries = await _context.Deliveries.CountAsync();
        ViewBag.TotalReturns = await _context.ReturnRequests.CountAsync();

        return View();
    }

    // GET: staff/orders
    [HttpGet("orders")]
    public async Task<IActionResult> ViewOrders()
    {
        var orders = await _context.Orders
            .Include(o => o.OrderItems)
            .ThenInclude(oi => oi.Product)
            .ToListAsync();

        return View(orders);
    }

    // GET: staff/deliveries
    [HttpGet("deliveries")]
    public async Task<IActionResult> ManageDeliveries()
    {
        var deliveries = await _context.Deliveries
            .Include(d => d.Order)
            .ToListAsync();

        return View(deliveries);
    }

    // GET: staff/returns
    [HttpGet("returns")]
    public async Task<IActionResult> ManageReturns()
    {
        var returns = await _context.ReturnRequests
            .Include(r => r.Order)
            .ToListAsync();

        return View(returns);
    }

    // POST: staff/return/approve/5
    [HttpPost("return/approve/{id}")]
    public async Task<IActionResult> ApproveReturn(int id)
    {
        var request = await _context.ReturnRequests.FindAsync(id);
        if (request == null) return NotFound();

        request.Status = "Approved";
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(ManageReturns));
    }

    // POST: staff/delivery/update/5
    [HttpPost("delivery/update/{id}")]
    public async Task<IActionResult> UpdateDeliveryStatus(int id, string status)
    {
        var delivery = await _context.Deliveries.FindAsync(id);
        if (delivery == null) return NotFound();

        delivery.Status = status;
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(ManageDeliveries));
    }
}
