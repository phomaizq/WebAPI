using Microsoft.AspNetCore.Mvc;

namespace ShopAPI.Controllers
{
	public class CategoryController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
