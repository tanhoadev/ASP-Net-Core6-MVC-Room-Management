using Đồ_án.Areas.Admin.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NToastNotify;
using X.PagedList;

namespace Đồ_án.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class DichVuController : Controller
	{
		public KhuTroContext context;
		private readonly IToastNotification toast;
		public DichVuController(KhuTroContext cc, IToastNotification toast)
		{
			context = cc;
			this.toast = toast;
		}
        private bool CheckLogin()
        {
            if (HttpContext.Session.GetString("role") == "Admin")
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
				var tks = context.dichVu.AsNoTracking().ToPagedList(page, pageSize);
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
		public async Task<IActionResult> Create(DichVu tk)
		{
			context.Add(tk);
			await context.SaveChangesAsync();
			toast.AddSuccessToastMessage("Thêm thành công dịch vụ mới", new ToastrOptions { Title = "Thông báo"});
			return RedirectToAction("Index");
		}
		[Route("Admin/[controller]/[action]")]
		public async Task<IActionResult> Update(int id)
		{
			DichVu tk = await context.dichVu.Where(e => e.Id == id).FirstOrDefaultAsync();
			return View(tk);
		}
		[HttpPost]
		[Route("Admin/[controller]/[action]")]
		public async Task<IActionResult> Update(DichVu tk)
		{
			context.Update(tk);
			await context.SaveChangesAsync();
			toast.AddSuccessToastMessage("Cập nhật thành công dịch vụ", new ToastrOptions { Title = "Thông báo" });
			return RedirectToAction("Index");
		}
		[HttpPost]
		[Route("Admin/[controller]/[action]")]
		public async Task<IActionResult> Delete(int id)
		{
			try
			{
				var tk = new DichVu() { Id = id };
				context.Remove(tk);
				await context.SaveChangesAsync();
				toast.AddSuccessToastMessage("Xóa thành công dịch vụ", new ToastrOptions { Title = "Thông báo" });
			}
			catch
			{
				toast.AddErrorToastMessage("Không xóa được dịch vụ này", new ToastrOptions { Title = "Thông báo" });
			}
			return RedirectToAction("Index");
		}
	}
}
