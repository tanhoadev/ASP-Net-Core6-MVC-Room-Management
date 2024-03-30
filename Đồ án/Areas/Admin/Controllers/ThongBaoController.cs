using Azure;
using Đồ_án.Areas.Admin.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NToastNotify;
using X.PagedList;

namespace Đồ_án.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class ThongBaoController : Controller
	{
		public KhuTroContext context;
		private readonly IToastNotification toast;
		public ThongBaoController(KhuTroContext cc, IToastNotification toast)
		{
			context = cc;
			this.toast = toast;
		}
		private bool CheckLogin()
		{
			if(HttpContext.Session.GetString("role") == "Admin")
			{
				return true;
			}
			return false;
		}
		[Route("Admin/[controller]/[action]")]
		public IActionResult Index(int page = 1)
		{
			if (CheckLogin())
			{
				page = page < 1 ? 1 : page;
				int pageSize = 10;
				var tks = context.thongBao.AsNoTracking().ToPagedList(page, pageSize);
				return View(tks);
			}
			return View("~/Areas/Admin/Views/Share/PageNotFound.cshtml");
        }
        [Route("Admin/[controller]/[action]")]
		public IActionResult Create()
        {
            if (CheckLogin())
            {
				return View();
            }
            return View("~/Areas/Admin/Views/Share/PageNotFound.cshtml");
		}
		[Route("Admin/[controller]/[action]")]
		[HttpPost]
		public async Task<IActionResult> Create(ThongBao tk)
		{
			tk.ngayDang = DateTime.Now;
			context.Add(tk);
			await context.SaveChangesAsync();
			toast.AddSuccessToastMessage("Thêm thành công 1 thông báo mới", new ToastrOptions { Title = "Thông báo" });
			return RedirectToAction("Index");
		}
		[Route("Admin/[controller]/[action]")]
		public async Task<IActionResult> Details(int id)
		{
            if (CheckLogin())
            {
				var thongBao = await context.thongBao.Where(x => x.Id == id).FirstOrDefaultAsync();//chưa thanh toán
				return View(thongBao);
            }
            return View("~/Areas/Admin/Views/Share/PageNotFound.cshtml");
		}
		[Route("Admin/[controller]/[action]")]
		public async Task<IActionResult> Update(int id)
		{
            if (CheckLogin())
            {
				var Thongbao = await context.thongBao.Where(x => x.Id == id).FirstOrDefaultAsync();
				return View(Thongbao);
            }
            return View("~/Areas/Admin/Views/Share/PageNotFound.cshtml");
		}
		[Route("Admin/[controller]/[action]")]
		[HttpPost]
		public async Task<IActionResult> Update(ThongBao thongBao)
		{
			thongBao.ngayDang = DateTime.Now;
			context.Update(thongBao);
			await context.SaveChangesAsync();
			toast.AddSuccessToastMessage("Cập nhật thành công thông báo", new ToastrOptions { Title = "Thông báo" });
			return RedirectToAction("Index");
		}
		[Route("Admin/[controller]/[action]")]
		[HttpPost]
		public async Task<IActionResult> Delete(int id)
		{
			var thongBao = new ThongBao() { Id = id };
			context.Remove(thongBao);
			await context.SaveChangesAsync();
			toast.AddSuccessToastMessage("Xóa thành công thông báo", new ToastrOptions { Title = "Thông báo" });
			return RedirectToAction("Index");
		}
	}
}
