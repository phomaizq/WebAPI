using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ShopAPI.Models;
using ShopAPI.Repository;

namespace ShopAPI.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class ProductController : Controller
	{
		private readonly DataContext _dataContext;
		private readonly IWebHostEnvironment _webHostEnvironment;

		public ProductController(DataContext context, IWebHostEnvironment webHostEnvironment)
		{
			_dataContext = context;
			_webHostEnvironment = webHostEnvironment;
		}

		public async Task<IActionResult> Index()
		{
			var products = await _dataContext.Products
				.OrderByDescending(p => p.Id)
				.Include(p => p.Category)
				.Include(p => p.Brand)
				.ToListAsync();

			return View(products);
		}

		[HttpGet]
		public IActionResult Create()
		{
			ViewBag.Categories = new SelectList(_dataContext.Categories, "Id", "Name");
			ViewBag.Brands = new SelectList(_dataContext.Brands, "Id", "Name");
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(ProductModel product)
		{
			ViewBag.Categories = new SelectList(_dataContext.Categories, "Id", "Name", product.CategoryId);
			ViewBag.Brands = new SelectList(_dataContext.Brands, "Id", "Name", product.BrandId);

			// Kiểm tra tên sản phẩm đã tồn tại chưa
			if (await _dataContext.Products.AnyAsync(p => p.Name == product.Name))
			{
				ModelState.AddModelError("Name", "Tên sản phẩm đã tồn tại, vui lòng chọn tên khác.");
				return View(product);
			}

			if (!ModelState.IsValid)
			{
				TempData["error"] = "Có lỗi xảy ra. Vui lòng kiểm tra lại!";
				return View(product);
			}

			// Tạo Slug
			product.Slug = product.Name.Replace(" ", "-").ToLower();

			// Xử lý upload ảnh
			if (product.ImageUpload != null)
			{
				string uploadsDir = Path.Combine(_webHostEnvironment.WebRootPath, "images/upload");
				if (!Directory.Exists(uploadsDir)) Directory.CreateDirectory(uploadsDir);

				string extension = Path.GetExtension(product.ImageUpload.FileName).ToLower();
				string imageName = $"{Guid.NewGuid()}{extension}";
				string filePath = Path.Combine(uploadsDir, imageName);

				using (FileStream fs = new FileStream(filePath, FileMode.Create))
				{
					await product.ImageUpload.CopyToAsync(fs);
				}

				product.Image = imageName;
			}

			_dataContext.Add(product);
			await _dataContext.SaveChangesAsync();

			TempData["success"] = "Thêm sản phẩm thành công!";
			return RedirectToAction("Index");
		}

		[HttpGet]
		public async Task<IActionResult> Edit(int Id )
		{
			ProductModel product =  await _dataContext.Products.FindAsync(Id);
			ViewBag.Categories = new SelectList(_dataContext.Categories, "Id", "Name", product.CategoryId);
			ViewBag.Brands = new SelectList(_dataContext.Brands, "Id", "Name", product.BrandId);
			return View(product);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int Id, CategoryModel category)
		{
			var existed_category = await _dataContext.Categories.FindAsync(Id);
			if (existed_category == null)
			{
				TempData["error"] = "Danh mục không tồn tại!";
				return RedirectToAction("Index");
			}

			// Kiểm tra nếu tên đã tồn tại nhưng là của danh mục khác (không cho trùng tên)
			if (await _dataContext.Categories.AnyAsync(c => c.Name == category.Name && c.Id != Id))
			{
				ModelState.AddModelError("Name", "Tên danh mục đã tồn tại, vui lòng chọn tên khác.");
				return View(category);
			}

			// Nếu ModelState không hợp lệ, trả lại View với lỗi
			if (!ModelState.IsValid)
			{
				TempData["error"] = "Có lỗi xảy ra. Vui lòng kiểm tra lại!";
				return View(category);
			}

			// Không cho phép thay đổi Name, giữ nguyên Name cũ
			category.Name = existed_category.Name;  // Giữ nguyên Name cũ của danh mục

			// Cập nhật thông tin danh mục, không thay đổi Name
			existed_category.Description = category.Description;
			existed_category.Slug = category.Slug;
			existed_category.Status = category.Status;

			// Lưu thay đổi vào database
			_dataContext.Update(existed_category);
			await _dataContext.SaveChangesAsync();

			TempData["success"] = "Cập nhật danh mục thành công!";
			return RedirectToAction("Index");
		}


		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Delete(int id)
		{
			var product = await _dataContext.Products.FindAsync(id);
			if (product == null)
			{
				TempData["error"] = "Sản phẩm không tồn tại!";
				return RedirectToAction("Index");
			}

			// Xóa ảnh nếu có
			if (!string.IsNullOrEmpty(product.Image))
			{
				string imagePath = Path.Combine(_webHostEnvironment.WebRootPath, "images/upload", product.Image);
				if (System.IO.File.Exists(imagePath))
				{
					System.IO.File.Delete(imagePath);
				}
			}

			// Xóa sản phẩm khỏi database
			_dataContext.Products.Remove(product);
			await _dataContext.SaveChangesAsync();

			TempData["success"] = "Xóa sản phẩm thành công!";
			return RedirectToAction("Index");
		}



	}
}
