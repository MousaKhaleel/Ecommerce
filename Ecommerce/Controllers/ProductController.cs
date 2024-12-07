﻿using Ecommerce.Data;
using Ecommerce.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
        public async Task<IActionResult> Index( int page = 1, int pageSize=15 )
        {
            //TODO: send the page number
            var productCount = _context.Products.Count();
            var numOfPages = (int)Math.Ceiling((double)productCount / pageSize );
            var productsInPage = await _context.Products.Skip((page -1) * pageSize).Take(pageSize).ToListAsync();
            return View(productsInPage);
        }
        public async Task<IActionResult> ProductDetails(int id)
        {
            var productFind = await _context.Products.FindAsync(id);
            var product = new Product
            {
                Id = productFind.Id,
                ProductName = productFind.ProductName,
                ProductDescription = productFind.ProductDescription,
                ProductPrice = productFind.ProductPrice,
                Currency = productFind.Currency,
                StockQuantity = productFind.StockQuantity,
                //UserId = productFind.UserId,
            };
            return View(product);
        }
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


    }
}
