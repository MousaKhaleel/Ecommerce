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
        public async Task<IActionResult> ProductDetails(int id)
        {
            var productFind = _context.Products.FindAsync(id);
            var product = new Product
            {
                Id = productFind.Result.Id,
                ProductName = productFind.Result.ProductName,
                ProductDescription = productFind.Result.ProductDescription,
                ProductPrice = productFind.Result.ProductPrice,
                Currency = productFind.Result.Currency,
                StockQuantity = productFind.Result.StockQuantity,
                //UserId = productFind.Result.UserId,
            };
            return View(product);
        }
        [HttpPost]
        public async Task<IActionResult> AddToCart(int id)
        {
            var product = _context.Products.Where(x => x.Id == id).FirstOrDefault();
            product.StockQuantity -= 1;

            var userId = _userManager.GetUserId(User);
            var userCart = _context.Carts.Where(x => x.UserId == userId).FirstOrDefault();
            var productCart = new ProductCart
            {
                CartId = userCart.Id,
                ProductId = product.Id,
            };
            await _context.productCarts.AddAsync(productCart);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Cart");
        }

        //[HttpPost]
        //public async Task<IActionResult> BuyProduct(int id)
        //{

        //    RedirectToAction("Index", "Cart");
        //}XXXXXXXXXXXXXXXX

        //public async Task<IActionResult> SearchProduct(string productName)
        //{
        //    if (productName != null)
        //    {
        //        var searchResults = _context.Products.Where(x => x.ProductName.Contains(productName)).ToListAsync();
        //    }
        //        return 

    }
}
