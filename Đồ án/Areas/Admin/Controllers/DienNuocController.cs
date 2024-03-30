using Đồ_án.Areas.Admin.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NToastNotify;
using X.PagedList;

namespace Đồ_án.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class DienNuocController : Controller
	{
		public KhuTroContext context;
		private readonly IToastNotification toast;
        private bool CheckLogin()
        {
            if (HttpContext.Session.GetString("role") == "Admin")
            {
                return true;
            }
            return false;
        }
        public DienNuocController(KhuTroContext cc, IToastNotification toast)
		{
			context = cc;
			this.toast = toast;
		}
		[Route("Admin/[controller]/[action]")]
		public IActionResult Index(int page = 1)
		{
            if (CheckLogin())
            {
				page = page < 1 ? 1 : page;
				int pageSize = 10;
				var tks = context.dienNuoc.Include(d => d.phong).ToPagedList(page, pageSize);
				return View(tks);
            }
            return View("~/Areas/Admin/Views/Share/PageNotFound.cshtml");
		}
		[Route("Admin/[controller]/[action]")]
		public IActionResult Create()
		{
            if (CheckLogin())
            {
				List<SelectListItem> din = new List<SelectListItem>();
				din = context.phong.Select(x => new SelectListItem { Text = x.tenPhong, Value = x.Id.ToString()}).ToList();
				ViewBag.Phong = din;
				return View();
            }
            return View("~/Areas/Admin/Views/Share/PageNotFound.cshtml");
		}
		[Route("Admin/[controller]/[action]")]
		[HttpPost]
		public async Task<IActionResult> Create(DienNuoc diennuoc)
		{
			context.Add(diennuoc);
			await context.SaveChangesAsync();
			toast.AddSuccessToastMessage("Thêm thành công", new ToastrOptions { Title = "Thông báo" });
			return RedirectToAction("Index");

		}
		[Route("Admin/[controller]/[action]")]
		public async Task<IActionResult> Update(int id)
		{
            if (CheckLogin())
            {
				DienNuoc din = context.dienNuoc.Where(x => x.Id == id).FirstOrDefault();
				List<SelectListItem> phong = new List<SelectListItem>();
				phong = context.phong.Select(x => new SelectListItem { Text = x.tenPhong, Value = x.Id.ToString() }).ToList();
				ViewBag.Phong = phong;
				return View(din);
            }
            return View("~/Areas/Admin/Views/Share/PageNotFound.cshtml");
		}
		[Route("Admin/[controller]/[action]")]
		[HttpPost]
		public async Task<IActionResult> Update(DienNuoc dienNuoc)
		{
			context.Update(dienNuoc);
			await context.SaveChangesAsync();
			toast.AddSuccessToastMessage("Cập nhật thành công", new ToastrOptions { Title = "Thông báo" });
			return RedirectToAction("Index");
		}
		[Route("Admin/[controller]/[action]")]
		[HttpPost]
		public async Task<IActionResult> Delete(int id)
		{
			var dienNuoc = new DienNuoc { Id = id};
			context.Remove(dienNuoc);
			await context.SaveChangesAsync();
			toast.AddSuccessToastMessage("Xóa thành công", new ToastrOptions { Title = "Thông báo" });
			return RedirectToAction("Index");
		}
	}
}
