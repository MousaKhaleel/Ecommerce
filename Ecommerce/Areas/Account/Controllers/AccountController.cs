using Ecommerce.Areas.Account.ViewModels;
using Ecommerce.Data;
using Ecommerce.Models;
using Ecommerce.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Areas.Account.Controllers
{
	[Area("Account")]
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
            return View("LogIn");
        }
        [HttpGet]
        public async Task<IActionResult> LogIn()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> LogIn(LogInViewModel viewModel)
        {
            var result = await _signInManager.PasswordSignInAsync(viewModel.UserName, viewModel.Password, viewModel.rememberMe, false);
            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return View(viewModel);
            }
        }
        [HttpGet]
        public async Task<IActionResult> Register()
        {
            return View("Register");
        }
        public async Task<IActionResult> Register(RegisterViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                User user = new()
                {
                    UserName = viewModel.UserName,
                    Email = viewModel.Email,
                    PhoneNumber = viewModel.PhoneNumber,
                    EmailConfirmed = false,
                    PhoneNumberConfirmed = false,
                    TwoFactorEnabled = false,
                    LockoutEnabled = false,
                    AccessFailedCount = 0,
                };
                var result = await _userManager.CreateAsync(user, viewModel.Password);
                if (result.Succeeded)
                {
                    var roleResult = await _userManager.AddToRoleAsync(user, "Customer");
                    if (roleResult.Succeeded)
                    {
                        await _signInManager.SignInAsync(user, false);
                        return RedirectToAction("Index", "Home");
                    }
                }
            }
            return View(viewModel);
        }
        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            var userId = _userManager.GetUserId(User);

            var user = await _userManager.FindByIdAsync(userId);

            var userVM = new UserViewModel
            {
                UserId = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
            };
            return View(userVM);
        }
        public async Task<IActionResult> EditProfile(UserViewModel userEdit)
        {
            var user = await _userManager.FindByIdAsync(userEdit.UserId);

            await _userManager.SetEmailAsync(user, userEdit.Email);
            await _userManager.SetPhoneNumberAsync(user, userEdit.PhoneNumber);

            if (userEdit.Password != null)
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                await _userManager.ResetPasswordAsync(user, token, userEdit.Password!);
            }

            await _userManager.UpdateAsync(user);

            return View("Profile");
        }
    }
}
