﻿using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Controllers
{
	public class UserController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
