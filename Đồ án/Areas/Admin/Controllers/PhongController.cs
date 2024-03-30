using Azure;
using Đồ_án.Areas.Admin.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NToastNotify;
using System.Drawing.Printing;
using X.PagedList;

namespace Đồ_án.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class PhongController : Controller
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
        public PhongController(KhuTroContext cc, IToastNotification toast)
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
				var tks = context.phong.Include(d => d.loaiPhong).Include(e => e.dayTro).ToPagedList(page, pageSize);
				List<SelectListItem> loaiPhong = context.loaiPhong.Select(x => new SelectListItem { Text = x.tenLoaiPhong, Value = x.tenLoaiPhong }).ToList();
				List<SelectListItem> dayTro = context.dayTro.Select(x => new SelectListItem { Text = x.tenDayTro, Value = x.tenDayTro.ToString() }).ToList();
				ViewBag.loaiPhong = loaiPhong;
				ViewBag.dayTro = dayTro;
				return View(tks);
            }
            return View("~/Areas/Admin/Views/Share/PageNotFound.cshtml");
		}
		[Route("Admin/[controller]/[action]")]
		public IActionResult Create()
		{
            if (CheckLogin())
            {
				List<SelectListItem> lPhong = new List<SelectListItem>();
				List<SelectListItem> dTro = new List<SelectListItem>();
				lPhong = context.loaiPhong.Select(x => new SelectListItem { Text = x.tenLoaiPhong, Value = x.Id.ToString() }).ToList();
				dTro = context.dayTro.Select(x => new SelectListItem { Text = x.tenDayTro, Value = x.Id.ToString() }).ToList();
				ViewBag.LoaiPhong = lPhong;
				ViewBag.DayTro = dTro;
				return View();
            }
            return View("~/Areas/Admin/Views/Share/PageNotFound.cshtml");
		}
		[Route("Admin/[controller]/[action]")]
		[HttpPost]
		public async Task <IActionResult> Create(Phong phong)
		{
			context.Add(phong);
			await context.SaveChangesAsync();
			toast.AddSuccessToastMessage("Thêm thành công phòng mới", new ToastrOptions { Title = "Thông báo" });
			return RedirectToAction("Index");
		}
		[Route("Admin/[controller]/[action]")]
		public async Task<IActionResult> Update(int id)
		{
            if (CheckLogin())
            {
				Phong phg = await context.phong.Where(x => x.Id == id).FirstOrDefaultAsync();
				List<SelectListItem> Lphong= new List<SelectListItem>();
				List<SelectListItem> Dtro = new List<SelectListItem>();
				Lphong = context.loaiPhong.Select(x => new SelectListItem { Text = x.tenLoaiPhong, Value = x.Id.ToString() }).ToList();
				Dtro = context.dayTro.Select(x => new SelectListItem { Text = x.tenDayTro, Value = x.Id.ToString()}).ToList();
				ViewBag.LoaiPhong = Lphong;
				ViewBag.DayTro = Dtro;
				return View(phg);
            }
            return View("~/Areas/Admin/Views/Share/PageNotFound.cshtml");
		}
		[HttpPost]
		[Route("Admin/[controller]/[action]")]
		public async Task<IActionResult> Update(Phong phong)
		{
			context.Update(phong);	
			await context.SaveChangesAsync();
			toast.AddSuccessToastMessage("Cập nhật thành công phòng", new ToastrOptions { Title = "Thông báo" });
			return RedirectToAction("Index");
		}
		[HttpPost]
		[Route("Admin/[controller]/[action]")]
		public async Task<IActionResult> Delete(int id)
		{
			try
			{
				var phg = new Phong { Id = id };
				context.Remove(phg);
				await context.SaveChangesAsync();
				toast.AddSuccessToastMessage("Xóa thành công phòng", new ToastrOptions { Title = "Thông báo" });
			}
			catch 
			{
				toast.AddErrorToastMessage("Không thể xóa phòng này", new ToastrOptions { Title = "Thông báo" });
			}
			return RedirectToAction("Index");
		}
		[HttpPost]
		[Route("Admin/[controller]/[action]")]
		public IActionResult Search(string tenLoaiPhong, string tenDayTro, Nullable<bool> trangThai)
		{
			List<SelectListItem> loaiPhong = context.loaiPhong.Select(x => new SelectListItem { Text = x.tenLoaiPhong, Value = x.tenLoaiPhong }).ToList();
			List<SelectListItem> dayTro = context.dayTro.Select(x => new SelectListItem { Text = x.tenDayTro, Value = x.tenDayTro.ToString() }).ToList();
			ViewBag.loaiPhong = loaiPhong;
			ViewBag.dayTro = dayTro;
			if (string.IsNullOrEmpty(tenDayTro) && !string.IsNullOrEmpty(tenLoaiPhong))
			{
				if (trangThai != null)
				{
					var phong = context.phong.Include(x => x.loaiPhong).Include(y => y.dayTro).Where(p => p.loaiPhong.tenLoaiPhong.ToLower().Contains(tenLoaiPhong.ToLower()) && p.conPhong == trangThai).ToList().ToPagedList(1, 10);
					return View("Index", phong);
				}
				else
				{
					var phong = context.phong.Include(x => x.loaiPhong).Include(y => y.dayTro).Where(p => p.loaiPhong.tenLoaiPhong.ToLower().Contains(tenLoaiPhong.ToLower())).ToList().ToPagedList(1, 10);
					return View("Index", phong);
				}
			}
			else if(string.IsNullOrEmpty(tenLoaiPhong) && !string.IsNullOrEmpty(tenDayTro))
			{
				if (trangThai != null)
				{
					var phong = context.phong.Include(x => x.loaiPhong).Include(y => y.dayTro).Where(p => p.dayTro.tenDayTro.ToLower().Contains(tenDayTro.ToLower()) && p.conPhong == trangThai).ToList().ToPagedList(1, 10);
					return View("Index", phong);
				}
				else
				{
					var phong = context.phong.Include(x => x.loaiPhong).Include(y => y.dayTro).Where(p => p.dayTro.tenDayTro.ToLower().Contains(tenDayTro.ToLower())).ToList().ToPagedList(1, 10);
					return View("Index", phong);
				}

			}
			else if(!string.IsNullOrEmpty(tenLoaiPhong) && !string.IsNullOrEmpty(tenDayTro))
			{
				if (trangThai != null)
				{
					var phong = context.phong.Include(x => x.loaiPhong).Include(y => y.dayTro).Where(p => p.loaiPhong.tenLoaiPhong.ToLower().Contains(tenLoaiPhong.ToLower()) && p.dayTro.tenDayTro.ToLower().Contains(tenDayTro.ToLower()) && p.conPhong == trangThai).ToList().ToPagedList(1, 10);
					return View("Index", phong);
				}
				else
				{
					var phong = context.phong.Include(x => x.loaiPhong).Include(y => y.dayTro).Where(p => p.loaiPhong.tenLoaiPhong.ToLower().Contains(tenLoaiPhong.ToLower()) && p.dayTro.tenDayTro.ToLower().Contains(tenDayTro.ToLower())).ToList().ToPagedList(1, 10);
					return View("Index", phong);
				}
			}
			else
			{
				if (trangThai != null)
				{
					var phong = context.phong.Include(d => d.loaiPhong).Include(e => e.dayTro).Where(p => p.conPhong == trangThai).ToPagedList(1, 10);
					return View("Index", phong);
				}
				else
				{
					var phong = context.phong.Include(d => d.loaiPhong).Include(e => e.dayTro).ToPagedList(1, 10);
					return View("Index", phong);
				}
			}
		}
	}
}
