using Ecommerce.Data;
using Ecommerce.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Controllers
{
	public class AccountController : Controller
	{
		private readonly SignInManager<User> _signInManager;
		private readonly UserManager<User> _userManager;
		private readonly AppDbContext _context;
		public AccountController(SignInManager<User> _signInManager, UserManager<User> _userManager, AppDbContext dbContext)
		{
			this._signInManager = _signInManager;
			this._userManager = _userManager;
			_context = dbContext;
		}
		public IActionResult Index()
		{
			return View();
		}
	}
}
