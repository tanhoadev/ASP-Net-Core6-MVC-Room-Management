using Microsoft.AspNetCore.Mvc;

namespace Đồ_án.Areas.User.Controllers
{
	[Area("User")]
	public class HomeController : Controller
	{
		[Route("User/[controller]/[action]")]
		public IActionResult Index()
		{
			return View();
		}
	}
}
