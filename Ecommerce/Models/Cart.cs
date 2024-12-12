using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Models
{
	public class Cart
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }

		[ForeignKey(nameof(UserId))]
		public string UserId { get; set; }
		public User User { get; set; }

		public List<ProductCart>? productCarts { get; set; }

	}
}
