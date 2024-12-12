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
			ViewBag.Categories= await _context.Categories.ToListAsync();
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

		public IActionResult AboutUs()
        {
            return View();
        }
    }
}
