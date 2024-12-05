using Ecommerce.Areas.Admin.ViewModels;
using Ecommerce.Data;
using Ecommerce.Models;
using Ecommerce.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.DependencyResolver;

namespace Ecommerce.Areas.Admin.Controllers
{
	[Area("Admin")]
	//[Authorize(Roles = "Admin")]
	public class AdminController : Controller
	{
		private readonly SignInManager<User> _signInManager;
		private readonly UserManager<User> _userManager;
		private readonly AppDbContext _context;
		public AdminController(SignInManager<User> _signInManager, UserManager<User> _userManager, AppDbContext dbContext)
		{
			this._signInManager = _signInManager;
			this._userManager = _userManager;
			_context = dbContext;
		}
		public IActionResult Index()
		{
			return View("Index");//TODO: replace with dashboard
		}
		[HttpGet]
		public async Task<IActionResult> AddCategory()
		{
			return View("AddCategoryForm");
		}
		[HttpPost]
		public async Task<IActionResult> AddCategory(CategoryViewModel categoryVM)
		{
			var category = new Category
			{
				CategoryName = categoryVM.CategoryName,
			};
			await _context.Categories.AddAsync(category);
			await _context.SaveChangesAsync();
			return RedirectToAction("Index");
		}
		[HttpGet]
		public async Task<IActionResult> AddProduct()
		{
			ViewBag.Categories = await _context.Categories.ToListAsync();
			return View("AddProductForm");
		}
		[HttpPost]
		public async Task<IActionResult> AddProduct(ProductViewModel productVM, IFormFile ProductImageFile)
		{
			//if (ProductImageFile != null && ProductImageFile.Length > 0)
			//{
			//	string imageUrl = await uploadi;
			//}
			var userId = _userManager.GetUserId(User);
			var product = new Product
			{
				ProductName = productVM.ProductName,
				ProductDescription = productVM.ProductDescription,
				ProductPrice = productVM.ProductPrice,
				Currency = productVM.Currency,
				StockQuantity = productVM.StockQuantity,
				//ProductImage=
				//UserId=userId!,
			};
			await _context.Products.AddAsync(product);
			await _context.SaveChangesAsync();

			int generatedId = product.Id;
			List<int> categoriesIds = new List<int>();
			for (int i = 0; i < productVM.SelectedCategoriesIds.Count; i++)
			{
				categoriesIds.Add(productVM.SelectedCategoriesIds[i]);
			}
			for (int i = 0; i < categoriesIds.Count; i++)
			{
				var productCategory = new ProductCategory
				{
					CategoryId = categoriesIds[i],
					ProductId = generatedId,
				};
				await _context.ProductCategories.AddAsync(productCategory);
			}

			await _context.SaveChangesAsync();

			return RedirectToAction("Index");
		}
	}
}
