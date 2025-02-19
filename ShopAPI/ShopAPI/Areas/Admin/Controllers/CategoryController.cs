using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ShopAPI.Models;
using ShopAPI.Repository;

namespace ShopAPI.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class CategoryController : Controller
	{
		private readonly DataContext _dataContext;
	
		public CategoryController(DataContext context )
		{
			_dataContext = context;
			
		}
		public async Task<IActionResult> Index()
		{
			var categories = await _dataContext.Categories
				.OrderByDescending(p => p.Id)
				.ToListAsync();
			return View(categories);
		}

		[HttpGet]
		public IActionResult Create()
		{
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(CategoryModel category)
		{
			// Kiểm tra tên danh mục đã tồn tại chưa
			if (await _dataContext.Categories.AnyAsync(c => c.Name == category.Name))
			{
				ModelState.AddModelError("Name", "Tên danh mục đã tồn tại, vui lòng chọn tên khác.");
				return View(category);
			}

			// Kiểm tra tính hợp lệ của ModelState
			if (!ModelState.IsValid)
			{
				TempData["error"] = "Có lỗi xảy ra. Vui lòng kiểm tra lại!";
				return View(category);
			}

			// Tạo Slug từ Name
			category.Slug = category.Name.Replace(" ", "-").ToLower();

			// Xử lý upload ảnh nếu có (nếu cần)

			// Thêm danh mục vào cơ sở dữ liệu
			_dataContext.Add(category);
			await _dataContext.SaveChangesAsync();

			TempData["success"] = "Thêm danh mục thành công!";
			return RedirectToAction("Index");
		}


		[HttpGet]
		public async Task<IActionResult> Edit(int Id)
		{
			CategoryModel category = await _dataContext.Categories.FindAsync(Id);
			return View(category);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int Id, CategoryModel category)
		{
			var existed_category = await _dataContext.Categories.FindAsync(Id);

			// Kiểm tra xem danh mục có tồn tại không
			if (existed_category == null)
			{
				TempData["error"] = "Danh mục không tồn tại!";
				return RedirectToAction("Index");
			}

			// Kiểm tra trùng lặp tên cho danh mục khác (không phải danh mục hiện tại)
			if (await _dataContext.Categories.AnyAsync(c => c.Name == category.Name && c.Id != Id))
			{
				ModelState.AddModelError("Name", "Tên danh mục đã tồn tại, vui lòng chọn tên khác.");
				return View(category);
			}

			// Cập nhật các trường khác (không thay đổi Name)
			existed_category.Name = category.Name;
			existed_category.Description = category.Description;
			existed_category.Status = category.Status;

			// Lưu thay đổi vào cơ sở dữ liệu
			_dataContext.Update(existed_category);
			await _dataContext.SaveChangesAsync();

			TempData["success"] = "Cập nhật danh mục thành công!";
			return RedirectToAction("Index");
		}


		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Delete(int id)
		{
			var category = await _dataContext.Categories.FindAsync(id);
			if (category == null)
			{
				TempData["error"] = "Sản phẩm không tồn tại!";
				return RedirectToAction("Index");
			}

			// Xóa sản phẩm khỏi database
			_dataContext.Categories.Remove(category);
			await _dataContext.SaveChangesAsync();

			TempData["success"] = "Xóa sản phẩm thành công!";
			return RedirectToAction("Index");
		}
	}
}
