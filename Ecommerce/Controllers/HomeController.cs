using Ecommerce.Data;
using Ecommerce.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Ecommerce.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
		private readonly AppDbContext _context;

		public HomeController(ILogger<HomeController> logger, AppDbContext dbContext)
		{
			_logger = logger;
			_context = dbContext;
		}

		public async Task<IActionResult> Index()
		{
			var Products = await _context.Products.Where(x => x.StockQuantity > 0).ToListAsync();
			//TODO: move
			return View(Products);
		}

		public IActionResult Privacy()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}

		public async Task<IActionResult> SearchProduct(string productName)
		{
			if (productName != null)
			{
				var searchResults = await _context.Products.Where(x => x.ProductName.Contains(productName)).ToListAsync();
				return View("Index", searchResults);
			}
			return View("Index", productName);
		}

		public IActionResult AboutUs()
        {
            return View();
        }
    }
}
