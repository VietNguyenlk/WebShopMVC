using AppMVC.Models;
using Microsoft.EntityFrameworkCore;

namespace AppMVC.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }
        // Define your DbSet properties for each entity in your application
        // public DbSet<YourEntity> YourEntities { get; set; }
        // Add any additional methods or properties needed for your context
        public DbSet<User> Users { get; set; }  // Example DbSet property
        public DbSet<Product> Products { get; set; } // Example DbSet property
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
      
    }

}
