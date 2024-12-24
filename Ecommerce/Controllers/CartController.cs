using Ecommerce.Data;
using Ecommerce.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using Stripe.BillingPortal;
using Stripe.Checkout;
using Stripe.FinancialConnections;

namespace Ecommerce.Controllers
{
	[Authorize(Roles = "Customer")]
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
			var cart = _context.Carts.Where(x => x.UserId == userId).FirstOrDefault();
			var ProductsInCartIds = _context.productCarts.Where(x => x.CartId == cart.Id).ToList();
			var productsInCart = new List<Models.Product>();
			decimal totalPrice = 0;

			foreach (var item in ProductsInCartIds)
			{
				var product = _context.Products.FirstOrDefault(p => p.Id == item.ProductId);
				product.ProductImageBase64 = product.ProductImage != null
					? Convert.ToBase64String(product.ProductImage)
					: string.Empty;
				totalPrice += product.ProductPrice * item.ProductQuantityInCart;
				productsInCart.Add(product);
			}
			ViewBag.TotalPrice = totalPrice;

			return View(productsInCart);
		}

		[HttpPost]
		public async Task<IActionResult> CheckOut()
		{
			var userId = _userManager.GetUserId(User);
			var cart = _context.Carts.FirstOrDefault(x => x.UserId == userId);

			var productCartItems = _context.productCarts.Where(x => x.CartId == cart.Id).ToList();

			var domain = "https://localhost:7154/"; //TODO: replace with env
			var sessionOptions = new Stripe.Checkout.SessionCreateOptions
			{
				SuccessUrl = $"{domain}CheckOut/ConfirmOrder",
				CancelUrl = $"{domain}CheckOut/CancelOrder",
				LineItems = new List<SessionLineItemOptions>(),
				Mode = "payment",
			};

			foreach (var item in productCartItems)
			{
				var product = _context.Products.FirstOrDefault(p => p.Id == item.ProductId);

				sessionOptions.LineItems.Add(new SessionLineItemOptions
				{
					PriceData = new SessionLineItemPriceDataOptions
					{
						UnitAmount = (long)(product.ProductPrice * 100),
						Currency = "usd",
						ProductData = new SessionLineItemPriceDataProductDataOptions
						{
							Name = product.ProductName,
							Description = product.ProductDescription,
						},
					},
					Quantity = item.ProductQuantityInCart,
				});
			}

			var sessionService = new Stripe.Checkout.SessionService();
			var session = await sessionService.CreateAsync(sessionOptions);

			ViewBag.SessionId = session.Id;
			ViewBag.PublishableKey = "pk_test_51QXfUCP60Rx3k8Tn0RCgmTmzBpMk0a7QhR08aHNUa1eeYs23xCXyB02D4042U7TolasNFBP7SSRjI70NM1kV7gof00kUKGTYkI";

			return Redirect(session.Url);
		}

		[HttpGet]
		public async Task<IActionResult> ConfirmOrder()
		{
			var userId = _userManager.GetUserId(User);
			var cart = _context.Carts.FirstOrDefault(x => x.UserId == userId);

			var productCartItems = _context.productCarts.Where(x => x.CartId == cart.Id).ToList();

			foreach (var item in productCartItems)
			{
				var product = _context.Products.FirstOrDefault(p => p.Id == item.ProductId);
				if (product != null && product.StockQuantity >= item.ProductQuantityInCart)
				{
					product.StockQuantity -= item.ProductQuantityInCart;
				}
				_context.productCarts.Remove(item);
			}

			_context.Carts.Remove(cart);
			await _context.SaveChangesAsync();

			return View("Success");
		}

		[HttpGet]
		public IActionResult CancelOrder()
		{
			return RedirectToAction("Cart", "Shop");
		}
	}
}
