using Ecommerce.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;

namespace Ecommerce.Data
{
    public class AppDbContext : IdentityDbContext<User>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Product> Products { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<ProductCart> productCarts { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            //		modelBuilder.Entity<Product>()
            //.HasOne(b => b.User)
            //.WithMany(u => u.Cart)
            //.HasForeignKey(b => b.UserId)
            //		.OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<User>()
.HasOne(u => u.Cart)
.WithOne(c => c.User)
.HasForeignKey<Cart>(c => c.UserId);

            modelBuilder.Entity<ProductCart>()
    .HasKey(tc => new { tc.ProductId, tc.CartId });

            modelBuilder.Entity<ProductCategory>()
    .HasKey(tc => new { tc.ProductId, tc.CategoryId });

            modelBuilder.Entity<ProductCart>()
.HasOne(pc => pc.Product)
.WithMany(p => p.productCarts)
.HasForeignKey(pc => pc.ProductId)
.OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ProductCart>()
                .HasOne(pc => pc.Cart)
                .WithMany(c => c.productCarts)
                .HasForeignKey(pc => pc.CartId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Product>()
                .Property(e => e.CreatedDate)
                .HasDefaultValueSql("GETDATE()");

            base.OnModelCreating(modelBuilder);
        }
    }
}
