using Ecommerce.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ecommerce.ViewModels
{
	public class ProductViewModel
	{
		public int Id { get; set; }
		public string ProductName { get; set; }
		public DateTime CreatedDate { get; set; }
		public int StockQuantity { get; set; }
		public string? ProductImage { get; set; }
		public string UserId { get; set; }
		public User User { get; set; }

		public List<ProductCategory>? productCategories { get; set; }
		public List<ProductCart>? productCarts { get; set; }

	}
}
