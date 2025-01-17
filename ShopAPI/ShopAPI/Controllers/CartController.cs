using Microsoft.AspNetCore.Mvc;
using ShopAPI.Models;
using ShopAPI.Models.ViewModels;
using ShopAPI.Repository;
using System.ComponentModel.Design;

namespace ShopAPI.Controllers
{
	public class CartController : Controller
	{
		private readonly DataContext _datacontext;
		public CartController(DataContext datacontext)
		{
			_datacontext = datacontext;
		}
		public IActionResult Index()
		{
			List<CartItemModel> cartItems = HttpContext.Session.GetJson<List<CartItemModel>>("Cart") ?? new List<CartItemModel>();

			CartItemViewModel cartVM = new()
			{
				CartItems = cartItems,
				GrandTotal = cartItems.Sum(x => x.Quantity * x.Price)
			};
			return View(cartVM);
		}
		public IActionResult Detail()
		{
			return View("~/Views/Checkout/Index");
		}
		public async Task<IActionResult> Add(int Id)
		{
			ProductModel product = await _datacontext.Products.FindAsync(Id);

			List<CartItemModel> cart = HttpContext.Session.GetJson<List<CartItemModel>>("Cart") ?? new List<CartItemModel>();

			CartItemModel cartItems = cart.Where(c => c.ProductId == Id).FirstOrDefault();


			if (cartItems == null)
			{
				cart.Add(new CartItemModel(product));
			}
			else
			{
				cartItems.Quantity += 1;
			}
			HttpContext.Session.SetJson("Cart", cart);
			return Redirect(Request.Headers["Referer"].ToString());
		}
		public async Task<IActionResult> Decrease(int id)
		{
			List<CartItemModel> cart = HttpContext.Session.GetJson<List<CartItemModel>>("Cart");

			CartItemModel cartItem = cart.Where(x => x.ProductId == id).FirstOrDefault();
			if (cartItem.Quantity > 1)
			{
				--cartItem.Quantity;
			}
			else
			{
				cart.RemoveAll(p => p.ProductId == id);
			}
			if(cart.Count == 0)
			{
				HttpContext.Session.Remove("Cart");
			}else
			{
				HttpContext.Session.SetJson("Cart", cart);
			}

			return RedirectToAction("Index");
		}

	}

}
