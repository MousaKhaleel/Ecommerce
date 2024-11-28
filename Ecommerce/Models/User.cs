using Microsoft.AspNetCore.Identity;
using System.Reflection.Metadata;

namespace Ecommerce.Models
{
	public class User: IdentityUser
	{
		public List<Product>? ProductsInCart { get; set; }


	}
}
