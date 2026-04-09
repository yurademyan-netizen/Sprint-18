using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace TaskAuthenticationAuthorization.Models
{
    public class ShoppingContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<SuperMarket> SuperMarkets { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }

        public ShoppingContext(DbContextOptions<ShoppingContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            string adminRoleName = "admin";
            string userRoleName = "user";
            string buyerRoleName = "buyer";

            string adminEmail = "admin@gmail.com";
            string adminPassword = "admin123";

            string buyerEmail = "buyer@gmail.com";
            string buyerPassword = "buyer123";

            Role adminRole = new Role { Id = 1, RoleName = adminRoleName };
            Role userRole = new Role { Id = 2, RoleName = userRoleName };
            Role buyerRole = new Role { Id = 3, RoleName = buyerRoleName };
            User adminUser = new User { Id = 1, Email = adminEmail, Password = adminPassword, Type = "none", RoleId = adminRole.Id };
            User buyerUser = new User { Id = 2, Email = buyerEmail, Password = buyerPassword, Type = "regular", RoleId = buyerRole.Id };

            modelBuilder.Entity<Role>().HasData(adminRole, userRole, buyerRole);
            modelBuilder.Entity<User>().HasData(adminUser, buyerUser);
            base.OnModelCreating(modelBuilder);
        }
    }
}
