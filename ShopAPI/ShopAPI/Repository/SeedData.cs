using Microsoft.EntityFrameworkCore;
using ShopAPI.Models;

namespace ShopAPI.Repository
{
	public class SeedData
	{
		public static void SeedingData(DataContext _context)
		{
			_context.Database.Migrate();
			if (!_context.Products.Any()) { 
				CategoryModel macbook = new CategoryModel { Name= "Macbook", Slug= "macbook", Description= "macbook  is Lagre Brand in the world", Status=1 };
				CategoryModel pc = new CategoryModel { Name= "Pc", Slug="pc",Description="Pc  is Lagre Brand in the world",Status=1 };
				BrandModel apple = new BrandModel { Name= "Apple", Slug="apple",Description="Apple  is Lagre Brand in the world",Status=1 };
				BrandModel samsung = new BrandModel { Name= "SamSung", Slug="samsung",Description="Samsung  is Lagre Brand in the world",Status=1 };
				_context.Products.AddRange(
					new ProductModel { Name = "Macbook", Slug = "Macbook", Description = "Macbook is Brand ", Image = "1.jpg", Category = macbook,Brand=apple, Price = 1233, },
					new ProductModel { Name = "Pc", Slug = "Pc", Description = "pc is Brand ", Image = "1.jpg", Category = pc,Brand=samsung, Price = 1233, }
				);
				_context.SaveChanges();
			}
		}
	}
}
