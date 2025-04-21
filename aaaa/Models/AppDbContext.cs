using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using YourNamespace.Models;

namespace Project.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Return> Returns { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Wishlist> Wishlists { get; set; }
        public DbSet<Delivery> Deliveries { get; set; }
        public DbSet<ReturnRequest> ReturnRequests { get; set; }
        public DbSet<Staff> Staff { get; set; }
        public DbSet<ReturnRecord> ReturnRecord { get; set; }
        public DbSet<CheckoutViewModel> CheckoutViewModel { get; set; }
        public DbSet<Item> Items { get; set; }
    }
}
