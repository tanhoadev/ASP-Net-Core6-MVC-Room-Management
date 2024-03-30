using Đồ_án.Areas.Admin.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NToastNotify;
using X.PagedList;

namespace Đồ_án.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class ThuePhongController : Controller
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
        public ThuePhongController(KhuTroContext cc, IToastNotification toast)
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
				var tks = context.thuePhong.Include(d => d.nguoiThue).Include(p => p.phong).ToPagedList(page, pageSize);
				return View(tks);
            }
            return View("~/Areas/Admin/Views/Share/PageNotFound.cshtml");
		}
		[Area("Admin")]
		[Route("Admin/[controller]/[action]")]
		public IActionResult Create()
		{
            if (CheckLogin())
            {
				List<SelectListItem> phong = new List<SelectListItem>();
				List<SelectListItem> nguoiThue = new List<SelectListItem>();
				phong = context.phong.Include(x => x.loaiPhong).Select(d => new SelectListItem { Text = d.tenPhong + " - " +d.loaiPhong.tenLoaiPhong, Value = d.Id.ToString() }).ToList();
				nguoiThue = context.nguoiThue.Select(x => new SelectListItem { Text = x.hoTen, Value = x.Id.ToString() }).ToList();
				ViewBag.Phong = phong;//.loaiPhong.tenLoaiPhong
				ViewBag.NguoiThue = nguoiThue;
				return View();
            }
            return View("~/Areas/Admin/Views/Share/PageNotFound.cshtml");
		}
		[Route("Admin/[controller]/[action]")]
		[HttpPost]
		public async Task<IActionResult> Create(ThuePhong thuePhong)
		{
			ThuePhong thue = await context.thuePhong.Where(x => x.IDNguoiThue == thuePhong.IDNguoiThue).FirstOrDefaultAsync();
			if(thue == null)
			{
				Phong phong = await context.phong.FirstOrDefaultAsync(x => x.Id == thuePhong.IDPhong);
				context.Add(thuePhong);
				await context.SaveChangesAsync();
				phong.SoNguoiDangO += 1;
				phong.conPhong = false;
				context.Update(phong);
				await context.SaveChangesAsync();
				toast.AddSuccessToastMessage("Thêm thành phòng cho thuê", new ToastrOptions { Title = "Thông báo" });
				toast.AddSuccessToastMessage("Đã cập nhật lại trạng thái của phòng", new ToastrOptions { Title = "Thông báo" });
			}
			else
			{
				toast.AddWarningToastMessage("Người này đã thuê phòng", new ToastrOptions { Title = "Thông báo" });
			}
			return RedirectToAction("Index");
		}
		[Route("Admin/[controller]/[action]")]
		public async Task<IActionResult> Update(int id)
		{
            if (CheckLogin())
            {
				ThuePhong phg = await context.thuePhong.Where(x => x.Id == id).FirstOrDefaultAsync();
				List<SelectListItem> phong = new List<SelectListItem>();
				List<SelectListItem> nguoiThue = new List<SelectListItem>();
				phong = context.phong.Select(x => new SelectListItem { Text = x.tenPhong, Value = x.Id.ToString() }).ToList();
				nguoiThue = context.nguoiThue.Select(x => new SelectListItem { Text = x.hoTen, Value = x.Id.ToString() }).ToList();
				ViewBag.Phong = phong;
				ViewBag.NguoiThue = nguoiThue;
				return View(phg);
            }
            return View("~/Areas/Admin/Views/Share/PageNotFound.cshtml");
		}
		[HttpPost]
		[Route("Admin/[controller]/[action]")]
		public async Task<IActionResult> Update(ThuePhong thuePhong, int idPhongOld)
		{
			context.Update(thuePhong);
			await context.SaveChangesAsync();
			toast.AddSuccessToastMessage("Cập nhật thành công phòng cho thuê", new ToastrOptions { Title = "Thông báo" });
			var phongold = context.thuePhong.Select(x => x.IDPhong == idPhongOld).ToList();
			var phongNew = context.thuePhong.Select(x => x.IDPhong == thuePhong.IDPhong).ToList();
			Phong phgOld = context.phong.FirstOrDefault(x => x.Id == idPhongOld);
			Phong phongN = context.phong.FirstOrDefault(x => x.Id == thuePhong.IDPhong);
			phgOld.SoNguoiDangO -= 1;
			if(phgOld.SoNguoiDangO == 0)
			{
				phgOld.conPhong = true;
			}
			context.Update(phgOld);
			await context.SaveChangesAsync();
			toast.AddSuccessToastMessage("Đã cập nhật lại trạng thái phòng", new ToastrOptions { Title = "Thông báo" });
			phongN.SoNguoiDangO += 1;
			phongN.conPhong = false;
			context.Update(phongN);
			await context.SaveChangesAsync();

			return RedirectToAction("Index");
		}
		[HttpPost]
		[Route("Admin/[controller]/[action]")]
		public async Task<IActionResult> Delete(int id, int idPhong)
		{
			try
			{
				ThuePhong phg = new ThuePhong { Id = id };
				context.Remove(phg);
				await context.SaveChangesAsync();
				Phong phong = context.phong.FirstOrDefault(x => x.Id == idPhong);
				phong.SoNguoiDangO -= 1;
				if (phong.SoNguoiDangO == 0)
				{
					phong.conPhong = true;
				}
				context.Update(phong);
				context.SaveChanges();
				toast.AddSuccessToastMessage("Đã cập nhật lại trạng thái phòng", new ToastrOptions { Title = "Thông báo" });
				toast.AddSuccessToastMessage("Xóa thành công phòng cho thuê", new ToastrOptions { Title = "Thông báo" });
			}
			catch (Exception ex)
			{
				toast.AddErrorToastMessage("Không thể xóa phòng cho thuê này", new ToastrOptions { Title = "Thông báo" });
			}
			return RedirectToAction("Index");
		}
		[Route("Admin/[controller]/[action]")]
		public async Task<IActionResult> Details(int id)
		{
            if (CheckLogin())
            {
				ThuePhong thue = await context.thuePhong.Include(p => p.phong).Include(n => n.nguoiThue).Where(x => x.Id == id).FirstOrDefaultAsync();
				Phong phg = await context.phong.Include(f => f.loaiPhong).Where(x => x.Id == thue.phong.Id).FirstOrDefaultAsync();
				var dichVu = context.dichVuSuDung.Include(p => p.dichVu).Include(f => f.phong).Where(x => x.IDPhong == thue.phong.Id);
				var dienNuocs = context.dienNuoc.Where(x => x.IDPhong == thue.phong.Id).OrderByDescending(x => x.Id);
				ViewBag.dienNuoc = dienNuocs;
				ViewBag.dichVu = dichVu;
				ViewBag.phong = phg;
				ViewBag.thuePhong = thue;
				return View();
            }
            return View("~/Areas/Admin/Views/Share/PageNotFound.cshtml");
		}
		[Route("Admin/[controller]/[action]")]
		public IActionResult Search()
		{
            if (CheckLogin())
            {
				return RedirectToAction("Index");	
            }
            return View("~/Areas/Admin/Views/Share/PageNotFound.cshtml");
		}
	}
}
