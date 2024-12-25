using Microsoft.AspNetCore.Mvc;

namespace ShopAPI.Controllers
{
	public class ProductController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	
	}
}
