using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace ShopAPI.Repository.Component
{
	public class CategoriesViewComponent : ViewComponent
	{
		private readonly DataContext _dataContext;

		public CategoriesViewComponent(DataContext context)
		{
			_dataContext = context;
		}

		public async Task<IViewComponentResult> InvokeAsync() => View(await _dataContext.Categories.ToListAsync());
	}
}
