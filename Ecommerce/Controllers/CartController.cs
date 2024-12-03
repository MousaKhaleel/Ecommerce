using Ecommerce.Data;
using Ecommerce.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Controllers
{
	[Authorize]
	public class CartController : Controller
	{
		private readonly AppDbContext _context;
		private readonly UserManager<User> _userManager;

		private IWebHostEnvironment _webHostEnvironment;

		public CartController(AppDbContext dbContext, UserManager<User> userManager, IWebHostEnvironment webHostEnvironment)
		{
			_context = dbContext;
			_userManager = userManager;
			_webHostEnvironment = webHostEnvironment;
		}
		public IActionResult Index()
		{
			return View();
		}
	}
}
