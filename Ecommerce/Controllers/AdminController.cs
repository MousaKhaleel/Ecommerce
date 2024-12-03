using Ecommerce.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Controllers
{
	[Authorize(Roles = "Admin")]
	public class AdminController : Controller
	{
		public IActionResult Index()
		{
			return View();//dashboard
		}

		//public async Task<IActionResult> AddProduct(ProductViewModel productVM)
		//{

		//}
	}
}
