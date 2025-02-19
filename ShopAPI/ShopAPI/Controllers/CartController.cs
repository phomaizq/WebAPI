using Microsoft.AspNetCore.Mvc;
using ShopAPI.Models;
using ShopAPI.Models.ViewModels;
using ShopAPI.Repository;


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
			// Retrieve the cart from the session
			List<CartItemModel> cart = HttpContext.Session.GetJson<List<CartItemModel>>("Cart") ?? new List<CartItemModel>();

			// Find the item to decrease
			var cartItem = cart.FirstOrDefault(x => x.ProductId == id);

			if (cartItem != null)
			{
				if (cartItem.Quantity >= 1)
				{
					// Decrease the quantity
					cartItem.Quantity--;
				}
				else
				{
					// Remove the item if quantity is 1
					cart.RemoveAll(x => x.ProductId == id);
				}
			}

			// Update or clear the cart in the session
			if (cart.Count == 0)
			{
				HttpContext.Session.Remove("Cart");
			}
			else
			{
				HttpContext.Session.SetJson("Cart", cart);
			}

			// Redirect back to the referring page
			return Redirect(Request.Headers["Referer"].ToString());
		}
		public async Task<IActionResult> Increase(int id)
		{

			// Retrieve the cart from the session
			List<CartItemModel> cart = HttpContext.Session.GetJson<List<CartItemModel>>("Cart") ?? new List<CartItemModel>();

			// Find the item to increase
			var cartItem = cart.FirstOrDefault(x => x.ProductId == id);

			if (cartItem != null)
			{
				// Increase the quantity
				cartItem.Quantity++;
			}

			// Update the cart in the session
			HttpContext.Session.SetJson("Cart", cart);

			// Redirect back to the referring page
			return Redirect(Request.Headers["Referer"].ToString());
		}
		public async Task<IActionResult> Remove(int id)
		{

			List<CartItemModel> carts = HttpContext.Session.GetJson<List<CartItemModel>>("Cart");
			CartItemModel cartItem = carts.Where(c => c.ProductId == id).FirstOrDefault();

			carts.RemoveAll(c => c.ProductId == id);

			if (carts.Count == 0)
			{
				HttpContext.Session.Remove("Cart");
			}
			else
			{
				HttpContext.Session.SetJson("Cart", carts);
			}
			return Redirect(Request.Headers["Referer"].ToString());
		}
		public async Task<IActionResult> Clear()
		{
			HttpContext.Session.Remove("Cart");

			// Redirect back to the referring page
			return RedirectToAction("Index");
		}
	}

}
