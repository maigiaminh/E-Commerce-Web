using System.Data.Entity;

namespace E_Commerce_Web.Models
{
    public class EcommerceContext : DbContext
    {
        public EcommerceContext() : base("name=EcommerceDB") { }

        public DbSet<User> Users { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Coupon> Coupons { get; set; }
    }
}