using Azure;
using Đồ_án.Areas.Admin.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NToastNotify;
using X.PagedList;

namespace Đồ_án.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class DichVuSuDungController : Controller
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
        public DichVuSuDungController(KhuTroContext cc, IToastNotification toast)
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
				var tks = context.dichVuSuDung.Include(d => d.phong).Include(e => e.dichVu).ToPagedList(page, pageSize);
				return View(tks);
            }
            return View("~/Areas/Admin/Views/Share/PageNotFound.cshtml");
		}
		[Route("Admin/[controller]/[action]")]
		public IActionResult Create()
		{
            if (CheckLogin())
            {
				List<SelectListItem> phong = new List<SelectListItem>();
				List<SelectListItem> dichVu = new List<SelectListItem>();
				phong = context.phong.Select(x => new SelectListItem { Text = x.tenPhong, Value = x.Id.ToString() }).ToList();
				dichVu = context.dichVu.Select(x => new SelectListItem { Text = x.tenDichVu, Value = x.Id.ToString() }).ToList();
				ViewBag.Phong = phong;
				ViewBag.DichVu = dichVu;
				return View();
            }
            return View("~/Areas/Admin/Views/Share/PageNotFound.cshtml");
		}
		[HttpPost]
		[Route("Admin/[controller]/[action]")]
		public async Task<IActionResult> Create(DichVuSuDung dichVuSuDung)
		{
			try
			{
				context.Add(dichVuSuDung);
				await context.SaveChangesAsync();
				toast.AddSuccessToastMessage("Thêm thành công dịch vụ sử dụng", new ToastrOptions { Title = "Thông báo" });
			}
			catch 
			{
				toast.AddWarningToastMessage("Dịch vụ đã được sử dụng", new ToastrOptions { Title = "Thông báo" });
			}
			return RedirectToAction("Index");
		}
		[HttpPost]
		[Route("Admin/[controller]/[action]")]
		public async Task<IActionResult> Delete(int IDPhong, int IDDichVu)
		{
			try
			{
				var dichVuSuDung = new DichVuSuDung { IDPhong = IDPhong, IDDichVu = IDDichVu };
				context.Remove(dichVuSuDung);
				await context.SaveChangesAsync();
				toast.AddSuccessToastMessage("Xóa thành công dịch vụ sử dụng", new ToastrOptions { Title = "Thông báo" });
			}
			catch 
			{
				toast.AddErrorToastMessage("Không thể xóa", new ToastrOptions { Title = "Thông báo" });
			}
			return RedirectToAction("Index");
		}
	}
}
