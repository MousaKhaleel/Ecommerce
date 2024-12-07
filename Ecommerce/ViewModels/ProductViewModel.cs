using Ecommerce.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ecommerce.ViewModels
{
	public class ProductViewModel
	{
		public int Id { get; set; }
		public string ProductName { get; set; }
		public string? ProductDescription { get; set; }

		[Column(TypeName = "decimal(18,2)")]
		[Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
		public decimal ProductPrice { get; set; }

		[MaxLength(3)]
		[RegularExpression("^[A-Z]{3}$", ErrorMessage = "Currency must be a valid code (USD, EUR, etc...)")]
		public string Currency { get; set; }
		public DateTime CreatedDate { get; set; }
		public int StockQuantity { get; set; }
		public string? ProductImage { get; set; }
		//public string UserId { get; set; }
		//public User User { get; set; }

		public List<int>? SelectedCategoriesIds { get; set; }

		public List<ProductCategory>? productCategories { get; set; }
		public List<ProductCart>? productCarts { get; set; }

	}
}
