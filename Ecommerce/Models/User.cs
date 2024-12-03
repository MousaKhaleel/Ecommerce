using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection.Metadata;

namespace Ecommerce.Models
{
	public class User: IdentityUser
	{
		[ForeignKey(nameof(CartId))]
		public string CartId { get; set; }
		public Cart Cart { get; set; }


	}
}
