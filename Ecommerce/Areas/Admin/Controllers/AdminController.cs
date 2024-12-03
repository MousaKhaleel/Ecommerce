using Ecommerce.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View("AddProductForm");//dashboard
        }

        //public async Task<IActionResult> AddProduct(ProductViewModel productVM)
        //{

        //}
    }
}
