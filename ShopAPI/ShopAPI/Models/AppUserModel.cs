using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ShopAPI.Models
{
	public class AppUserModel : IdentityUser
	{
		public string Ocupation { get; set; }
	}
}
