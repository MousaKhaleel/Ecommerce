using Ecommerce.Data;
using Ecommerce.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Controllers
{
	public class ProductController : Controller
	{
		private readonly SignInManager<User> _signInManager;
		private readonly UserManager<User> _userManager;
		private readonly AppDbContext _context;
		public ProductController(SignInManager<User> _signInManager, UserManager<User> _userManager, AppDbContext dbContext)
		{
			this._signInManager = _signInManager;
			this._userManager = _userManager;
			_context = dbContext;
		}
		public IActionResult Index()
		{
			var allProducts = _context.Products.ToListAsync(); 
			return View(allProducts);
		}
	}
}
