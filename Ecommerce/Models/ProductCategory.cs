using System.ComponentModel.DataAnnotations.Schema;

namespace Ecommerce.Models
{
	public class ProductCategory
	{
		[ForeignKey(nameof(ProductId))]
		public int ProductId { get; set; }
		public Product Product { get; set; }

		[ForeignKey(nameof(CategoryId))]
		public int CategoryId { get; set; }
		public Category Category { get; set; }
	}
}
