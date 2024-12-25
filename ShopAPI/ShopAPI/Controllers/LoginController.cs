using Microsoft.AspNetCore.Mvc;

namespace ShopAPI.Controllers
{
	public class LoginController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
