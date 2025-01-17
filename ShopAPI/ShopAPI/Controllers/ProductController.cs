using Microsoft.AspNetCore.Mvc;
using ShopAPI.Repository;


namespace ShopAPI.Controllers
{
	public class ProductController : Controller
	{
		private readonly DataContext _datacontext;
		public ProductController(DataContext datacontext)
		{
			_datacontext = datacontext;
		}
		public IActionResult Index()
		{
			return View();
		}
		
		public async Task<IActionResult> Details(int Id) 
		{
			if (Id == null) return RedirectToAction("Index");

			 var productsById = _datacontext.Products.Where(c => c.Id == Id).FirstOrDefault();
			return View(productsById);
		}
	}
}
