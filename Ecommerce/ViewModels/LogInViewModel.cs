using System.ComponentModel.DataAnnotations;

namespace Ecommerce.ViewModels
{
	public class LogInViewModel
	{
		public string Id { get; set; }

		[Required(ErrorMessage = "Required")]
		public string? UserName { get; set; }

		[Required(ErrorMessage = "Required")]
		public string? Password { get; set; }

		public bool rememberMe { get; set; }
	}
}
