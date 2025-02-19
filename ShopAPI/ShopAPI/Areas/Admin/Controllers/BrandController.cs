using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopAPI.Models;
using ShopAPI.Repository;

namespace ShopAPI.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class BrandController : Controller
	{
		private readonly DataContext _dataContext;

		public BrandController(DataContext context)
		{
			_dataContext = context;

		}
		public async Task<IActionResult> Index()
		{
			var brand = await _dataContext.Brands
				.OrderByDescending(p => p.Id)
				.ToListAsync();
			return View(brand);
		}

		[HttpGet]
		public IActionResult Create()
		{
			return View();
		}


		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(BrandModel brand)
		{
			// Kiểm tra tên danh mục đã tồn tại chưa
			if (await _dataContext.Brands.AnyAsync(c => c.Name == brand.Name))
			{
				ModelState.AddModelError("Name", "Tên danh mục đã tồn tại, vui lòng chọn tên khác.");
				return View(brand);
			}

			// Kiểm tra tính hợp lệ của ModelState
			if (!ModelState.IsValid)
			{
				TempData["error"] = "Có lỗi xảy ra. Vui lòng kiểm tra lại!";
				return View(brand);
			}

			// Tạo Slug từ Name
			brand.Slug = brand.Name.Replace(" ", "-").ToLower();

			// Xử lý upload ảnh nếu có (nếu cần)

			// Thêm danh mục vào cơ sở dữ liệu
			_dataContext.Add(brand);
			await _dataContext.SaveChangesAsync();

			TempData["success"] = "Thêm danh mục thành công!";
			return RedirectToAction("Index");
		}

		[HttpGet]
		public async Task<IActionResult> Edit(int Id)
		{
			BrandModel brand = await _dataContext.Brands.FindAsync(Id);
			return View(brand);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int Id, BrandModel brand)
		{
			var existed_brands = await _dataContext.Brands.FindAsync(Id);

			// Kiểm tra xem danh mục có tồn tại không
			if (existed_brands == null)
			{
				TempData["error"] = "Danh mục không tồn tại!";
				return RedirectToAction("Index");
			}

			// Kiểm tra trùng lặp tên cho danh mục khác (không phải danh mục hiện tại)
			if (await _dataContext.Brands.AnyAsync(c => c.Name == brand.Name && c.Id != Id))
			{
				ModelState.AddModelError("Name", "Tên danh mục đã tồn tại, vui lòng chọn tên khác.");
				return View(brand);
			}

			// Cập nhật các trường khác (không thay đổi Name)
			existed_brands.Name = brand.Name;
			existed_brands.Description = brand.Description;
			existed_brands.Status = brand.Status;

			// Lưu thay đổi vào cơ sở dữ liệu
			_dataContext.Update(existed_brands);
			await _dataContext.SaveChangesAsync();

			TempData["success"] = "Cập nhật danh mục thành công!";
			return RedirectToAction("Index");
		}


		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Delete(int id)
		{
			var brand = await _dataContext.Brands.FindAsync(id);
			if (brand == null)
			{
				TempData["error"] = "Sản phẩm không tồn tại!";
				return RedirectToAction("Index");
			}

			// Xóa sản phẩm khỏi database
			_dataContext.Brands.Remove(brand);
			await _dataContext.SaveChangesAsync();

			TempData["success"] = "Xóa sản phẩm thành công!";
			return RedirectToAction("Index");
		}
	}
}
