using Ecommerce.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;

namespace Ecommerce.Data
{
	public class AppDbContext: IdentityDbContext<User>
	{
		public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

		public DbSet<Product> Products { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{

			modelBuilder.Entity<Product>()
	.HasOne(b => b.User)
	.WithMany(u => u.ProductsInCart)
	.HasForeignKey(b => b.UserId)
			.OnDelete(DeleteBehavior.Restrict);

			modelBuilder.Entity<Product>()
				.Property(e => e.CreatedDate)
				.HasDefaultValueSql("GETDATE()");

			base.OnModelCreating(modelBuilder);
		}
	}
}
