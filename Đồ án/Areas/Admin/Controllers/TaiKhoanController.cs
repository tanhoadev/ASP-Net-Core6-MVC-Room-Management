using Azure;
using Đồ_án.Areas.Admin.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NToastNotify;
using X.PagedList;

namespace Đồ_án.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class TaiKhoanController : Controller
	{
		private KhuTroContext context;
		private readonly IToastNotification toast;
        private bool CheckLogin()
        {
            if (HttpContext.Session.GetString("role") == "Admin")
            {
                return true;
            }
            return false;
        }
        public TaiKhoanController(KhuTroContext cc, IToastNotification toast)
		{
			context = cc;
			this.toast = toast;
		}
		[Route("Admin/[controller]/[action]")]
		public IActionResult Index(int page=1)
		{
            if (CheckLogin())
            {
				page = page < 1 ? 1 : page;
				int pageSize = 10;
				var tks = context.taiKhoan.AsNoTracking().ToPagedList(page, pageSize);
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
		public async Task <IActionResult> Create(TaiKhoan tk)
		{
			context.Add(tk);
			await context.SaveChangesAsync();
			toast.AddSuccessToastMessage("Thêm thành công tài khoản", new ToastrOptions { Title = "Thông báo" });
			return RedirectToAction("Index");
		}
		[Route("Admin/[controller]/[action]")]
		public async Task <IActionResult> Update(int id)
		{
            if (CheckLogin())
            {
				TaiKhoan tk = await context.taiKhoan.Where(e => e.Id == id).FirstOrDefaultAsync();
				return View(tk);
            }
            return View("~/Areas/Admin/Views/Share/PageNotFound.cshtml");
		}
		[HttpPost]
		[Route("Admin/[controller]/[action]")]
		public async Task<IActionResult> Update(TaiKhoan tk)
		{
			context.Update(tk);
			await context.SaveChangesAsync();
			toast.AddSuccessToastMessage("Cập nhật thành công tài khoản", new ToastrOptions { Title = "Thông báo" });
			return RedirectToAction("Index");
		}
		[HttpPost]
		[Route("Admin/[controller]/[action]")]
		public async Task<IActionResult> Delete(int id)
		{
			var tk = new TaiKhoan() { Id = id};
			context.Remove(tk);
			await context.SaveChangesAsync();
			toast.AddSuccessToastMessage("Xóa thành công tài khoản", new ToastrOptions { Title = "Thông báo" });
			return RedirectToAction("Index");
		}
    }
}
