﻿using System.ComponentModel.DataAnnotations;

namespace ShopAPI.Models
{
	public class CategoryModel
	{
		[Key]
		public int Id { get; set; }
		[Required( ErrorMessage = "Yêu cầu nhập tên danh mục...")]
		public string Name { get; set; }
		[Required ( ErrorMessage = "Yêu cầu nhập tên mô tả")]
		public string Description { get; set; }
		public string Slug { get; set; }
		public int Status { get; set; }
	}
}
