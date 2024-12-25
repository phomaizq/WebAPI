using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.Design;

namespace ShopAPI.Controllers
{
	public class CartController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
		public IActionResult Detail()
		{
			return View("~/Views/Checkout/Index");
		}
	}
}
