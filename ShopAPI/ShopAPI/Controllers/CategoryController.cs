using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopAPI.Models;
using ShopAPI.Repository;

namespace ShopAPI.Controllers
{
	public class CategoryController : Controller
	{
		private readonly DataContext _datacontext;
		public CategoryController(DataContext datacontext)
		{
			_datacontext = datacontext;
		}

		public async Task< IActionResult> Index(string Slug = "")
		{
			CategoryModel category = _datacontext.Categories.Where(c => c.Slug == Slug).FirstOrDefault();

			if (category == null) return RedirectToAction("Index");

			var productsByCategory = _datacontext.Products.Where(c => c.CategoryId == category.Id);

			return View(await productsByCategory.OrderByDescending(c => c.Id).ToListAsync());
		}
	}
}
