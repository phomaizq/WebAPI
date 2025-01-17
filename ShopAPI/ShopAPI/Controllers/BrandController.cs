using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopAPI.Models;
using ShopAPI.Repository;

namespace ShopAPI.Controllers
{
	public class BrandController : Controller
	{
		private readonly DataContext _datacontext;
		public BrandController(DataContext datacontext)
		{
			_datacontext = datacontext;
		}
		public async Task<IActionResult> Index(string Slug = "")
		{
			BrandModel brand = _datacontext.Brands.Where(c => c.Slug == Slug).FirstOrDefault();

			if (brand == null) return RedirectToAction("Index");

			var productsByBrand = _datacontext.Products.Where(c => c.BrandId == brand.Id);

			return View(await productsByBrand.OrderByDescending(c => c.Id).ToListAsync());
		}
	}
}
