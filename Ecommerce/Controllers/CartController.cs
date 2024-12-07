using Ecommerce.Data;
using Ecommerce.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Controllers
{
	[Authorize(Roles = "Customer")]
	//TODO: fix
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
		public async Task<IActionResult> Index()
		{
			var userId = _userManager.GetUserId(User);
			var cart= _context.Carts.Where(x=>x.UserId == userId).FirstOrDefault();
			var ProductsInCartIds = _context.productCarts.Where(x=>x.CartId== cart.Id).ToList();
			var productsInCart = new List<Product>();

			foreach (var item in ProductsInCartIds)
			{
				var product = _context.Products.FirstOrDefault(p => p.Id == item.ProductId);
					productsInCart.Add(product);
			}

			return View(productsInCart);
		}
	}
}
