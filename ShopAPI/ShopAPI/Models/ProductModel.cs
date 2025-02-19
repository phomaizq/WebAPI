using ShopAPI.Repository.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace ShopAPI.Models
{
	public class ProductModel
	{
		[Key]
		public int Id { get; set; }

		[Required, MinLength(4, ErrorMessage = "Yêu cầu nhập tên sản phẩm...")]
		public string Name { get; set; }

		public string Slug { get; set; }

		[Required, MinLength(4, ErrorMessage = "Yêu cầu nhập mô tả sản phẩm")]
		public string Description { get; set; }

		[Required, Range(0.01, double.MaxValue, ErrorMessage = "Giá sản phẩm phải lớn hơn 0.")]
		public decimal Price { get; set; }

		public string Image { get; set; }
		public int BrandId { get; set; }
		public int CategoryId { get; set; }
		public CategoryModel Category { get; set; }
		public BrandModel Brand { get; set; }

		[NotMapped]
		[FileExtension(ErrorMessage = "Vui lòng tải lên định dạng ảnh hợp lệ (.jpg, .png, .jpeg, .webp)")]
		public IFormFile ImageUpload { get; set; }
	}
}
