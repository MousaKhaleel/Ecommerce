using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Models
{
	public class Product
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }
		public string ProductName { get; set; }
		public DateTime CreatedDate { get; set; }
		public int AvailableStockCount { get; set; }
		public string? ProductImage { get; set; }

		[ForeignKey(nameof(UserId))]
		public string UserId { get; set; }
		public User User { get; set; }

		//public bool IsDeleted { get; set; }

		public List<ProductCategory>? productCategories { get; set; }


	}
}
