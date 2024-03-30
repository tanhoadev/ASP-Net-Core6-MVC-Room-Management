using Đồ_án.Areas.Admin.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NToastNotify;
using X.PagedList;

namespace Đồ_án.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class TaiKhoanNguoiDungController : Controller
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
        public TaiKhoanNguoiDungController(KhuTroContext taiKhoanNguoiDung, IToastNotification toast)
        {
            context = taiKhoanNguoiDung;
            this.toast = toast;
        }
        [Route("Admin/[controller]/[action]")]
        public IActionResult Index(int page = 1)
        {
            if (CheckLogin())
            {
				page = page < 1 ? 1 : page;
				int pageSize = 10;
				var tks = context.taikhoanNguoiDung.AsNoTracking().ToPagedList(page, pageSize);
				return View(tks);
            }
            return View("~/Areas/Admin/Views/Share/PageNotFound.cshtml");
        }
		[Route("Admin/[controller]/[action]")]
		public IActionResult Create()
		{
            if (CheckLogin())
            {
				List<SelectListItem> cmnd = context.nguoiThue.Select(x => new SelectListItem { Text = x.hoTen +" - "+ x.CMND_CCCD, Value = x.CMND_CCCD.ToString() }).ToList();
				ViewBag.CMND = cmnd;
				return View();
            }
            return View("~/Areas/Admin/Views/Share/PageNotFound.cshtml");
		}
		[Route("Admin/[controller]/[action]")]
		[HttpPost]
		public async Task<IActionResult> Create(TaikhoanNguoiDung tk)
		{
			TaikhoanNguoiDung taikhoan = await context.taikhoanNguoiDung.Where(x => x.CMND_CCCD == tk.CMND_CCCD).FirstOrDefaultAsync();
			if(taikhoan == null)
			{
				context.Add(tk);
				await context.SaveChangesAsync();
				toast.AddSuccessToastMessage("Thêm thành công 1 tài khoản người dùng mới", new ToastrOptions { Title = "Thông báo" });
			}
			else
			{
				toast.AddWarningToastMessage("Người dùng này đã có tài khoản", new ToastrOptions { Title = "Thông báo" });
			}
			return RedirectToAction("Index");
		}
		[Route("Admin/[controller]/[action]")]
		public async Task<IActionResult> Update(int id)
		{
            if (CheckLogin())
            {
				TaikhoanNguoiDung tk = await context.taikhoanNguoiDung.Where(e => e.Id == id).FirstOrDefaultAsync();
				return View(tk);
            }
            return View("~/Areas/Admin/Views/Share/PageNotFound.cshtml");
		}
		[HttpPost]
		[Route("Admin/[controller]/[action]")]
		public async Task<IActionResult> Update(TaikhoanNguoiDung tk)
		{
            if (CheckLogin())
            {
				context.Update(tk);
				await context.SaveChangesAsync();
				toast.AddSuccessToastMessage("Cập nhật thành công tài khoản người dùng", new ToastrOptions { Title = "Thông báo" });
				return RedirectToAction("Index");
            }
            return View("~/Areas/Admin/Views/Share/PageNotFound.cshtml");
		}
		[HttpPost]
		[Route("Admin/[controller]/[action]")]
		public async Task<IActionResult> Delete(int id)
		{
			try
			{
				var tk = new TaikhoanNguoiDung() { Id = id };
				context.Remove(tk);
				await context.SaveChangesAsync();
				toast.AddSuccessToastMessage("Xóa thành công 1 tài khoản người dùng", new ToastrOptions { Title = "Thông báo" });
			}
			catch
			{
				toast.AddErrorToastMessage("Không xóa được tài khoản người dùng này", new ToastrOptions { Title = "Thông báo" });
			}

			return RedirectToAction("Index");
		}
	}
}
