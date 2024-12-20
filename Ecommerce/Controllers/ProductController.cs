﻿using Ecommerce.Data;
using Ecommerce.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.CodeAnalysis.Elfie.Diagnostics;
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
		public async Task<IActionResult> Index(int page = 1, int pageSize = 15)
		{
			//TODO: send the page number
			var productCount = _context.Products.Count();
			var numOfPages = (int)Math.Ceiling((double)productCount / pageSize);
			var productsInPage = await _context.Products.Where(x=>x.StockQuantity>0).Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
			foreach (var product in productsInPage)
			{
				product.ProductImageBase64 = product.ProductImage != null
					? Convert.ToBase64String(product.ProductImage)
					: string.Empty;
			}
			return View(productsInPage);
		}
		public async Task<IActionResult> ProductDetails(string d)
		{
			var id = int.Parse(d); //TODO: fix
			var productFind = await _context.Products.FindAsync(id);
			var product = new Product
			{
				Id = productFind.Id,
				ProductName = productFind.ProductName,
				ProductDescription = productFind.ProductDescription,
				ProductPrice = productFind.ProductPrice,
				Currency = productFind.Currency,
				StockQuantity = productFind.StockQuantity,

				ProductImageBase64= productFind.ProductImage != null
					? Convert.ToBase64String(productFind.ProductImage)
					: string.Empty,
			};
			return View(product);
		}
		[Authorize]
		public async Task<IActionResult> AddToCart(int id)
		{
			var product = _context.Products.Where(x => x.Id == id).FirstOrDefault();

			var userId = _userManager.GetUserId(User);
			var userCart = _context.Carts.Where(x => x.UserId == userId).FirstOrDefault();
			var productCart = await _context.productCarts.Where(x => x.CartId == userCart.Id).Where(y => y.ProductId == id).Include(y => y.Product).FirstOrDefaultAsync();
			if (productCart == null)
			{
				productCart = new ProductCart
				{
					CartId = userCart.Id,
					ProductId = product.Id,
					ProductQuantityInCart = 1
				};
				await _context.productCarts.AddAsync(productCart);
			}

			else
			{
				productCart.ProductQuantityInCart += 1;
			}

				product.StockQuantity -= 1;

				await _context.SaveChangesAsync();

			var hubContext = (IHubContext<ProductHub>)HttpContext.RequestServices.GetService(typeof(IHubContext<ProductHub>));
			await hubContext.Clients.All.SendAsync("ReceiveStockUpdate", product.Id, product.StockQuantity);

			return RedirectToAction("Index", "Home");
		}
		//TODO: add signalr
		public async Task<IActionResult> RemoveFromCart(int id)
		{
			var product = _context.Products.Where(x => x.Id == id).FirstOrDefault();

			var userId = _userManager.GetUserId(User);
			var userCart = _context.Carts.Where(x => x.UserId == userId).FirstOrDefault();
			var productCart = await _context.productCarts.Where(x => x.CartId == userCart.Id).Include(y => y.Product).FirstOrDefaultAsync();
			if (productCart.ProductQuantityInCart > 1)
			{
				productCart.ProductQuantityInCart -= 1;
			}
			else
			{
				_context.productCarts.Remove(productCart);
			}
			product.StockQuantity += 1;

			await _context.SaveChangesAsync();

			return RedirectToAction("Index", "Cart");
		}


        public async Task<IActionResult> SearchProduct(string productName)
        {
            if (productName != null)
            {
                var searchResults = await _context.Products.Where(x => x.ProductName.Contains(productName)).ToListAsync();
				foreach (var product in searchResults)
				{
					product.ProductImageBase64 = product.ProductImage != null
						? Convert.ToBase64String(product.ProductImage)
						: string.Empty;
				}
				ViewBag.Categories = await _context.Categories.ToListAsync();
                return View("Index", searchResults);
            }
            return View("Index", productName);
        }

        public async Task<IActionResult> ProductByCategory(int id)
		{
			var products = await _context.ProductCategories.Where(pc => pc.CategoryId == id).Select(pc => pc.Product).ToListAsync();
			foreach (var product in products)
			{
				product.ProductImageBase64 = product.ProductImage != null
					? Convert.ToBase64String(product.ProductImage)
					: string.Empty;
			}
			ViewBag.Categories = await _context.Categories.ToListAsync();
			return View("Index", products);
		}

		//[HttpPost]
		//public async Task<IActionResult> BuyProduct(int id)
		//{

		//    RedirectToAction("Index", "Cart");
		//}XXXXXXXXXXXXXXXX


	}
}
