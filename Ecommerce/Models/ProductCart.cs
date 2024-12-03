using System.ComponentModel.DataAnnotations.Schema;

namespace Ecommerce.Models
{
	public class ProductCart
	{
		[ForeignKey(nameof(ProductId))]
		public int ProductId { get; set; }
		public Product Product { get; set; }

		[ForeignKey(nameof(CartId))]
		public int CartId { get; set; }
		public Cart Cart { get; set; }
	}
}
